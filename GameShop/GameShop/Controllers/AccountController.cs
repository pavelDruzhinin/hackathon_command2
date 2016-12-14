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

namespace GameShop.Controllers
{
    public class AccountController : Controller
    {
        private AccountService _accountService;

        public AccountController()
        {
            _accountService = new AccountService();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewData["registrationSuccess"] = TempData["registrationSuccess"];
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (_accountService.Login(model.Login, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.Login, true);

                //ниже пересчет игр в корзине и добавление в куки
                var currentOrder = db.Orders.Include(x => x.OrderPositions).FirstOrDefault(x => x.Current && x.Customer.Login == model.Login);
                var gamesInCartCookie = new System.Web.HttpCookie("gamesInCart");
                if (currentOrder != null)
                {
                    gamesInCartCookie.Value = currentOrder.OrderPositions.Count.ToString();
                }
                else
                {
                    gamesInCartCookie.Value = "0";
                }
                    gamesInCartCookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(gamesInCartCookie);
                
                return RedirectToAction("Index", "Games");
            }

            ModelState.AddModelError("", "Имя пользователя и пароль были введены неверно. Либо ваш пользователь не зарегистрирован.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Games");
        }
        private GameShopContext db = new GameShopContext();

        // GET: Customers/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Customers/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer customer)
        {

            if (db.Customers.FirstOrDefault() != null)
            {
                var loginExists = db.Customers.Any(x => x.Login == customer.Login);
                var emailExists = db.Customers.Any(x => x.Email == customer.Email);
                if (loginExists)
                {
                    ViewData["loginExistsMessage"] = "Такой логин уже занят!";
                }
                if (emailExists)
                {
                    ViewData["emailExistsMessage"] = "Пользователь с таким e-mail уже зарегистрирован";
                }
                if ((loginExists == false) && (emailExists == false) && ModelState.IsValid)
                {
                    customer.RoleId = 1;
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    TempData["registrationSuccess"] = "Регистрация успешна!";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                //customer.RoleId = 3;
                db.Customers.Add(customer);
                db.SaveChanges();
                TempData["registrationSuccess"] = "Регистрация успешна!";
                return RedirectToAction("Login");
            }
            return View();
        }

        //GET /Account/Profile
        public new ActionResult Profile()
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            var customer = db.Customers.Include(x => x.PurchasedGames).FirstOrDefault(x => x.Login == User.Identity.Name);
            return View(customer);
        }

        //GET /Account/Edit
        public ActionResult Edit()
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            var customer = db.Customers.FirstOrDefault(x => x.Login == User.Identity.Name);
            return View(customer);
        }

        //POST /Account/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile");
            }
            return View(customer);
        }

        // GET: /Account/Delete
        public ActionResult Delete()
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            var customer = db.Customers.FirstOrDefault(x => x.Login == User.Identity.Name);
            return View(customer);
        }

        // POST: /Account/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            if (User.Identity.IsAuthenticated == false)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            var customer = db.Customers.FirstOrDefault(x => x.Login == User.Identity.Name);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return Logout();
        }
    }
}