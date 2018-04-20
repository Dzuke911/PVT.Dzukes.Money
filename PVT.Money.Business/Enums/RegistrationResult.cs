using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business.Enums
{
    ///////////////////////////////////////
    /// ANTIPATTERN: ANCHOR
    ///////////////////////////////////////
    public enum RegistrationResult
    {
        LoginAlreadyExists,
        EmailAlreadyExists,
        Fail,
        Success
    }
}
