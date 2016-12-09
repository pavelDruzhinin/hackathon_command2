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
using PagedList;

namespace GameShop.Controllers
{
    public class OrdersController : Controller
    {
        private GameShopContext db = new GameShopContext();

        // GET: Orders
        [Authorize(Roles = "admin")]
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            var orders = db.Orders.Include(o => o.Customer).Where(c => c.Current == true);
            return View(orders.ToList().ToPagedList(pageNumber, 10));
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Include(o => o.Customer).Include(o => o.OrderPositions.Select(op => op.Game)).FirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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

        public ActionResult AddToCart(int id)
        {
            var games = db.Games.FirstOrDefault(x => x.Id == id);
            var currentOrder = db.Orders.Include(x => x.OrderPositions).FirstOrDefault(x => x.Current && x.Customer.Login == User.Identity.Name);

            if (currentOrder == null)
            {
                currentOrder = new Order
                {
                    Customer = db.Customers.FirstOrDefault(x => x.Login == User.Identity.Name),
                    Current = true,
                    OrderPositions = new List<OrderPosition>
                    {
                        new OrderPosition { Game = games }
                    }
                };

                db.Orders.Add(currentOrder);
                db.SaveChanges();
            }
            else
            {
                var orderPosition = currentOrder.OrderPositions.FirstOrDefault(x => x.Game == games);
                if (orderPosition == null)
                    currentOrder.OrderPositions.Add(new OrderPosition { Game = games });
            }
            db.SaveChanges();

            setGamesInCart(currentOrder.OrderPositions.Count); // пересчет игр в корзине и добавление в куки

            return RedirectToAction("Cart", "Orders");
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

            setGamesInCart(db.OrderPositions.Where(o => o.OrderId == orderPosition.OrderId).Count());// пересчет игр в корзине и добавление в куки

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

            setGamesInCart(0); // обнуление количества игр в корзине в куки

            return RedirectToAction("Profile", "Account");
        }

        public void setGamesInCart(int gamesInCart)
        {
            var gamesInCartCookie = new HttpCookie("gamesInCart");
            gamesInCartCookie.Value = gamesInCart.ToString();
            gamesInCartCookie.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Add(gamesInCartCookie);
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
