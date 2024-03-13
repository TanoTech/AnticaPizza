using System.Linq;
using System.Web.Mvc;
using AnticaPizza.Models;

namespace AnticaPizza.Controllers
{
    public class LoginController : Controller
    {
        private PizzaDBContext db = new PizzaDBContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string usernameOrEmail, string password)
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Menu");
            }

            var user = db.Users.FirstOrDefault(u => (u.Username == usernameOrEmail || u.Email == usernameOrEmail) && u.Password == password);

            if (user != null)
            {
                if (user.Role == "admin")
                {
                    Session["AdminID"] = user.ID;
                }
                else
                {
                    Session["UserID"] = user.ID;
                }

                return RedirectToAction("Index", "Menu");
            }

            ViewBag.ErrorMessage = "Credenziali non valide";
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Menu");
        }
    }
}
