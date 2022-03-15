using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NutsStore.Models
{
    public enum PasswordPolicy
    {
        WithouLimitaion = 0,
        OnlyGreaterThan7Characters = 1,
        MustConainsCapitalCasesAndGreaterThan7Characters = 2,
        MustConainsCapitalCasesAndSymbolsAndGreaterThan7Characters = 3,
    }
}
