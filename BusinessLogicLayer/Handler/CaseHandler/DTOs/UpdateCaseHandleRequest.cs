using BusinessLogicLayer.Handler.CaseHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class UpdateCaseHandleRequest : IRequest<UpdateCaseHandleResponse>
{
    public long Id { get; set; }
    public long CurrentId { get; set; } // Güvenlik kontrolü için
    public string Address { get; set; }
    public Status Status { get; set; }
}
