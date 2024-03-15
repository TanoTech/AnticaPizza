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

        public ActionResult Procedi()
        {
            int userId = (int)Session["UserID"];

            var cartItems = db.Carts.Where(c => c.UserID == userId).ToList();

            if (cartItems.Any())
            {
                decimal totalPrice = cartItems.Sum(c => c.Menu.Prezzo * c.Quantita);

                int orderNumber = GenerateOrderNumber(); 

                foreach (var cartItem in cartItems)
                {
                    History orderHistory = new History
                    {
                        UserID = cartItem.UserID,
                        MenuID = cartItem.MenuID,
                        Prezzo = cartItem.Menu.Prezzo * cartItem.Quantita,
                        Quantita = cartItem.Quantita,
                        Stato = true,
                        NumeroOrdine = orderNumber, 
                        DataOrdine = DateTime.Now
                    };

                    db.Histories.Add(orderHistory); 
                }

                db.Carts.RemoveRange(cartItems);
                db.SaveChanges();

                return RedirectToAction("Index", "History");
            }

            return RedirectToAction("Index", "Carts");
        }

        private int GenerateOrderNumber()
        {
            var lastOrder = db.Histories.OrderByDescending(h => h.ID).FirstOrDefault();

            if (lastOrder == null)
            {
                return 1;
            }

            return lastOrder.NumeroOrdine + 1;
        }

    }

}