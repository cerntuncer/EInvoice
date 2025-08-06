using MediatR;


namespace BusinessLogicLayer.Handler.BankHandler
{
    public class GetBankByIdHandleRequest : IRequest<GetBankByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
