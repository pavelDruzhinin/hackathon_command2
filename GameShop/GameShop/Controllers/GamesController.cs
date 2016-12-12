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
using System.IO;

namespace GameShop.Controllers
{
    public class GamesController : Controller
    {
        private GameShopContext db = new GameShopContext();

        // GET: Games
        public ActionResult Index(string search, int? categoryList, bool? Fcategory, int? page)
        {

            var games = db.Games.Include(g => g.Category);

            ViewBag.CategoryList = new SelectList(db.Categories, "Id", "Name");

            if (!string.IsNullOrWhiteSpace(search))
            {
                games = games.Where(x => x.Name.Contains(search));
            }

            if (Fcategory == true)
            {
                games = games.Where(x => x.CategoryId == categoryList);
            }

            int pageNumber = (page ?? 1);
            int gamesPerPage;
            if (Request.Cookies["gamesPerPage"] != null)
            {
                if (Int32.TryParse(Request.Cookies["gamesPerPage"].Value, out gamesPerPage)) { } //сколько отзывов показывать на странице
                else { gamesPerPage = 10; } //по умолчанию
            }
            else { gamesPerPage = 10; } //по умолчанию
            ViewBag.gamesPerPage = gamesPerPage;
            return View(games.OrderByDescending(g => g.Rating).ToList().ToPagedList(pageNumber, gamesPerPage));
        }

        // GET: Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Include(o => o.GameComments).Include(x => x.GameComments.Select(c => c.Customer)).Include(c => c.Category).FirstOrDefault(g => g.Id == id);
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
       // [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Game game, HttpPostedFileBase gamePoster)
        {
            if (ModelState.IsValid)
            {
                if (gamePoster != null && gamePoster.ContentLength > 0)
                {
                    string path = Server.MapPath("~/images/games/");  
                    string pic = Path.GetFileName(gamePoster.FileName);
                    gamePoster.SaveAs(Path.Combine(path, pic));
                    game.GamePosterUrl = Path.Combine("/images/games/", pic);
                }
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", game.CategoryId);
            return View(game);
        }

        // GET: Games/Edit/5
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Game game)
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("Index");
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
