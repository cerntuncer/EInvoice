using BusinessLogicLayer.Handler.CaseHandler.DTOs;
using MediatR;

public class DeleteCaseHandleRequest : IRequest<DeleteCaseHandleResponse>
{
    public long Id { get; set; }
    public long CurrentId { get; set; } // Güvenlik için
}
