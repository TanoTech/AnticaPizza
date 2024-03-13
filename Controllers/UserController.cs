using System;
using System.Linq;
using System.Web.Mvc;
using AnticaPizza.Models;

namespace AnticaPizza.Controllers
{
    public class UserController : Controller
    {
        private PizzaDBContext db = new PizzaDBContext();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {

                if (db.Users.Any(u => u.Username == user.Username || u.Email == user.Email))
                {
                    ModelState.AddModelError("", "L'utente con lo stesso nome utente o email esiste già.");
                    return View(user);
                }

                db.Users.Add(user);
                db.SaveChanges();

                Session["UserID"] = user.ID;

                return RedirectToAction("Index", "Menu");
            }

            return View(user);
        }
    }
}
