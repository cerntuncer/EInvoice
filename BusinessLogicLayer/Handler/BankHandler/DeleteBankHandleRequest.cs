using MediatR;

namespace BusinessLogicLayer.Handler.BankHandler
{
    public class DeleteBankHandleRequest : IRequest<DeleteBankHandleResponse>
    {
        public long Id { get; set; }
        public long CurrentId { get; set; }
    }
}
