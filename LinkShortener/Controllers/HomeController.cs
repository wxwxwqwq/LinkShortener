using LinkShortener.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Diagnostics;
using HashidsNet;
using LiteDB;

namespace LinkShortener.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {             
            if (Link.Authorized != null)
            {
                Downloader();
            }
            return View();
        }


        [HttpPost]
        public IActionResult ToShorten(Link link)
        {
            if (ModelState.IsValid)
            {

                foreach (Link l in Link.AllLinks)
                {
                    if (l.creator == Link.Authorized.login)
                    {
                        if (l.url == link.url)
                        {
                            ViewBag.Message = "Такая ссылка уже сокращена";
                            return View("Index");
                        }
                    }
                }

                if (Link.AllLinks.Count == 0)
                {
                    link.id = 1;
                }
                else
                {
                    link.id = Link.AllLinks[Link.AllLinks.Count - 1].id + 1;
                }                

                var hashids = new Hashids("salt");
                var token = hashids.Encode(link.id);               

                var shorturl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{token}";

                link.shorturl = shorturl;
                link.token = token;
                link.dateOfCreation = DateTime.Now;
                link.clicks = 0;
                link.creator = Link.Authorized.login;
                Link.AllLinks.Add(link);
                Link.UserLinks.Add(link);
                Uploader(link);                

                return Redirect("/");
            }
            return View("Index");
        }

     
        public IActionResult Redirector()
        {
            var path = HttpContext.Request.Path.ToUriComponent().Trim('/');

            foreach (Link l in Link.AllLinks)
            {
                if (l.token == path)
                {
                    l.clicks++;
                    Updater(l);

                    return Redirect(l.url);
                }
            }

            HttpContext.Response.StatusCode = 404;
            return Redirect("/");
        }


        public void Downloader()
        {
            using (var db = new LiteDatabase(@"DB/LinkShortenerDB.db"))
            {
                var col = db.GetCollection<Link>("links");
        
                var result = col.FindAll();

                Link.AllLinks.Clear();
                Link.UserLinks.Clear();

                foreach (Link item in result)
                {
                    if(item.creator == Link.Authorized.login)
                    {
                        Link.UserLinks.Add(item);
                    }

                    Link.AllLinks.Add(item);
                }
            }
        }
        

        public void Uploader(Link link)
        {
            using (var db = new LiteDatabase(@"DB/LinkShortenerDB.db"))
            {
                var col = db.GetCollection<Link>("links");
        
                col.Insert(link);
            }
        }
        

        public void Updater(Link link)
        {
            using (var db = new LiteDatabase(@"DB/LinkShortenerDB.db"))
            {
                var col = db.GetCollection<Link>("links");
        
                col.Update(link);
            }
        }       
    }
}