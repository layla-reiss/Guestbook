using guestbook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis;

namespace guestbook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public ActionResult Index(int pagNumber)
        {            
            List<guest> guests = new List<guest>();
            guestDAO guestDAO = new guestDAO();
            List<string> languages = new List<string>();
            languages = guestDAO.FetchLanguages();

            ViewBag.languages = languages;
            ViewBag.pageNumber = pagNumber;

            guests = guestDAO.FetchAll();
            int pageNumber = guests.Count % 6;
            ViewBag.pageCount = pageNumber;
            int currentIndex = 0;
            if (pagNumber > 0)
            {

                for (int i = 0; i < pageNumber; i++)
                {
                    List<guest> currentGuests = new List<guest>();
                    if (currentIndex + 6 <= guests.Count)
                    {
                        for (int j = currentIndex; j < currentIndex + 6; j++)
                        {
                            currentGuests.Add(guests[j]);
                        }
                        if (currentIndex + 1 == pagNumber)
                        {
                            ViewBag.guests = currentGuests;
                            return View();
                        }

                        currentIndex += 6;
                    }
                    else if (currentIndex + 6 > guests.Count)
                    {
                        for (int j = currentIndex; j < guests.Count; j++)
                        {
                            currentGuests.Add(guests[j]);
                        }
                        ViewBag.guests = currentGuests;
                        return View();
                    }
                }
            }
            else
            {
                currentIndex  = 0;
                List<guest> startGuests = new List<guest>();
                for (int j = 0; j < currentIndex + 6; j++)
                {
                    startGuests.Add(guests[j]);
                }
                ViewBag.guests = startGuests;
                ViewBag.pageNumber = 1;
                return View();
            }
            return View();
        }
        public ActionResult ChangeLanguage(string language)
        {
            List<guest> guests = new List<guest>();
            guestDAO guestDAO = new guestDAO();
            guests = guestDAO.FilterByLanguage(language);
            List<string> languages = new List<string>();
            languages = guestDAO.FetchLanguages();

            ViewBag.languages = languages;

            ViewBag.guests = guests;
            return View("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public ActionResult CreateGuest()
        {
            ViewBag.Message = "";
            return View();
        }
        public ActionResult AddNewGuest(guest thisGuest)
        {
            guestDAO guestDAO = new guestDAO();
            guestDAO.AddNew(thisGuest);
            return SendEmail(thisGuest.email, "Welcome to GuestBook!", thisGuest.comment);
        }
        public ActionResult ValidateGuest(guest thisGuest) 
        {
            guestDAO guestDAO = new guestDAO();
            bool isTitleValid = guestDAO.isEmailValid(thisGuest.email);

            if (isTitleValid)
            {
                return AddNewGuest(thisGuest);
            }
            else
            {
                ViewBag.Message = "denied";
                return View("CreateGuest");
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult SendEmail()
        {
            return View();
        }

        public ActionResult useradd()
        {
            return View();
        }
        public ActionResult userdelete(string email)
        {
            guestDAO guestDAO = new guestDAO();
            guestDAO.DeleteGuest(email);
            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public ActionResult SendEmail(string receiver, string subject, string message)
        {
            //s1xteenl3tt3rpa55w0rd
            Execute(receiver, subject, message);
            ViewBag.Message = "approved";
            return View("CreateGuest");
        }
        public ActionResult previewResult(guest thisGuest)
        {
            ViewBag.preview = thisGuest;
            ViewBag.message = "";
            return View("CreateGuest");
        }
        static async Task Execute(string receiver, string subject, string message)
        {
            var apiKey = Environment.GetEnvironmentVariable("apiKey");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("Layla.Reiss@coxautoinc.com", "Example User"));
            msg.AddTo(new EmailAddress(receiver, "Example User"));
            msg.SetTemplateId("d-386b027e05e7403ea9c6a4f9f4a5b234");
            var dynamicTemplateData = new
            {
                comment = message
            };
            msg.SetTemplateData(dynamicTemplateData);
            await client.SendEmailAsync(msg);
        }
        public ActionResult ShowMore(string name, string email, string date, string comment, int pagNumber)
        {
            ViewBag.name = name;
            ViewBag.email = email;
            ViewBag.date = date;
            ViewBag.comment = comment;
            ViewBag.pageNumber = pagNumber;
            return View("ShowMore");
        }
    }
}