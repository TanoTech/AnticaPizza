using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AnticaPizza.Models;

namespace AnticaPizza.Controllers
{
    public class MenuController : Controller
    {
        private PizzaDBContext db = new PizzaDBContext();

        public ActionResult Index()
        {
            var menuItems = db.Menus.ToList();

            return View(menuItems);
        }

        private int GetNextImageNumber()
        {
            int maxNumber = 0;

            foreach (var fileName in GetUploadedFiles())
            {
                string name = Path.GetFileNameWithoutExtension(fileName);
                if (int.TryParse(name.Substring(3), out int number) && number > maxNumber)
                {
                    maxNumber = number;
                }
            }

            return maxNumber + 1;
        }

        private List<string> GetUploadedFiles()
        {
            string uploadFolderPath = Server.MapPath("~/Uploads");
            string[] fileNames = Directory.GetFiles(uploadFolderPath);
            return fileNames.Select(Path.GetFileName).ToList();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Menu menu, HttpPostedFileBase imgUrl)
        {
            if (ModelState.IsValid)
            {
                if (imgUrl != null && imgUrl.ContentLength > 0)
                {
                    var productName = menu.Nome;
                    int nextImageNumber = GetNextImageNumber();
                    var fileName = productName + nextImageNumber.ToString("00") + Path.GetExtension(imgUrl.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    imgUrl.SaveAs(path);
                    menu.ImgUrl = fileName;
                }

                db.Menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menu);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Menu menu = db.Menus.Find(id);

            if (menu == null)
            {
                return HttpNotFound();
            }

            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu menu, HttpPostedFileBase imgUrl)
        {
            if (ModelState.IsValid)
            {
                var oldFileName = menu.ImgUrl;

                if (imgUrl != null && imgUrl.ContentLength > 0)
                {
                    var productName = menu.Nome;
                    int nextImageNumber = GetNextImageNumber();
                    var fileName = productName + nextImageNumber.ToString("00") + Path.GetExtension(imgUrl.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    imgUrl.SaveAs(path);
                    menu.ImgUrl = fileName;
                }

                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();

                if (!string.IsNullOrEmpty(oldFileName))
                {
                    var oldFilePath = Path.Combine(Server.MapPath("~/Uploads"), oldFileName);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                return RedirectToAction("Index");
            }

            return View(menu);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Menu menu = db.Menus.Find(id);

            if (menu == null)
            {
                return HttpNotFound();
            }

            return View(menu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
            db.SaveChanges();
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
