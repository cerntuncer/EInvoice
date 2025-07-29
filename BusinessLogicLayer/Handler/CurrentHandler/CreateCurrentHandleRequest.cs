using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CurrentHandler
{
    public class CreateCurrentHandleRequest : IRequest<CreateCurrentHandleResponse>
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public CurrentType CurrentType { get; set; }
        public long UserId { get; set; }
    }
}
