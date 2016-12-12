using System.Web.Mvc;
using System.Web.Security;
using GameShop.Services;
using GameShop.ViewModels;
using GameShop.DataAccess;
using GameShop.Models;
using System.Linq;
using System;
using System.Data.Entity;
using System.Net;
using PagedList;

namespace GameShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private GameShopContext db = new GameShopContext();

        // GET: Admin
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            return View(db.PurchasedGames.Include(p => p.Customer).OrderByDescending(x => x.Time).ToList().ToPagedList(pageNumber, 10));
        }
        // GET: Admin/TopSales
        public ActionResult SalesTop(int? page)
        {
            int pageNumber = (page ?? 1);
            var TopSales = db.PurchasedGames.GroupBy(x => x.Name).OrderByDescending(o => o.Count()).ToList().Select(g => new SalesViewModel { Name = g.Key, Count = g.Count(), GameId = g.ToList()[0].GameId });
            return View(TopSales.ToList().ToPagedList(pageNumber, 10));

        }
        public ActionResult SalesByDate(int? page)
        {
            int pageNumber = (page ?? 1);
            var SalesByDate = db.PurchasedGames.GroupBy(x => DbFunctions.TruncateTime(x.Time)).OrderByDescending(o => o.Key).ToList().Select(g => new SalesViewModel { Time = Convert.ToDateTime(g.Key), Count = g.Count() });
            return View(SalesByDate.ToList().ToPagedList(pageNumber, 10));

        }
    }
}