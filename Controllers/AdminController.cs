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
            var ordersInProgress = await db.Histories
                .Include(h => h.User)
                .Include(h => h.Menu)
                .Where(h => h.Stato)
                .ToListAsync();

            return View(ordersInProgress);
        }

        [HttpPost]
        public async Task<ActionResult> Evaso(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = await db.Histories.FindAsync(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            order.Stato = false;

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> SetAllEvasoForUser(int userId)
        {
            var userOrders = await db.Histories.Where(h => h.UserID == userId && h.Stato).ToListAsync();
            if (userOrders != null && userOrders.Any())
            {
                foreach (var order in userOrders)
                {
                    order.Stato = false;
                }
                await db.SaveChangesAsync();
            }
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
