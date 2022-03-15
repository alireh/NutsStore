using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NutsStore.Models
{
    [Serializable]
    public class UserData
    {
        public string Token;
        public string FirstName;
        public string Username;
        public string LastName;
        public int[] profile_pic;
        public string profile_picBase64;
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
