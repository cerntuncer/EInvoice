using BusinessLogicLayer.Handler.CustomerSupplierHandler;
using MediatR;

public class DeleteCustomerSupplierHandleRequest : IRequest<DeleteCustomerSupplierHandleResponse>
{
    public long Id { get; set; }
}
