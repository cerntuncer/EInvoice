using DatabaseAccessLayer.Enumerations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.AddressHandler
{
    public class CreateAddressHandleRequest :IRequest<CreateAddressHandleResponse>
    {
        public AddressType AddressType { get; set; }   // Ev, iş, fatura vs.
        public string Text { get; set; }             
        public long PersonId { get; set; } 
      

    }
}
