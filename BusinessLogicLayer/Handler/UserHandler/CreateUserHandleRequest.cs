using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.UserHandler
{
    public class CreateUserHandleRequest : IRequest<CreateUserHandleResponse>
    {
        public UserType Type { get; set; }
        public Status Status { get; set; }
        public long PersonId { get; set; }
    }
}
