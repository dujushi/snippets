using System.Web.Mvc;
using CamelCase.CustomResult;
using CamelCase.Models;

namespace CamelCase.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var product = new Product
            {
                FancyName = "iPad Pro",
                SalesPrice = 12.10m
            };
            return this.JsonCamelCase(product);
        }
    }
}