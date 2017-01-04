using System;
using FestivalWeb.Models;
using FestivalWeb.SystemHelper;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FestivalWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataContext db = new DataContext();
        private readonly FileUploaderService fileuploadService = new FileUploaderService();
        public ActionResult Index()
        {
            Users user = Session["Current_User"] as Users;
            if (user == null || !user.IsAdmin)
                return RedirectToAction("LogIn", "Admin");

            return View();
        }

        #region Users için Actionlar
        /// <summary>
        ///  Kullanıcı listesi action'u
        /// </summary>
        public ActionResult Users(string res)
        {
            Users user = Session["Current_User"] as Users;
            if (user == null || !user.IsAdmin)
                return RedirectToAction("LogIn", "Admin");

            var list = db.Users.Where(c => !c.IsDeleted).ToList();

            if(!res.IsNullOrEmpty())
                ViewBag.Res = res;
            else
                ViewBag.Res = JsonConvert.SerializeObject(new {});

            return View(list);
        }
        /// <summary>
        /// Kullanıcı Ekleme veya Editleme action'u
        /// </summary>
        public ActionResult AddEditUser(Guid? ID)
        {
            Users model;
            try
            {
                Users user = Session["Current_User"] as Users;
                if (user == null || !user.IsAdmin)
                    return RedirectToAction("LogIn", "Admin");

                if (!ID.HasValue)
                {
                    model = new Models.Users();
                }
                else
                {
                    model = db.Users.Where(c => !c.IsDeleted && c.ID.Equals(ID.Value)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return Json("Hata - " + ex.Message,JsonRequestBehavior.AllowGet);
            }
            return View(model);
        }
        /// <summary>
        /// Kullanıcı Ekleme veya Editleme action'u POST methodu
        /// </summary>
        [HttpPost]
        public ActionResult AddEditUser(Users item, HttpPostedFileBase Resim)
        {
            try
            {
                Users user = Session["Current_User"] as Users;
                if (user == null || !user.IsAdmin)
                    return RedirectToAction("LogIn", "Admin");

                if (item.ID == Guid.Empty)
                {
                    item.ID = Guid.NewGuid();
                    item.InsertDate = DateTime.Now;
                    item.IsDeleted = false;

                    if (Resim != null)
                        item.Avatar = fileuploadService.AddPicture(Resim, "Users", HttpContext);
                    else
                        item.Avatar = null;
                    db.Users.Add(item);
                }
                else
                {
                    var data = db.Users.Where(c => !c.IsDeleted && c.ID.Equals(item.ID)).FirstOrDefault();

                    if (Resim != null)
                        item.Avatar = fileuploadService.AddPicture(Resim, "Users", HttpContext);
                    else
                        item.Avatar = null;

                    data.Email = item.Email;
                    data.IsActive = item.IsActive;
                    data.IsAdmin = item.IsAdmin;
                    data.Password = item.Password;
                    data.Name = item.Name;
                    data.Surname = item.Surname;
                    data.BirthDay = item.BirthDay;
                    data.Avatar = item.Avatar;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                object result = new {
                    messages = "Kayıt başarılı!",
                    success = true
                };
                return RedirectToAction("Users", "Admin", new { res = JsonConvert.SerializeObject(result) });
            }
            catch (Exception ex)
            {
                object result = new
                {
                    messages = "Hata! - " + ex.Message,
                    success = false
                };
                return RedirectToAction("Users", "Admin", new { res = JsonConvert.SerializeObject(result) });
            }

        }
        /// <summary>
        /// Kullanıcı Silme action'u POST methodu
        /// </summary>
        [HttpPost]
        public ActionResult DeleteUser(Guid ID)
        {
            try
            {
                db.Database.ExecuteSqlCommand("Update Users Set IsDeleted = 1 Where ID ='" + ID + "'");
                return Json("Kullanıcı silindi");
            }
            catch (Exception ex)
            {
                return Json("Hata - " + ex.Message);
            }
        }
        #endregion


        #region Festivaller için Actionlar
        public ActionResult Fests(string res)

        {
            Users user = Session["Current_User"] as Users;
            if (user == null || !user.IsAdmin)
                return RedirectToAction("LogIn", "Admin");

            var list = db.Fests.ToList();

            if (!res.IsNullOrEmpty())
                ViewBag.Res = res;
            else
                ViewBag.Res = JsonConvert.SerializeObject(new { });

            return View(list);
        }        
        public ActionResult AddEditFest(Guid? ID)
        {
            Fests model;
            try
            {
                Users user = Session["Current_User"] as Users;
                if (user == null || !user.IsAdmin)
                    return RedirectToAction("LogIn", "Admin");

                if (!ID.HasValue)
                {
                    model = new Models.Fests();
                }
                else
                {
                    model = db.Fests.Where(c => c.ID.Equals(ID.Value)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return Json("Hata - " + ex.Message, JsonRequestBehavior.AllowGet);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AddEditFest(Fests item, HttpPostedFileBase Resim)
        {
            try
            {
                Users user = Session["Current_User"] as Users;
                if (user == null || !user.IsAdmin)
                    return RedirectToAction("LogIn", "Admin");

                if (item.ID == Guid.Empty)
                {
                    item.ID = Guid.NewGuid();

                    if (item.IsFree)
                        item.Price = null;

                    item.InsertDate = DateTime.Now;
                    if (Resim != null)
                        item.FestPicture = fileuploadService.AddPicture(Resim, "FestPicture", HttpContext);
                    else
                        item.FestPicture = null;

                    db.Fests.Add(item);
                }
                else
                {
                    var data = db.Fests.Where(c => c.ID.Equals(item.ID)).FirstOrDefault();

                    if (Resim != null)
                        item.FestPicture = fileuploadService.AddPicture(Resim, "FestPicture", HttpContext);
                    else
                        item.FestPicture = null;

                    data.Descrition = item.Descrition;
                    data.FinishDate = item.FinishDate;
                    data.IsFree = item.IsFree;

                    if (!data.IsFree)
                        data.Price = item.Price;

                    data.ShortDesc = item.ShortDesc;
                    data.StartDate = item.StartDate;
                    data.Title = item.Title;
                    data.FestPicture = item.FestPicture;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                object result = new
                {
                    messages = "Kayıt başarılı!",
                    success = true
                };
                return RedirectToAction("Fests", "Admin", new { res = JsonConvert.SerializeObject(result) });
            }
            catch (Exception ex)
            {
                object result = new
                {
                    messages = "Hata! - " + ex.Message,
                    success = false
                };
                return RedirectToAction("Fests", "Admin", new { res = JsonConvert.SerializeObject(result) });
            }

        }
        [HttpPost]
        public ActionResult DeleteFest(Guid ID)
        {
            try
            {
                db.Database.ExecuteSqlCommand("Delete From Fests Where ID ='" + ID + "'");
                return Json("Kullanıcı silindi");
            }
            catch (Exception ex)
            {
                return Json("Hata - " + ex.Message);
            }
        }
        #endregion


        #region Oturum Actionları
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(string email, string password)
        {
            try
            {
                if ((!email.IsNullOrEmpty() && email.IsMail()) && !password.IsNullOrEmpty())
                {
                    var result = db.Users.Where(c => !c.IsDeleted && c.IsAdmin && c.IsActive && (c.Email.Equals(email) && c.Password.Equals(password))).FirstOrDefault();
                    if (result != null)
                    {
                        result.Password = "";
                        Session["Current_User"] = result;
                        return Json(new { message = "Yönlendiriliyorsunuz...", result = true });
                    }
                    else
                    {
                        return Json(new { message = "Kullanıcı adı veya şifre yanlış.", result = false });
                    }
                }
                else
                {
                    return Json(new { message = "Kullanıcı adı veya şifre alanı boş olamaz.", result = false });
                }
            }
            catch (Exception ex)
            {
                return Json("Hata - " + ex.Message);
            }

        }
        
        public ActionResult LogOut()
        {
            Users user = Session["Current_User"] as Users;
            if (user == null || !user.IsAdmin)
                return RedirectToAction("LogIn", "Admin");

            Session["Current_User"] = null;

            return RedirectToAction("LogIn", "Admin");
        }
        #endregion


    }
}