using BusinessLogicLayer.Handler.BankHandler;
using MediatR;

public class CreateBankHandleRequest : IRequest<CreateBankHandleResponse>
{
    public string Name { get; set; }
    public string Iban { get; set; }
    public int BranchCode { get; set; }
    public int AccountNo { get; set; }

    public long UserId { get; set; } // ❗ eklendi
}
