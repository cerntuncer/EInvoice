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
            var bank = _bankRepository.Find(request.Id);

            if (bank == null)
            {
                return new DeleteBankHandleResponse
                {
                    Error = true,
                    Message = "Banka bulunamadı."
                };
            }

            if (bank.CurrentId != request.CurrentId)
            {
                return new DeleteBankHandleResponse
                {
                    Error = true,
                    Message = "Bu banka belirtilen cari hesaba ait değil. Silinemez."
                };
            }

            bank.Status = Status.Passive;
            _bankRepository.Update(bank); // Soft delete işlemi

            return new DeleteBankHandleResponse
            {
                Error = false,
                Message = "Banka başarıyla silindi (pasif hale getirildi)."
            };
        }
    }
}
