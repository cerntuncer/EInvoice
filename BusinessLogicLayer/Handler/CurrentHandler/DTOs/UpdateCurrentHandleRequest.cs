using DatabaseAccessLayer.Enumerations;
using MediatR;

public class UpdateCurrentHandleRequest : IRequest<UpdateCurrentHandleResponse>
{
    public long Id { get; set; } // Güncellenecek Current kaydının ID’si
    public long UserId { get; set; } // Güvenlik kontrolü
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public CurrencyType CurrencyType { get; set; }
    public CurrentType CurrentType { get; set; }
    public Status Status { get; set; }
}
