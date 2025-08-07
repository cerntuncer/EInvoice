using MediatR;

namespace BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs
{
    public class GetCustomerSupplierByIdHandleRequest : IRequest<GetCustomerSupplierByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
