using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.AddressHandler
{
    public class DeleteAddressHandleRequest : IRequest<DeleteAddressHandleResponse>
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        
    }
}
