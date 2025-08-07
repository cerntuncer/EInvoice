using BusinessLogicLayer.Handler.PersonHandler.DTOs;
using BusinessLogicLayer.Handler.UserHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class CreateUserHandleRequest : IRequest<CreateUserHandleResponse>
{
    public UserType Type { get; set; }
    public Status Status { get; set; }
    public long? PersonId { get; set; }
    public CreatePersonHandleRequest? Person {  get; set; }
}
