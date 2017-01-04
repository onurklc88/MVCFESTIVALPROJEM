using FestivalWeb.Models;
using FestivalWeb.SystemHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FestivalWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext db = new DataContext();
        // GET: Site
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult _Slider()
        {
            try
            {
                var list = db.Fests.ToList();

                return PartialView(list);
            }
            catch (Exception)
            {
                return PartialView();
            }
        }
        public PartialViewResult _OurNextEventTime()
        {
            try
            {
                var date = db.Fests.Where(c => c.StartDate >= DateTime.Now).OrderBy(c => c.StartDate).FirstOrDefault();

                return PartialView(date);
            }
            catch (Exception)
            {
                return PartialView();
            }
        }
        public PartialViewResult _Events()
        {
            try
            {
                var list = db.Fests.ToList();
                return PartialView(list);
            }
            catch (Exception)
            {
                return PartialView();
            }
        }
        public ActionResult _Modal(Guid? ID)
        {
            Fests data = new Fests();
            try
            {
                if (ID.HasValue)
                {
                    data = db.Fests.Where(c => c.ID.Equals(ID.Value)).FirstOrDefault();
                    return View(data);
                }

                return View(data);
            }
            catch (Exception)
            {
                return  View(data);
            }
        }

        public PartialViewResult _CommentList(Guid FestID)
        {
            List<UserComments> list = new List<UserComments>();
            try
            {
                if(FestID != Guid.Empty)
                {
                    list = db.UserComments.Where(c => c.FestID.Equals(FestID)).OrderBy(c => c.InsertDate).ToList();
                    foreach (var item in list)
                    {
                        item.User = db.Users.Where(c => c.ID.Equals(item.UserID)).FirstOrDefault();
                    }
                    return PartialView(list);
                }
                return PartialView(list);
            }
            catch (Exception ex)
            {
                return PartialView(list);
            }
        }
        [HttpPost]
        public ActionResult PostComment(Guid FestID,string comment)
        {
            try
            {
                Users user = Session["Current_User"] as Users;
                if (user == null)
                    return Json(new { message = "Giriş yapmadan yorum atamazsınız...", result = true });

                if(comment.IsNullOrEmpty())
                {
                    return Json(new { message = "Boş Yorum atılamaz...", result = false });
                }
                UserComments userComments = new UserComments()
                {
                    ID = Guid.NewGuid(),
                    FestID = FestID,
                    Comment = comment,
                    UserID = user.ID,
                    InsertDate = DateTime.Now,
                    LikeCount = 0,
                };
                db.UserComments.Add(userComments);
                db.SaveChanges();

                return Json(new { message = "Başarılı..", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Hata - " + ex.Message, result = false });
            }
        }
        [HttpPost]
        public ActionResult LikeComment(Guid CommentID)
        {
            try
            {
                Users user = Session["Current_User"] as Users;
                if (user == null)
                    return Json(new { message = "Giriş yapmadan beğeni yapamazsınız...", result = true });

                db.Database.ExecuteSqlCommand("UPDATE UserComments SET LikeCount = LikeCount + 1 WHERE ID = @p0",CommentID);
                db.SaveChanges();

                return Json(new { message = "Beğenildi", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Hata - " + ex.Message, result = false });
            }
        }

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
                    var result = db.Users.Where(c => !c.IsDeleted && c.IsActive && (c.Email.Equals(email) && c.Password.Equals(password))).FirstOrDefault();
                    if (result != null)
                    {
                        result.Password = "";
                        Session["Current_User"] = result;
                        return Json(new { message = "Başarılı...", result = true });
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
            if (user == null)
                return RedirectToAction("LogIn", "Home");

            Session["Current_User"] = null;

            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}