using BusinessLogicLayer.Handler.CurrentHandler;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.BankHandler
{
    public class UpdateBankHandleRequest : IRequest<UpdateBankHandleResponse>
    {

        public long Id { get; set; }// Güncellenecek banka ID'si
        public string Name { get; set; }
        public string Iban { get; set; }
        public int BranchCode { get; set; }
        public int AccountNo { get; set; }
        public Status Status { get; set; }// Aktif / Pasif
        public long CurrentId { get; set; }//bankaya ait mi?
    }



    }

