using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.BankHandler
{
    public class CreateBankHandle : IRequestHandler<CreateBankHandleRequest, CreateBankHandleResponse>
    {
        private readonly IBankRepository _bankRepository;
        private readonly ICurrentRepository _currentRepository;

        public CreateBankHandle(IBankRepository bankRepository, ICurrentRepository currentRepository)
        {
            _bankRepository = bankRepository;
            _currentRepository = currentRepository;
        }

        public async Task<CreateBankHandleResponse> Handle(CreateBankHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;

            if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 50)
                message = "Banka adı boş olamaz ve 50 karakteri geçemez.";

            else if (string.IsNullOrWhiteSpace(request.Iban) || request.Iban.Length > 26)
                message = "IBAN boş olamaz ve 26 karakteri geçemez.";

            else if (request.BranchCode <= 0)
                message = "Şube kodu geçersiz.";

            else if (request.AccountNo <= 0)
                message = "Hesap numarası geçersiz.";

            if (message != null)
            {
                return new CreateBankHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            // Cari hesap oluşturuluyor (otomatik)
            var current = new Current
            {
                Name = request.Name + " Cari Hesap", // istersen Name alanını da kullanabilirsin
                Status = Status.Active
            };
            _currentRepository.Add(current);

            // Banka oluşturuluyor
            var bank = new Bank
            {
                Name = request.Name,
                Iban = request.Iban,
                BranchCode = request.BranchCode,
                AccountNo = request.AccountNo,
                CurrentId = current.Id, // yeni oluşturulan current ile ilişki kuruldu
                Status = Status.Active
            };

            _bankRepository.Add(bank);

            return new CreateBankHandleResponse
            {
                Message = "Banka başarıyla oluşturuldu.",
                Error = false
            };
        }
    }

}
