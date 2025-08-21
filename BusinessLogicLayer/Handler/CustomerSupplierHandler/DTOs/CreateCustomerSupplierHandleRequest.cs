using BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs;
using BusinessLogicLayer.Handler.PersonHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class CreateCustomerSupplierHandleRequest : IRequest<CreateCustomerSupplierHandleResponse>
{
    public CustomerOrSupplierType Type { get; set; }
    public Status Status { get; set; }     // ❗ eklendi (isteğe bağlı olarak sabit de atayabiliriz)

    public long? PersonId { get; set; }
    public CreatePersonHandleRequest? Person { get; set; }
    public long UserId { get; set; }
}
