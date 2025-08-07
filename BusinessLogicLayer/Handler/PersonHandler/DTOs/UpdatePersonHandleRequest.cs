using BusinessLogicLayer.Handler.PersonHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class UpdatePersonHandleRequest : IRequest<UpdatePersonHandleResponse>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long IdentityNumber { get; set; }
    public string TaxOffice { get; set; }
    public PersonType Type { get; set; }
    public Status Status { get; set; }
}

