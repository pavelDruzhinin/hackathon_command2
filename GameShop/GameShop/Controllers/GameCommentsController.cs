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
    public class GameCommentsController : Controller
    {
        private GameShopContext db = new GameShopContext();

        // GET: GameComments
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var gameComments = db.GameComments.Include(g => g.Customer).Include(g => g.Game);
            return View(gameComments.ToList());
        }

        // GET: GameComments/Create
        public ActionResult Create(int? id)
        {
            if (User.Identity.IsAuthenticated == false) 
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewData["Game"] = db.Games.Find(id).Name;

            return View();
        }

        // POST: GameComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GameComment gameComment, int id)
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            if (ModelState.IsValid)
            {
                gameComment.Customer = db.Customers.FirstOrDefault(x => x.Login == User.Identity.Name);
                gameComment.Time = DateTime.Now;
                gameComment.Game = db.Games.Find(id);
                db.GameComments.Add(gameComment);
                UpdateRating(id);
                db.SaveChanges();
                return RedirectToAction("Details", "Games", new { id = gameComment.GameId });
            }

            return View(gameComment);
        }

        // GET: GameComments/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            GameComment gameComment = db.GameComments.Find(id);
            if (gameComment == null)
                return HttpNotFound();
            return View(gameComment);
        }

        // POST: GameComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(GameComment gameComment, int id)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gameComment).State = EntityState.Modified;
                UpdateRating(gameComment.GameId);
                db.SaveChanges();
                return RedirectToAction("Details", "Games", new { id = gameComment.GameId });
            }
            return View(gameComment);
        }

        // GET: GameComments/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            GameComment gameComment = db.GameComments.Find(id);
            if (gameComment == null)
                return HttpNotFound();
            return View(gameComment);
        }

        // POST: GameComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            GameComment gameComment = db.GameComments.Find(id);
            db.GameComments.Remove(gameComment);
            UpdateRating(gameComment.GameId);
            db.SaveChanges();
            return RedirectToAction("Details", "Games", new {id = gameComment.GameId });
        }
        
        public void UpdateRating(int id)
        {
            var game = db.Games.Include(o => o.GameComments).FirstOrDefault(x => x.Id == id);
            if (game.GameComments != null)
            {
                int ratingSum = 0;
                int ratingCount = 0;
                foreach (var gameComment in game.GameComments)
                {
                    ratingSum += gameComment.Rating;
                    ratingCount++;
                }
                game.Rating = ratingSum / ratingCount;
                db.Entry(game).State = EntityState.Modified;
            }
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
