using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Enumerations
{
    public enum InvoiceSenario
    {
        EInvoice = 1,   // e-Fatura
        EArchive = 2,   // e-Arşiv
        Paper = 3       // Kağıt Fatura 
    }
}
