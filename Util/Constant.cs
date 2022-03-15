using System;
using System.Collections.Generic;
using System.Linq;
using NutsStore.Models;
using System.Threading.Tasks;

namespace NutsStore.Util
{
    public class Constant
    {
        public static string AdminInitialUsername = "storeadmin";
        public static string AdminInitialPassword = "admin8415913";
        public static PasswordPolicy PasswordPolicy = PasswordPolicy.OnlyGreaterThan7Characters;
    }
}
