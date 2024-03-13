using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AnticaPizza.Models;

namespace AnticaPizza.Controllers
{
    public class AdminController : Controller
    {
        private PizzaDBContext db = new PizzaDBContext();

        public async Task<ActionResult> Index()
        {

            var cartsInProgress = await db.Carts
                .Include(c => c.User)
                .Include(c => c.Menu)
                .Where(c => c.Stato)
                .GroupBy(c => c.UserID)
                .Select(group => new SingleCart
                {
                    UserID = group.Key,
                    Username = group.FirstOrDefault().User.Username,
                    Carts = group.ToList()
                })
                .ToListAsync();

            return View(cartsInProgress);
        }

        [HttpPost]
        public async Task<ActionResult> Evaso(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cart = await db.Carts.FindAsync(id);

            if (cart == null)
            {
                return HttpNotFound();
            }

            cart.Stato = false;

            await db.SaveChangesAsync();

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
