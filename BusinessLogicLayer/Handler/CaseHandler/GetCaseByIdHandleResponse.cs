using DatabaseAccessLayer.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.CaseHandler
{
    public class GetCaseByIdHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public long Id { get; set; }
        public string Address { get; set; }
        public long CurrentId { get; set; }
        public Status Status { get; set; }
    } 
}
