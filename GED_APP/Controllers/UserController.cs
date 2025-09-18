using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GED_APP.Controllers
{
   
    public class UserController : Controller
    {
        private readonly IUser _userRepo;

        public UserController(IUser userRepo)
        {
            _userRepo = userRepo;
        }
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            ICollection<User> users = null;
            try
            {
                users=_userRepo.GetAll();
                ViewBag.DataSource = users;
                return View(users);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "Error chargement users.....";
            }
            return View(users);
        }
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult AddOrEdit(int id = 0)
        {
            User u = new User();
            OnloadServices();
            if (id == 0)
            {
                return View(new User());
            }
            else
            {
                u=_userRepo.GetById(id);
               
            }
            return View(u);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit([Bind("Id, UserName, UserEmail, Service, Role, Password, saltPassword, Token,  KeepLoginIn")] User usr)
        {
            int res;
            if (ModelState.IsValid)
            {
                OnloadServices();
                if (usr.Id == 0)
                {
                    res=_userRepo.Add(usr);
                    if (res > 0)
                    {
                        TempData["AlertMessage"] = "User added successfully.....";
                        return RedirectToAction("Index");
                    }
                       
                }
                else
                {
                    res= _userRepo.Update(usr);
                    if (res > 0)
                    {
                        TempData["AlertMessage"] = "User updated successfully.....";
                        return RedirectToAction("Index");
                    }

                }

            }
            return RedirectToAction("Index");

        }

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            int res;

            if (id > 0)
            {
                res= _userRepo.Delete(id);
                if (res > 0)
                {
                    TempData["AlertMessage"] = "User deleted successfully.....";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["AlertMessage"] = "Error deleted User.....";
                    return RedirectToAction("Index");
                }
               
            }
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");
            return View();
        }
        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> Login(string UserEmail, string Password)
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            var u = new User();

            if (UserEmail != null)
            {
                u=_userRepo.GetByEmail(UserEmail);
                if (u!=null)
                {
                    bool verifpass = verifHashPassword(Password, u.Password, u.SaltPassword);
                    if (verifpass)
                    {
                        SignInUser(u);
                        TempData["UserId"] = u.Id;
                        TempData["User"] = u.UserName;
                        //ViewBag.user = HttpContext.User.Claims.;
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        ViewBag.user = "";
                        TempData["User"] = "";
                        TempData["AlertMessage"] = "Login or password incorrect.....";
                        return View();
                    }
                }
            }
            return View();
        }
        [Authorize]
        public IActionResult AccesDenied()
        {
            return View();
        }
        private bool verifHashPassword(string enterPass, string storePass, string storeSalt)
        {
            var salBytes = Convert.FromBase64String(storeSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enterPass, salBytes, 1000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storePass;

        }
        private bool ValidateLogin(string UserEmail, string password)
        {
            if (UserEmail == "dahirou@gmail.com" && password == "1234")
                return true;
            else
                return false;
        }
        private async Task SignInUser(User u)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Email, u.UserEmail),
                new Claim(ClaimTypes.Role, u.Role)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authentificationProperties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authentificationProperties);
        }
        [NonAction]
        public void OnloadServices()
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<string> services = new List<string>();
            if (ModelState.IsValid)
            {
                services.Insert(0, "Choisir le service");
                services.Insert(1, "CABNIET");
                services.Insert(2, "COURRIER");
                ViewBag.Services = services;
            }
        }
    
     [NonAction]
        public void OnloadTypes()
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<string> types = new List<string>();
            if (ModelState.IsValid)
            {
                types.Insert(0, "Choisir le type");
                types.Insert(1, "OPERATEUR");
                types.Insert(2, "CHEF SERVICE COURRIER");
                types.Insert(2, "ADMIN");
                ViewBag.Types = types;
            }
        }
    }
}
