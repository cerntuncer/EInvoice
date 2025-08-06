using BusinessLogicLayer.Handler.CaseHandler;
using MediatR;

public class DeleteCaseHandleRequest : IRequest<DeleteCaseHandleResponse>
{
    public long Id { get; set; }
    public long CurrentId { get; set; } // Güvenlik için
}
