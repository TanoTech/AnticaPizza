using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnticaPizza.Models;

namespace AnticaPizza.Controllers
{
    public class CartsController : Controller
    {
        private PizzaDBContext db = new PizzaDBContext();

        public ActionResult Index()
        {
             int userId = (int)Session["UserID"];

             var cartItems = db.Carts
                    .Where(c => c.UserID == userId)
                    .Include(c => c.Menu)
                    .ToList();

             return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int menuId)
        {
                int userId = (int)Session["UserID"];

                Cart newCartItem = new Cart
                {
                    UserID = userId,
                    MenuID = menuId,
                    Quantita = 1
                };

                db.Carts.Add(newCartItem);
                db.SaveChanges();

            return RedirectToAction("Index", "Carts");
        }
    }
}