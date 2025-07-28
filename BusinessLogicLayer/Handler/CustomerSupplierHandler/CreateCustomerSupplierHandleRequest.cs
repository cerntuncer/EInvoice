using DatabaseAccessLayer.Enumerations;
using MediatR;


namespace BusinessLogicLayer.Handler.CustomerSupplierHandler
{
    public class CreateCustomerSupplierHandleRequest : IRequest<CreateCustomerSupplierHandleResponse>
    {
        public CustomerOrSupplierType Type { get; set; }
        public long PersonId { get; set; }
        public Status Status { get; set; }
    }
}
