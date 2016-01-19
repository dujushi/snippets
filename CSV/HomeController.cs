using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Demo.Models;
using FileHelpers;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        public FileResult Csv()
        {
            var data = new StringBuilder();
            data.AppendLine("Id,Name");
            foreach (var product in getProducts())
                data.AppendLine($"{product.Id},{product.Name}");
            var bytes = Encoding.UTF8.GetBytes(data.ToString());
            return File(bytes, "text/csv", "Products.csv");
        }

        public FileResult FileHelperCsv()
        {
            var engine = new FileHelperEngine<Product> {HeaderText = "Id,Name"};
            var data = engine.WriteString(getProducts());
            var bytes = Encoding.UTF8.GetBytes(data);
            return File(bytes, "text/csv", "Products.csv");
        }

        private List<Product> getProducts()
        {
            return Enumerable.Range(0, 1000).Select(x => new Product {Id = x, Name = $"Name {x}"}).ToList();
        }
    }
}