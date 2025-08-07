using BusinessLogicLayer.Handler.PersonHandler.DTOs;
using MediatR;

public class DeletePersonHandleRequest : IRequest<DeletePersonHandleResponse>
{
    public long Id { get; set; }
    public long RequestedByUserId { get; set; } // Gerekirse doğrulama için
}


