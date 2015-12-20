using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelBinding.Models;

namespace ModelBinding.Controllers
{
    public class HomeController : Controller
    {
        private readonly EfDbContext _db = new EfDbContext();

        // GET: Home
        public ActionResult Index()
        {
            return View(_db.Chores);
        }

        [HttpPost]
        public ActionResult AddChores(ICollection<Chore> chores)
        {
            if (chores.Any())
            {
                _db.Chores.AddRange(chores);
                _db.SaveChanges();
            }
            
            return Json(new { Success = true });
        }
    }
}