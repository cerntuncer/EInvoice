using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace BusinessLogicLayer.Handler.AddressHandler.DTOs
{
    public class DeleteAddressHandleResponse 
    {
        public string Message { get; set; }
        public bool Error { get; set; }
    }
}
