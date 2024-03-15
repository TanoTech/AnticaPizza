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
        public async Task<ActionResult> SetAllEvasoForOrder(int numeroOrdine, string returnUrl)
        {
            var orders = await db.Histories.Where(h => h.NumeroOrdine == numeroOrdine && h.Stato).ToListAsync();
            if (orders != null && orders.Any())
            {
                foreach (var order in orders)
                {
                    order.Stato = false;
                }
                await db.SaveChangesAsync();
            }
            return Redirect(returnUrl);
        }

        public async Task<ActionResult> Storico()
        {
            var ordersEvasi = await db.Histories
                .Include(h => h.User)
                .Include(h => h.Menu)
                .Where(h => !h.Stato) 
                .ToListAsync();

            return View(ordersEvasi);
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
