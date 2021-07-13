using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SignalRinLINE.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalRinLINE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(IFormCollection post)
        {
            string account = post["account"];
            string password = post["password"];

            string userAccount = _configuration.GetValue<string>("USER:account");
            string userPassword = _configuration.GetValue<string>("USER:password");

            if (String.IsNullOrEmpty(account) || String.IsNullOrEmpty(password))
            {
                ViewBag.Msg = "請輸入帳號密碼";
                return View("Login");
            }
            else
            {
                if (userAccount.Equals(account) && userPassword.Equals(password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userAccount),
                        new Claim(ClaimTypes.Role, "XiaoSuLa"),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        RedirectUri = Url.Content("/Home/CallCenter"),
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    return View();
                }
                else
                {
                    ViewBag.Msg = "帳號或密碼錯誤";
                    return View("Login");
                }
            }

        }

        [Authorize]
        public IActionResult CallCenter()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
