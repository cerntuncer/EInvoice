using MediatR;

namespace BusinessLogicLayer.Handler.BankHandler.DTOs
{
    public class DeleteBankHandleRequest : IRequest<DeleteBankHandleResponse>
    {
        public long Id { get; set; }
        public long CurrentId { get; set; }
    }
}
