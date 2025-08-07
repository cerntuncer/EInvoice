using BusinessLogicLayer.Handler.BankHandler.DTOs;
using BusinessLogicLayer.Handler.CurrentHandler.DTOs;
using DatabaseAccessLayer.Entities;
using MediatR;

public class CreateBankHandleRequest : IRequest<CreateBankHandleResponse>
{
    public string Name { get; set; }
    public string Iban { get; set; }
    public int BranchCode { get; set; }
    public int AccountNo { get; set; }
    public long? CurrentId { get; set; }
    public CreateCurrentHandleRequest? Current { get; set; }
}
