using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using MediatR;

namespace BusinessLogicLayer.Handler.BankHandler
{
    public class UpdateBankHandle : IRequestHandler<UpdateBankHandleRequest, UpdateBankHandleResponse>
    {
        private readonly IBankRepository _bankRepository;

        public UpdateBankHandle(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public async Task<UpdateBankHandleResponse> Handle(UpdateBankHandleRequest request, CancellationToken cancellationToken)
        {
            var bank = _bankRepository.Find(request.Id);

            if (bank == null)
            {
                return new UpdateBankHandleResponse
                {
                    Error = true,
                    Message = "Banka bulunamadı."
                };
            }

            //Bu banka gerçekten CurrentId'ye bağlı mı?
            if (bank.CurrentId != request.CurrentId)
            {
                return new UpdateBankHandleResponse
                {
                    Error = true,
                    Message = "Bu banka belirtilen Current kaydına ait değil."
                };
            }

            
            if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 50)
            {
                return new UpdateBankHandleResponse
                {
                    Error = true,
                    Message = "Banka adı boş olamaz ve 50 karakteri geçemez."
                };
            }

            if (string.IsNullOrWhiteSpace(request.Iban) || request.Iban.Length > 26)
            {
                return new UpdateBankHandleResponse
                {
                    Error = true,
                    Message = "IBAN boş olamaz ve 26 karakteri geçemez."
                };
            }

            if (request.BranchCode <= 0 || request.AccountNo <= 0)
            {
                return new UpdateBankHandleResponse
                {
                    Error = true,
                    Message = "Şube kodu ve hesap numarası sıfırdan büyük olmalıdır."
                };
            }

            bank.Name = request.Name;
            bank.Iban = request.Iban;
            bank.BranchCode = request.BranchCode;
            bank.AccountNo = request.AccountNo;
            bank.Status = request.Status;

            _bankRepository.Update(bank);

            return new UpdateBankHandleResponse
            {
                Error = false,
                Message = "Banka başarıyla güncellendi."
            };
        }
    }
}
