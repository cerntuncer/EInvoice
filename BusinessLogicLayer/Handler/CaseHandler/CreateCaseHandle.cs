using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.BankHandler;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CaseHandler
{
    public class CreateCaseHandle : IRequestHandler<CreateCaseHandleRequest, CreateCaseHandleResponse>
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICurrentRepository _currentRepository;
        private readonly IBankRepository _bankRepository;
        private readonly IMediator _mediator;

        public CreateCaseHandle(
            ICaseRepository caseRepository,
            ICurrentRepository currentRepository,
            IBankRepository bankRepository,
            IMediator mediator)
        {
            _caseRepository = caseRepository;
            _currentRepository = currentRepository;
            _bankRepository = bankRepository;
            _mediator = mediator;
        }

        public async Task<CreateCaseHandleResponse> Handle(CreateCaseHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;
            long? currentId = null;
            bool current = true;
            if (string.IsNullOrWhiteSpace(request.Name))
                message = "Kasa adı boş olamaz.";
            else if (string.IsNullOrWhiteSpace(request.Address) || request.Address.Length > 100)
                message = "Adres 100 karakteri geçemez.";
            else if (request.CurrentId == null)
            {
                if (request.Current == null)
                    message = "CurrentId ya da Yeni oluşturalacak Current Bilgileri iletilmelidir";
                else
                {
                    var newCurrent = await _mediator.Send(request.Current, cancellationToken);
                    if (newCurrent.Error == false)
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
            else if (_currentRepository.Find(request.CurrentId.Value) == null)
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
                return new CreateCaseHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }
            if (currentId == null)
            {
                currentId = request.CurrentId.Value;
            }

            // 🧱 Case oluşturuluyor
            var kasa = new Case
            {
                Address = request.Address,
                CurrentId = currentId.Value,
                Status = request.Status
            };
            _caseRepository.Add(kasa);
            if (current)
            {
                message = "Kasa başarıyla oluşturuldu.";
            }
            else
            {
                message = "Kasa ve cari hesap başarıyla oluşturuldu.";
            }
            return new CreateCaseHandleResponse
            {
                Message = message,
                Error = false
            };
        }
    }
}
