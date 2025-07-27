using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.PersonHandler
{
    public class CreatePersonHandleRequest : IRequest<CreatePersonHandleResponse>
    {
        public string Name { get; set; }
        public long IdentityNumber { get; set; }
        public string TaxOffice { get; set; }
        public PersonType Type { get; set; }
        public Status Status { get; set; }
    }
}
