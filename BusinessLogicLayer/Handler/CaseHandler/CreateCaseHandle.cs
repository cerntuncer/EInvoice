using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CaseHandler
{
    public class CreateCaseHandle : IRequestHandler<CreateCaseHandleRequest, CreateCaseHandleResponse>
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICurrentRepository _currentRepository;

        public CreateCaseHandle(ICaseRepository caseRepository, ICurrentRepository currentRepository)
        {
            _caseRepository = caseRepository;
            _currentRepository = currentRepository;
        }

        public Task<CreateCaseHandleResponse> Handle(CreateCaseHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;

            if (string.IsNullOrWhiteSpace(request.Name))
                message = "Kasa adı boş olamaz.";

            else if (string.IsNullOrWhiteSpace(request.Address) || request.Address.Length > 100)
                message = "Adres 100 karakteri geçemez.";

            if (message != null)
            {
                return Task.FromResult(new CreateCaseHandleResponse
                {
                    Message = message,
                    Error = true
                });
            }

            // Current oluştur
            var current = new Current
            {
                Name = request.Name,
                Status = Status.Active
            };
            _currentRepository.Add(current);

            // Case oluştur
            var kasa = new Case
            {
                Address = request.Address,
                CurrentId = current.Id,
                Status = request.Status
            };
            _caseRepository.Add(kasa);

            return Task.FromResult(new CreateCaseHandleResponse
            {
                Message = "Kasa başarıyla oluşturuldu.",
                Error = false
            });
        }
    }
}
