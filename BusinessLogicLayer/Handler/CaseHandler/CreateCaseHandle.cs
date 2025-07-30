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
        private readonly IUserRepository _userRepository;

        public CreateCaseHandle(
            ICaseRepository caseRepository,
            ICurrentRepository currentRepository,
            IUserRepository userRepository)
        {
            _caseRepository = caseRepository;
            _currentRepository = currentRepository;
            _userRepository = userRepository;
        }

        public async Task<CreateCaseHandleResponse> Handle(CreateCaseHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;

            if (string.IsNullOrWhiteSpace(request.Name))
                message = "Kasa adı boş olamaz.";
            else if (string.IsNullOrWhiteSpace(request.Address) || request.Address.Length > 100)
                message = "Adres 100 karakteri geçemez.";
            else if (request.UserId <= 0)
                message = "Geçerli bir kullanıcı ID girilmelidir.";
            else if (_userRepository.Find(request.UserId) == null)
                message = "Kullanıcı bulunamadı.";

            if (message != null)
            {
                return new CreateCaseHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            // 🔄 Current (Cari Hesap) otomatik oluşturuluyor
            var current = new Current
            {
                Name = request.Name,
                UserId = request.UserId,
                Status = Status.Active
            };
            _currentRepository.Add(current);

            // 🧱 Case oluşturuluyor
            var kasa = new Case
            {
                Address = request.Address,
                CurrentId = current.Id,
                Status = request.Status
            };
            _caseRepository.Add(kasa);

            return new CreateCaseHandleResponse
            {
                Message = "Kasa ve cari hesap başarıyla oluşturuldu.",
                Error = false
            };
        }
    }
}
