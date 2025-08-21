using BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class UpdateCustomerSupplierHandleRequest : IRequest<UpdateCustomerSupplierHandleResponse>
{
    public long Id { get; set; }
    public CustomerOrSupplierType Type { get; set; }
    public Status Status { get; set; }
    public long UserId { get; set; }
}
