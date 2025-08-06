using BusinessLogicLayer.Handler.CaseHandler;
using BusinessLogicLayer.Handler.CurrentHandler;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class CreateCaseHandleRequest : IRequest<CreateCaseHandleResponse>
{
    public string Name { get; set; } // Cari adı (Current)
    public string Address { get; set; } // Kasa adresi
    public Status Status { get; set; }// Durum bilgisi
    public long? CurrentId { get; set; } //eklendi
    public CreateCurrentHandleRequest? Current { get; set; }
}
