using DatabaseAccessLayer.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.BankHandler
{
    public class GetBankByIdHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Iban { get; set; }
        public int BranchCode { get; set; }
        public int AccountNo { get; set; }
        public long CurrentId { get; set; }
        public Status Status { get; set; }
    }
}
