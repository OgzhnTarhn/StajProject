using StajProject.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StajProject.Controllers
{
    public class BlockController : Controller
    {
        private readonly SapController _sapController;

        public BlockController()
        {
            _sapController = new SapController();
        }

        // Block listesi
        public ActionResult Index()
        {
            // Session kontrolü
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var vm = _sapController.GetBlocks(null); // Tüm header'lar
                return View(vm.Headers); // View'a sadece header listesi gönderiyoruz
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = "Error getting block list: " + ex.Message;
                return View(new List<BlockHeaderModel>()); // Boş liste ile devam et
            }
        }

        // Block detayları
        public ActionResult Details(string id)
        {
            // Session kontrolü
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            try
            {
                var vm = _sapController.GetBlocks(id); // Seçilen header + detaylar
                return View(vm);
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = "Error getting block details: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [AdminOnly]
        public ActionResult Create()
        {
            return View(new CreateBlockVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public ActionResult Create(CreateBlockVm model)
        {
            if (!ModelState.IsValid) 
                return View(model);

            try
            {
                // Çok satırlı textbox'tan satırları al
                var lines = new List<string>();
                if (!string.IsNullOrWhiteSpace(model.DetailLines))
                {
                    foreach (var l in model.DetailLines.Split('\n'))
                        if (!string.IsNullOrWhiteSpace(l)) 
                            lines.Add(l.Trim());
                }

                var newId = _sapController.InsertBlock(model.Title, lines);

                // Başarılı → detaya git
                TempData["SuccessMessage"] = "Block created successfully!";
                return RedirectToAction("Details", new { id = newId });
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", "Error creating block: " + ex.Message);
                return View(model);
            }
        }
        [HttpGet]
        [AdminOnly]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");
            var data = _sapController.GetBlocks(id);

            var m = new EditBlockVm
            {
                BlockId = id,
                Title = data.Headers.Count > 0 ? data.Headers[0].Title : "",
                // mevcut detayları textarea'ya koymak istersen:
                DetailLines = string.Join("\n", data.Details.ConvertAll(d => d.LineText))
            };
            return View(m);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AdminOnly]
        public ActionResult Edit(EditBlockVm model)
        {
            if (!ModelState.IsValid) return View(model);

            var lines = ParseLines(model.DetailLines);
            var ok = _sapController.UpdateBlock(model.BlockId, model.Title, model.ReplaceDetails, lines);
            if (!ok) ModelState.AddModelError("", "Update failed");

            return RedirectToAction("Details", new { id = model.BlockId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public ActionResult Delete(string id)
        {
            // Session kontrolü
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(id)) 
                return RedirectToAction("Index");

            try
            {
                var ok = _sapController.DeleteBlock(id);
                if (ok)
                {
                    TempData["SuccessMessage"] = "Trip deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete trip.";
                }
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting trip: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // Helper method to parse lines from textarea
        private List<string> ParseLines(string detailLines)
        {
            var lines = new List<string>();
            if (!string.IsNullOrWhiteSpace(detailLines))
            {
                foreach (var line in detailLines.Split('\n'))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        lines.Add(line.Trim());
                }
            }
            return lines;
        }

    }
}
