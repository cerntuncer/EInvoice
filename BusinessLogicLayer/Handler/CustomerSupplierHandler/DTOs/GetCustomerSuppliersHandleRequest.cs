using MediatR;

namespace BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs
{
    public class GetCustomerSuppliersHandleRequest : IRequest<GetCustomerSuppliersHandleResponse>
    {
        public long UserId { get; set; }
    }
}