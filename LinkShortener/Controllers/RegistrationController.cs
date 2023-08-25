using LinkShortener.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Diagnostics;
using HashidsNet;
using LiteDB;

namespace LinkShortener.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            if (Link.AllLinks.Count == 0)
            {
                Downloader();
            }
            return View();
        }


        [HttpPost]
        public IActionResult Register(Userr user)
        {
            if (ModelState.IsValid)
            {
                foreach (Userr u in Userr.AllUsers)
                {

                    if (u.login == user.login)
                    {
                        ViewBag.Message2 = "Пользователь с таким логином уже существует";
                        return View("Index");
                    }

                }

                Userr.AllUsers.Add(user);

                using (var db = new LiteDatabase(@"DB/LinkShortenerDB.db"))
                {
                    var col = db.GetCollection<Userr>("users");

                    col.Insert(user);
                }
              
                return Redirect("/Registration");
            }
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
