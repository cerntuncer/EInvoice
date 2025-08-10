using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.BankHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.BankHandler.Commands
{
    public class DeleteBankHandle : IRequestHandler<DeleteBankHandleRequest, DeleteBankHandleResponse>
    {
        private readonly IBankRepository _bankRepository;

        public DeleteBankHandle(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public async Task<DeleteBankHandleResponse> Handle(DeleteBankHandleRequest request, CancellationToken cancellationToken)
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
                    message = "Bu banka belirtilen cari hesaba ait değil. Silinemez.";

                // (opsiyonel) Zaten pasif mi?
                if (message == null && bank.Status == Status.Passive)
                    message = "Banka zaten pasif durumda.";
            }

            if (message != null)
            {
                return new DeleteBankHandleResponse
                {
                    Error = true,
                    Message = message
                };
            }

            // --- Soft delete ---
            bank!.Status = Status.Passive;
            _bankRepository.Update(bank);

            return new DeleteBankHandleResponse
            {
                Error = false,
                Message = "Banka başarıyla silindi (pasif hale getirildi)."
            };
        }
    }
}
