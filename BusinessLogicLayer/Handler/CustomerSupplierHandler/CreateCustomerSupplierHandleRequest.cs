using BusinessLogicLayer.Handler.UserHandler;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.CustomerSupplierHandler
{
    public class CreateCustomerSupplierHandleRequest : IRequest<CreateCustomerSupplierHandleResponse>
    {
        public CustomerOrSupplierType Type { get; set; }
        public long PersonId { get; set; }
        public Status Status { get; set; }
    }
}
