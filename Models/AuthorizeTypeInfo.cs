using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NutsStore.Models
{
    public class AuthorizeTypeInfo
    {
        public string Username;
        public bool IsCheckAdmin;
        public bool IsMustBeOwnUser;

        public static AuthorizeTypeInfo MustBeAdminInstance()
        {
            return new AuthorizeTypeInfo()
            {
                Username = "",
                IsMustBeOwnUser = false,
                IsCheckAdmin = true,
            };
        }

        public static AuthorizeTypeInfo MustBeOwnUserInstance(string username)
        {
            return new AuthorizeTypeInfo
            {
                Username = username,
                IsMustBeOwnUser = true,
                IsCheckAdmin = false,
            };
        }

        public static AuthorizeTypeInfo EmptyInstance()
        {
            return new AuthorizeTypeInfo
            {
                Username = "",
                IsMustBeOwnUser = false,
                IsCheckAdmin = false,
            };
        }
    }
}
