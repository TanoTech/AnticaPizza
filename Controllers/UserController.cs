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

      
        public ActionResult TuoiOrdini()
        {
            int userId = (int)Session["UserID"];

            var userOrders = db.Histories
                .Where(c => c.UserID == userId)
                .ToList();

            return View(userOrders);
        }

        public ActionResult Storico()
        {
            int userId = (int)Session["UserID"];

            var userOrders = db.Histories
                .Where(c => c.UserID == userId && c.Stato == false)
                .ToList();

            return View(userOrders);
        }

        public ActionResult UpdateAddress()
        {
            int userId = (int)Session["UserID"];
            var user = db.Users.FirstOrDefault(u => u.ID == userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index", "Carts");
        }

        [HttpGet]

        public JsonResult GetAddress()
        {
            int userId = (int)Session["UserID"];
            var user = db.Users.FirstOrDefault(u => u.ID == userId);

            if (user != null)
            {
                return Json(new { success = true, indirizzoConsegna = user.IndirizzoConsegna }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAddress(string indirizzoConsegna)
        {
            if (ModelState.IsValid)
            {
                int userId = (int)Session["UserID"];
                var existingUser = db.Users.FirstOrDefault(u => u.ID == userId);

                if (existingUser == null)
                {
                    return HttpNotFound();
                }

                existingUser.IndirizzoConsegna = indirizzoConsegna;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}