using System.Threading.Tasks;
using System.Web.Mvc;
using GoogleDistanceMatrix.Services;

namespace GoogleDistanceMatrix.Controllers
{
    public class DefaultController : Controller
    {
        public async Task<ActionResult> Index()
        {
            GoogleDistanceMatrixApi api = new GoogleDistanceMatrixApi(new [] { "Auckland Airport" } , new [] { "Corner Princes Street and Waterloo Quadrant" });
            var response = await api.GetResponse();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}