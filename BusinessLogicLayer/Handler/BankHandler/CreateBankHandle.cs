using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.BankHandler;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class CreateBankHandle : IRequestHandler<CreateBankHandleRequest, CreateBankHandleResponse>
{
    private readonly IBankRepository _bankRepository;
    private readonly ICurrentRepository _currentRepository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMediator _mediator;
    public CreateBankHandle(
        IBankRepository bankRepository,
        ICurrentRepository currentRepository,
        ICaseRepository caseRepository,
        IMediator mediator)
    {
        _bankRepository = bankRepository;
        _currentRepository = currentRepository;
        _caseRepository = caseRepository;
        _mediator = mediator;
    }

    public async Task<CreateBankHandleResponse> Handle(CreateBankHandleRequest request, CancellationToken cancellationToken)
    {
        string message = null;
        long? currentId = null;
        bool current = true;
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 50)
            message = "Banka adı boş olamaz ve 50 karakteri geçemez.";

        else if (string.IsNullOrWhiteSpace(request.Iban) || request.Iban.Length > 26)
            message = "IBAN boş olamaz ve 26 karakteri geçemez.";

        else if (request.BranchCode <= 0)
            message = "Şube kodu geçersiz.";

        else if (request.AccountNo <= 0)
            message = "Hesap numarası geçersiz.";

        else if (request.CurrentId == null)
        {
            if (request.Current == null)
                message = "CurrentId ya da Yeni oluşturalacak Current Bilgileri iletilmelidir";
            else
            {
                var newCurrent = await _mediator.Send(request.Current, cancellationToken);//createcurrenthandlerequest
                if(newCurrent.Error == false)
                {
                    currentId = newCurrent.Id.Value;
                    current = false;
                }
                else
                {
                    message = newCurrent.Message;
                }
            }
        }
        else if(_currentRepository.Find(request.CurrentId.Value) == null)
        {
            message = "Gönderilen Id ye uygun Current bulunamadı";
        }
        var existingBank = _bankRepository.FirstOrDefault(b => b.CurrentId == request.CurrentId);
        var existinCase = _caseRepository.FirstOrDefault(b => b.CurrentId == request.CurrentId);
        if (existingBank != null || existinCase != null) 
        {
            message = "Belirtilen Cari Bir Hesaba Bağlıdır";
        }
        if (message != null)
        {
            return new CreateBankHandleResponse
            {
                Message = message,
                Error = true
            };
        }

       if(currentId == null)
       {
            currentId = request.CurrentId.Value;
       }

        var bank = new Bank
        {
            Name = request.Name,
            Iban = request.Iban,
            BranchCode = request.BranchCode,
            AccountNo = request.AccountNo,
            CurrentId = currentId.Value,
            Status = Status.Active
        };
        _bankRepository.Add(bank);
        if (current)
        {
            message = "Banka başarıyla oluşturuldu.";
        }
        else
        {
            message = "Banka ve cari hesap başarıyla oluşturuldu.";
        }
        return new CreateBankHandleResponse
        {
            Message = message,
            Error = false
        };
    }
}
