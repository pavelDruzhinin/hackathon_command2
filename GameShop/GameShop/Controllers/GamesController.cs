using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameShop.DataAccess;
using GameShop.Models;

namespace GameShop.Controllers
{
    public class GamesController : Controller
    {
        private GameShopContext db = new GameShopContext();

        // GET: Games
        public ActionResult Index(string search)
        {
            
            var games = db.Games.Include(g => g.Category);
            return View(games.ToList());
        }

        // GET: Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Games/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Key,CategoryId,Price,Description")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", game.CategoryId);
            return View(game);
        }

        // GET: Games/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", game.CategoryId);
            return View(game);
        }

        // POST: Games/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Key,CategoryId,Price,Description")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", game.CategoryId);
            return View(game);
        }

        // GET: Games/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddToCart(int id)
        {
            var games = db.Games.FirstOrDefault(x => x.Id == id);
            var currentOdrder = db.Orders.Include(x => x.OrderPositions).FirstOrDefault(x => x.Current && x.Customer.Login ==  User.Identity.Name);

            if (currentOdrder == null)
            {
                currentOdrder = new Order
                {
                    Customer = db.Customers.FirstOrDefault(x => x.Login == User.Identity.Name),
                    Current = true,
                    OrderPositions = new List<OrderPosition>
                    {
                        new OrderPosition { Game = games }
                    }
                };

                db.Orders.Add(currentOdrder);
                db.SaveChanges();
            }
            else
            {
                var orderPosition = currentOdrder.OrderPositions.FirstOrDefault(x => x.Game == games);
                if(orderPosition == null)
                currentOdrder.OrderPositions.Add(new OrderPosition { Game = games });
            }
            db.SaveChanges();

            return RedirectToAction("Cart", "Orders", new { id = currentOdrder.Id});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
