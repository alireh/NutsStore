using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NutsStore.Models;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace NutsStore.Util
{
    public static class Utility
    {
        public static bool ValidateJSON(this string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch (JsonReaderException ex)
            {
                return false;
            }
        }
        public static byte[] ObjectToByteArray(Object obj)
        {
            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            var bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
        public static async Task<T> GetBodyObject<T>(IHttpContextAccessor httpContextAccessor)
        {
            var request = httpContextAccessor.HttpContext.Request;
            T obj;
            using (var reader = new StreamReader(request.Body))
            {
                var content = await reader.ReadToEndAsync();
                obj = JsonConvert.DeserializeObject<T>(content.ToString());
            }
            return obj;
        }

        public static ResponseModel CheckAuthorize(IHttpContextAccessor httpContextAccessor, AuthorizeTypeInfo authorizeTypeInfo, ref UserData userData)
        {
            userData = SessionManager.GetSession(httpContextAccessor, "UserData");
            if (userData == null)
            {
                return new ResponseModel
                {
                    message = "Unauthorized",
                    status = 401
                };
            }
            var accessToken = userData.Token;
            if (string.IsNullOrWhiteSpace(accessToken) || !accessToken.Contains("__token__"))
            {
                return new ResponseModel
                {
                    message = "token is invalid",
                    status = 400
                };
            }
            if (authorizeTypeInfo.IsCheckAdmin)
            {
                var user = SqliteManager.Instance.GetUser(userData.Username);
                var isAdmin = user.IsAdmin;
                if (!isAdmin)
                {
                    return new ResponseModel
                    {
                        message = "Unauthorized",
                        status = 401
                    };
                }
            }
            else if (authorizeTypeInfo.IsMustBeOwnUser)
            {
                var usernameHash = accessToken.Split("__token__")[0];
                var isOwnuser = SecurePasswordHasher.Verify(authorizeTypeInfo.Username, usernameHash);
                if (!isOwnuser)
                {
                    return new ResponseModel
                    {
                        message = "token is invalid",
                        status = 400
                    };
                }
            }
            return null;
        }
        public static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPassword(string password, PasswordPolicy passwordPolicy, ref string passwordPolicyMessage)
        {
            string atLeastOneUppercaseRegex = "(?=.*[A-Z])";
            string atLeastOneSymbolRegex = "^(?=.{1})(?=.*\\W)";
            var isFilled = !string.IsNullOrWhiteSpace(password);
            var isGreaterThan7Chars = password.Length > 7;
            var hasAtLeastOneCapitalCase = IsMatchRegex(atLeastOneUppercaseRegex, password);
            var hasAtLeastOneSymbol = IsMatchRegex(atLeastOneSymbolRegex, password);

            var result = false;
            switch (passwordPolicy)
            {
                case PasswordPolicy.WithouLimitaion:
                    result = isFilled;
                    break;
                case PasswordPolicy.OnlyGreaterThan7Characters:
                    result = isFilled && isGreaterThan7Chars;
                    break;
                case PasswordPolicy.MustConainsCapitalCasesAndGreaterThan7Characters:
                    result = isFilled && isGreaterThan7Chars && hasAtLeastOneCapitalCase;
                    break;
                case PasswordPolicy.MustConainsCapitalCasesAndSymbolsAndGreaterThan7Characters:
                    result = isFilled && isGreaterThan7Chars && hasAtLeastOneCapitalCase && hasAtLeastOneSymbol;
                    break;
            }
            if (!isFilled)
            {
                passwordPolicyMessage = "password is empty";
            }
            else if (!isGreaterThan7Chars)
            {
                passwordPolicyMessage = "password is short";
            }
            else if (!hasAtLeastOneCapitalCase)
            {
                passwordPolicyMessage = "password muse have at least one capital case";
            }
            else if (!hasAtLeastOneSymbol)
            {
                passwordPolicyMessage = "password muse have at least one symbol";
            }
            return result;
        }
        public static bool IsMatchRegex(string pattern, string word)
        {
            var regex = new Regex(pattern);
            return regex.IsMatch(word);
        }

        public static string TryGetStringConfig(IConfiguration Configuration, string key, string defaultValue)
        {
            try
            {
                return Configuration.GetSection("AppSettings").GetSection(key).Value;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static bool SetStringConfig(IConfiguration configuration, string key, string val)
        {
            try
            {
                configuration[key] = val;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static int TryGetIntConfig(IConfiguration Configuration, string key, int defaultValue)
        {
            int result;
            if (!int.TryParse(Configuration.GetSection("AppSettings").GetSection(key).Value, out result))
            {
                result = defaultValue;
            }
            return result;
        }
    }
}
