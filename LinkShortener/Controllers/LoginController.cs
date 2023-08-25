using LinkShortener.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            if (Link.AllLinks.Count == 0)
            {
                Downloader();
            }
            return View();
        }


        public IActionResult ToLogIn(Userr user)
        {
            if (ModelState.IsValid)
            {
                Userr u = null;
                u = Userr.AllUsers.Find(i => i.login == user.login);

                if(u == null)
                {
                    ViewBag.Message3 = "Неверное имя пользователя или пароль";
                    return View("Index");
                }
                else
                {
                    if(u.password != user.password)
                    {
                        ViewBag.Message3 = "Неверное имя пользователя или пароль";
                        return View("Index");
                    }
                    else
                    {
                        Link.Authorized = user;
                        return Redirect("/");
                    }
                }
               
            }
            return View("Index");
        }


        public IActionResult ToLogOut()
        {
            Link.Authorized = null;
        
            return View("Index");
        }


        public void Downloader()
        {
            using (var db = new LiteDatabase(@"DB/LinkShortenerDB.db"))
            {
                var col = db.GetCollection<Userr>("users");

                var result = col.FindAll();

                foreach (Userr item in result)
                {
                    Userr.AllUsers.Add(item);
                }
            }
        }
    }
}