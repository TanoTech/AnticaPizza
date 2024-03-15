using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            var user = db.Users.FirstOrDefault(u => u.ID == userId);

            if (cartItems.Any())
            {
                string userIndirizzo = user.IndirizzoConsegna;

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
                        Note = cartItem.Note,
                        Stato = true,
                        IndirizzoConsegna = userIndirizzo,
                        NumeroOrdine = orderNumber, 
                        DataOrdine = DateTime.Now
                    };

                    db.Histories.Add(orderHistory); 
                }

                db.Carts.RemoveRange(cartItems);
                db.SaveChanges();

                return RedirectToAction("TuoiOrdini", "User");
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

        public ActionResult RemoveFromCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cartItem = db.Carts.Find(id);

            if (cartItem == null)
            {
                return HttpNotFound();
            }

            db.Carts.Remove(cartItem);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public JsonResult GetCartItemNotes(int cartItemId)
        {
            var cartItem = db.Carts
                            .Include(c => c.Menu)
                            .SingleOrDefault(c => c.ID == cartItemId);

            if (cartItem != null)
            {
                return Json(new { success = true, notes = cartItem.Note });
            }
            else
            {
                return Json(new { success = false, errorMessage = "Elemento del carrello non trovato." });
            }
        }

        [HttpPost]
        public ActionResult AddNoteToCart(int cartItemId, string note)
        {
            var cartItem = db.Carts.Find(cartItemId);

            if (cartItem == null)
            {
                return HttpNotFound();
            }

            cartItem.Note = note;
            db.Entry(cartItem).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true, notes = cartItem.Note });
        }
    }
}