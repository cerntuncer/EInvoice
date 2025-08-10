using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.BankHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.BankHandler.Commands
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
            string? message = null;

            // --- Varlık kontrolü ---
            var bank = _bankRepository.Find(request.Id);
            if (bank == null)
            {
                message = "Banka bulunamadı.";
            }
            else
            {
                // --- Sahiplik kontrolü ---
                if (bank.CurrentId != request.CurrentId)
                    message = "Bu banka belirtilen Current kaydına ait değil.";

                // --- Alan validasyonları ---
                // Name
                if (message == null)
                {
                    var name = request.Name?.Trim();
                    if (string.IsNullOrWhiteSpace(name) || name!.Length > 50)
                        message = "Banka adı boş olamaz ve 50 karakteri geçemez.";
                }

                // IBAN (boşlukları temizleyip uzunluk kontrolü)
                if (message == null)
                {
                    var iban = request.Iban?.Trim().Replace(" ", "");
                    if (string.IsNullOrWhiteSpace(iban) || iban!.Length > 26)
                        message = "IBAN boş olamaz ve 26 karakteri geçemez.";
                }

                // Sayısal alanlar
                if (message == null && (request.BranchCode <= 0 || request.AccountNo <= 0))
                    message = "Şube kodu ve hesap numarası sıfırdan büyük olmalıdır.";

                // Status enum kontrolü (varsa)
                if (message == null && !Enum.IsDefined(typeof(Status), request.Status))
                    message = "Geçersiz durum bilgisi.";
            }

            if (message != null)
            {
                return new UpdateBankHandleResponse
                {
                    Error = true,
                    Message = message
                };
            }

            // --- Güncelleme ---
            bank!.Name = request.Name!.Trim();
            bank.Iban = request.Iban!.Trim().Replace(" ", "");
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
