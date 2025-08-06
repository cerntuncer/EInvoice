using MediatR;

namespace BusinessLogicLayer.Handler.CustomerSupplierHandler
{
    public class GetCustomerSupplierByIdHandleRequest : IRequest<GetCustomerSupplierByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
