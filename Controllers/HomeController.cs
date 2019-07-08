using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RandomPasscode.Models;

namespace RandomPasscode.Controllers
{
    public static class SessionExtensions
{
    // We can call ".SetObjectAsJson" just like our other session set methods, by passing a key and a value
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        // This helper function simply serializes theobject to JSON and stores it as a string in session
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    // generic type T is a stand-in indicating that we need to specify the type on retrieval
    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
        string value = session.GetString(key);
        // Upon retrieval the object is deserialized based on the type we specified
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
}

    public class HomeController : Controller
    {
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetInt32("Count", 0);
            int Count = (int)HttpContext.Session.GetInt32("Count");
            ViewBag.Count = Count;
            return View("Index");
        }

        [HttpGet("session")]
        public IActionResult Session()
        {
            int? Count = HttpContext.Session.GetInt32("Count");
            Count++;
            HttpContext.Session.SetInt32("Count", Convert.ToInt32(Count));
            string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            char[] Passcode = new char[14];
            Random rand = new Random();
            for(var i = 0; i < 14; i++)
            {
                Passcode[i] = Characters[rand.Next(0, Characters.Length)];
            }
            string passcode = new string(Passcode);
            Console.WriteLine(passcode);
            ViewBag.Passcode = passcode;
            ViewBag.Count = Count;
            return View("Index");
        }
    }
}
