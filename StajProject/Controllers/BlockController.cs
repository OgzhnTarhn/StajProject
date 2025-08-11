using StajProject.Models;
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
            var vm = _sapController.GetBlocks(null); // Tüm header'lar
            return View(vm.Headers); // View'a sadece header listesi gönderiyoruz
        }

        // Block detayları
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var vm = _sapController.GetBlocks(id); // Seçilen header + detaylar
            return View(vm);
        }
    }
}
