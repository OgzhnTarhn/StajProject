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
            try
            {
                var vm = _sapController.GetBlocks(null); // Tüm header'lar
                return View(vm.Headers); // View'a sadece header listesi gönderiyoruz
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = "Block listesi alınırken hata oluştu: " + ex.Message;
                return View(new List<BlockHeaderModel>()); // Boş liste ile devam et
            }
        }

        // Block detayları
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            try
            {
                var vm = _sapController.GetBlocks(id); // Seçilen header + detaylar
                return View(vm);
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = "Block detayları alınırken hata oluştu: " + ex.Message;
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
                TempData["SuccessMessage"] = "Block başarıyla oluşturuldu!";
                return RedirectToAction("Details", new { id = newId });
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", "Block oluşturulurken hata oluştu: " + ex.Message);
                return View(model);
            }
        }

    }
}
