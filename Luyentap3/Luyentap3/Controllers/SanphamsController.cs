using Luyentap3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Luyentap3.Controllers
{
    public class SanphamsController : Controller
    {
        private Model1 db = new Model1();
        // GET: Sanphams
        public ActionResult Index(string searchString)
        {
            var sanphams = db.Sanpham.Select(s => s);
            if (!String.IsNullOrEmpty(searchString))
            {
                String x = Request.Form["searchType"];
                if (x == "pname")
                {
                    sanphams = db.Sanpham.Where(s => s.Tenvd.Contains(searchString));
                }
                else
                {
                    int searchInt = int.Parse(searchString);
                    sanphams = db.Sanpham.Where(s => s.Giatien < searchInt);
                }
            }
            return View(sanphams.ToList());
        }
        //------------------------------------------------DETAIL------------------------------------------------
        public ActionResult Detail(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanpham.Find(id);
            if(sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }
        //------------------------------------------------CREATE------------------------------------------------
        public ActionResult Create()
        {
            ViewBag.MaDanhmuc = new SelectList(db.Danhmuc, "MaDanhmuc", "TenDanhmuc");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="Madv,Tendv, Mota, TenAnh,Giatien,Soluong,MaDanhmuc")] Sanpham sanpham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sanpham.TenAnh = "";
                    var f = Request.Files["ImageFile"];
                    if(f == null && f.ContentLength > 0)
                    {
                        string fileName = System.IO.Path.GetFileName(f.FileName);
                        string uploadFile = Server.MapPath("~/Images/" + fileName);
                        f.SaveAs(uploadFile);
                        sanpham.TenAnh = fileName;
                    }
                    db.Sanpham.Add(sanpham);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }catch(Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;
                ViewBag.MaDanhmuc = new SelectList(db.Danhmuc, "MaDanhmuc", "TenDanhmuc", sanpham.MaDanhmuc);
                return View(sanpham);
            }
        }
        //------------------------------------------------DELETE-------------------------------------------------------
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanpham.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sanpham sp = db.Sanpham.Find(id);
            db.Sanpham.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //------------------------------------------------EDIT------------------------------------------------

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanpham.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDanhmuc = new SelectList(db.Danhmuc, "MaDanhmuc", "TenDanhmuc", sanpham.MaDanhmuc);
            return View(sanpham);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include ="Mavd,Tenvd, Mota, TenAnh,Giatien,Soluong,MaDanhmuc")]Sanpham sanpham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(sanpham).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }catch(Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;
                ViewBag.MaDanhmuc = new SelectList(db.Danhmuc, "MaDanhmuc", "TenDanhmuc", sanpham.MaDanhmuc);
                return View(sanpham);
            }
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