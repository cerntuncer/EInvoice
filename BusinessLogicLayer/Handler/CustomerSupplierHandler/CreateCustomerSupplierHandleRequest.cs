using BusinessLogicLayer.Handler.CustomerSupplierHandler;
using BusinessLogicLayer.Handler.PersonHandler;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class CreateCustomerSupplierHandleRequest : IRequest<CreateCustomerSupplierHandleResponse>
{
    public CustomerOrSupplierType Type { get; set; }
    public Status Status { get; set; }     // ❗ eklendi (isteğe bağlı olarak sabit de atayabiliriz)

    public long? PersonId { get; set; }
    public CreatePersonHandleRequest? Person { get; set; }
}
