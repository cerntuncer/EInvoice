using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.BankHandler
{
    public class CreateBankHandleRequest : IRequest<CreateBankHandleResponse>
    {
        public string Name { get; set; }
        public string Iban { get; set; }
        public int BranchCode { get; set; }
        public int AccountNo { get; set; }
      
    }
}
