using MediatR;


namespace BusinessLogicLayer.Handler.BankHandler.DTOs
{
    public class GetBankByIdHandleRequest : IRequest<GetBankByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
