using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.BankHandler;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class CreateBankHandle : IRequestHandler<CreateBankHandleRequest, CreateBankHandleResponse>
{
    private readonly IBankRepository _bankRepository;
    private readonly ICurrentRepository _currentRepository;
    private readonly IUserRepository _userRepository;

    public CreateBankHandle(
        IBankRepository bankRepository,
        ICurrentRepository currentRepository,
        IUserRepository userRepository)
    {
        _bankRepository = bankRepository;
        _currentRepository = currentRepository;
        _userRepository = userRepository;
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

        else if (request.UserId <= 0)
            message = "Geçerli bir kullanıcı ID girilmelidir.";

        else if (_userRepository.Find(request.UserId) == null)
            message = "Kullanıcı bulunamadı.";

        if (message != null)
        {
            return new CreateBankHandleResponse
            {
                Message = message,
                Error = true
            };
        }

        // 🔄 Current otomatik oluşturuluyor
        var current = new Current
        {
            Name = request.Name + " Cari Hesap",
            UserId = request.UserId,
            Status = Status.Active
        };
        _currentRepository.Add(current);

        // 🧱 Banka oluşturuluyor
        var bank = new Bank
        {
            Name = request.Name,
            Iban = request.Iban,
            BranchCode = request.BranchCode,
            AccountNo = request.AccountNo,
            CurrentId = current.Id,
            Status = Status.Active
        };
        _bankRepository.Add(bank);

        return new CreateBankHandleResponse
        {
            Message = "Banka ve cari hesap başarıyla oluşturuldu.",
            Error = false
        };
    }
}
