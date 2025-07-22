using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Enumerations
{
    public enum CurrencyType
    {
        [Description("TL")]
        TurkLirasi = 1,

        [Description("EUR")]
        Euro = 2,

        [Description("USD")]
        Dolar = 3
    }
}
