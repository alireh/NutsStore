using Microsoft.AspNetCore.Http;
using NutsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NutsStore.Util
{
    public static class SessionManager
    {
        public static UserData GetSession(IHttpContextAccessor httpContextAccessor, string key)
        {
            if (httpContextAccessor.HttpContext.Session.GetString(key) == null) return null;
            var user = httpContextAccessor.HttpContext.Session.Get(key);
            var ud = Util.Utility.FromByteArray<UserData>(user);
            return ud;
        }

        public static void SetSession(IHttpContextAccessor httpContextAccessor, string key, string value)
        {
            httpContextAccessor.HttpContext.Session.SetString(key, value);
        }

        public static string getToken(IHttpContextAccessor httpContextAccessor)
        {
            var userData = GetSession(httpContextAccessor, "UserData");
            if (userData != null)
            {
                return userData.Token.Replace("\"", "");
            }
            return null;
        }
    }
}
