using System;
using System.Linq;
using Demo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class Demo
    {
        [TestMethod]
        public void InClauseMaxValuesWithRawQuery()
        {
            using (var db = new EfDbContext())
            {
                for (var i = 10000; i < 20000; i += 1000)
                {
                    var idArr = Enumerable.Range(0, i).Select(x => Guid.NewGuid()).ToArray();
                    var idStr = string.Join("','", idArr);
                    var sql = $"select count(*) from categories where id in ('{idStr}')";
                    var count = db.Database.SqlQuery<int>(sql).Single();
                    Console.WriteLine($"Guid count: {i}, sql length: {sql.Length}, result: {count}");
                }
            }
        }

        [TestMethod]
        public void InClauseMaxValuesWithEntityFramework()
        {
            using (var db = new EfDbContext())
            {
                for (var i = 10000; i < 20000; i += 1000)
                {
                    var log = "";
                    db.Database.Log = s => log += s;
                    var idArr = Enumerable.Range(0, i).Select(x => Guid.NewGuid()).ToArray();
                    var count = db.Categories.Count(c => idArr.Contains(c.Id));
                    Console.WriteLine($"Guid count: {i}, sql length: {log.Length}, result: {count}");
                }
            }
        }
    }
}
