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
    public class OrdersController : Controller
    {
        private GameShopContext db = new GameShopContext();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customer);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName");
            return View();
        }

        // POST: Orders/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Cart()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            var customer = db.Customers.FirstOrDefault(x => x.Login == User.Identity.Name);
            var order = db.Orders.Include(x => x.OrderPositions).Include(x => x.OrderPositions.Select(g => g.Game).Select(c => c.Category)).FirstOrDefault(x => x.CustomerId == customer.Id && x.Current);

            if (order == null)
            {          
                order = new Order
                {
                    Customer = customer,
                    Current = true,
                    OrderPositions = new List<OrderPosition>()
                };
                db.Orders.Add(order);
                db.SaveChanges();          
            }

            return View(order);
        }

        public ActionResult Remove(int id)
        {
            var orderPosition = db.OrderPositions.FirstOrDefault(x => x.Id == id);

            if (orderPosition == null)
            {
                return RedirectToAction("Cart");    
            }

            db.OrderPositions.Remove(orderPosition);
            db.SaveChanges();
            return RedirectToAction("Cart");
        }

        public ActionResult Pay()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            var customer = db.Customers.FirstOrDefault(x => x.Login == User.Identity.Name);
            var currentOrder = db.Orders.Include(o => o.OrderPositions.Select(op => op.Game)).FirstOrDefault(x => x.CustomerId == customer.Id && x.Current);
            var rand = new Random();
            foreach (var orderPosition in currentOrder.OrderPositions)
            {
                var purchasedGame = new PurchasedGame();
                purchasedGame.Name = orderPosition.Game.Name;
                purchasedGame.Price = orderPosition.Game.Price;
                purchasedGame.GameId = orderPosition.Game.Id;
                
                purchasedGame.Key = rand.Next(1000, 9999).ToString() + '-' + rand.Next(1000, 9999).ToString() + '-' + rand.Next(1000, 9999).ToString();
                purchasedGame.Time = DateTime.Now;
                purchasedGame.CustomerId = customer.Id;
                db.PurchasedGames.Add(purchasedGame);
            }
            currentOrder.Current = false;
            db.SaveChanges();
            return RedirectToAction("Profile", "Account");
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
