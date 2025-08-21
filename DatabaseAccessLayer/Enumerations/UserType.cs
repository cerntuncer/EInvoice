using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Enumerations
{
    public enum UserType
    {
        [Description("Gerçek Kişi")]
        NaturalPerson = 1,

        [Description("Tüzel Kişi")]
        LegalEntity = 2

    }
}
