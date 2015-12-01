using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HtmlAction.Models;

namespace HtmlAction.Controllers
{
    public class TaskController : Controller
    {
        private TaskDbContext db = new TaskDbContext();

        public ActionResult List(int page = 1)
        {
            var model = db.Tasks.OrderByDescending(t => t.Id).AsQueryable();
            int totalItems = model.Count();
            int itemsPerPage = 10;
            model = model
                .Skip(itemsPerPage*(page - 1))
                .Take(itemsPerPage);

            ViewBag.Pagination = new Pagination
            {
                TotalItems = totalItems,
                ItemsPerPage = itemsPerPage,
                CurrentPage = page,
                PageUrl = x => Url.Action("List", new {page = x})
            };

            return View(model);
        }
    }
}
