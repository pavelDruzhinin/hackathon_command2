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
    public class NewsController : Controller
    {
        private GameShopContext db = new GameShopContext();

        // GET: News
        public ActionResult Index()
        {
            var news = db.News.Include(n => n.NewsFeed);
            return View(news.ToList());
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            //ViewBag.NewsFeedId = new SelectList(db.NewsFeeds, "Id", "Name");
            return View();
        }

        // POST: News/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News news)  // было Create([Bind(Include = "Id,NewsHeader,NewsBody,CreateDate,ModifyDate,NewsFeedId")] News news)
        {
            news.CreateDate = DateTime.Now;     // при создании новости сохраняем дату создания
            news.ModifyDate = DateTime.Now;     // при создании новости дата модификации равна дате создания
            news.NewsFeedId = 1;                // у нас по-умолчанию все новости для ленты с id=1 (используем одну ленту)
            //news.NewsFeedId = 1;                // тест для sourcetree - увидит ли изменения в этом "балыбердинском" контроллере?
            if (ModelState.IsValid)
            {
                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.NewsFeedId = new SelectList(db.NewsFeeds, "Id", "Name", news.NewsFeedId);
            return View(news);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            //ViewBag.NewsFeedId = new SelectList(db.NewsFeeds, "Id", "Name", news.NewsFeedId);
            return View(news);
        }

        // POST: News/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News news) // было Edit([Bind(Include = "Id,NewsHeader,NewsBody,CreateDate,ModifyDate,NewsFeedId")] News news)
        {
            news.ModifyDate = DateTime.Now; // после редактирования сохраняем дату модификации
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.NewsFeedId = new SelectList(db.NewsFeeds, "Id", "Name", news.NewsFeedId);
            return View(news);
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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
