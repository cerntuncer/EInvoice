using BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs;
using MediatR;

public class DeleteCustomerSupplierHandleRequest : IRequest<DeleteCustomerSupplierHandleResponse>
{
    public long Id { get; set; }
    public long UserId { get; set; }
}
