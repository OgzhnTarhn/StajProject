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

        public ActionResult Index()
        {
            var vm = _sapController.GetBlocks(null); // tüm headerlar
            return View(vm.Headers);
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var vm = _sapController.GetBlocks(id); // seçilen block
            return View(vm);
        }
    }
}
