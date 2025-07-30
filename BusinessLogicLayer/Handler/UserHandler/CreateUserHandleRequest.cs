using BusinessLogicLayer.Handler.UserHandler;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class CreateUserHandleRequest : IRequest<CreateUserHandleResponse>
{
    public UserType Type { get; set; }
    public Status Status { get; set; }

    // Person bilgileri
    public string Name { get; set; }
    public long IdentityNumber { get; set; }
    public string TaxOffice { get; set; }

}
