using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NutsStore.Models;
using NutsStore.Util;

namespace NutsStore.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Edit()
        {
            try
            {
                var userData = new UserData();
                var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.EmptyInstance(), ref userData);
                if (null != authorizeResponse)
                {
                    return View("../Home/Error", authorizeResponse);
                }

                var user = SqliteManager.Instance.GetUser(userData.Username);
                return View("EditUser", user);
            }
            catch
            {
                return View("../Home/Error", new ResponseModel { status = 500 });
            }
        }
        public IActionResult Users()
        {
            try
            {
                var userData = new UserData();
                var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                if (null != authorizeResponse)
                {
                    return View("../Home/Error", authorizeResponse);
                }

                var users = SqliteManager.Instance.GetUsers();
                return View("Users", users);
            }
            catch
            {
                return View("../Home/Error", new ResponseModel { status = 500 });
            }
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
    }
}
