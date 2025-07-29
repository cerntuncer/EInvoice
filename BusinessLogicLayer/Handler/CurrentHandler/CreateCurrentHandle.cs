using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CurrentHandler
{
    public class CreateCurrentHandle : IRequestHandler<CreateCurrentHandleRequest, CreateCurrentHandleResponse>
    {
        private readonly ICurrentRepository _currentRepository;

        public CreateCurrentHandle(ICurrentRepository currentRepository)
        {
            _currentRepository = currentRepository;
        }

        public async Task<CreateCurrentHandleResponse> Handle(CreateCurrentHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;

            if (string.IsNullOrWhiteSpace(request.Name))
                message = "İsim boş olamaz.";

            if (request.UserId <= 0)
                message = "UserId geçerli değil.";

            if (message != null)
                return new CreateCurrentHandleResponse { Message = message, Error = true };

            var current = new Current
            {
                Name = request.Name,
                Balance = request.Balance,
                CurrencyType = request.CurrencyType,
                CurrentType = request.CurrentType,
                UserId = request.UserId,
                Status = Status.Active
            };

            _currentRepository.Add(current);

            return new CreateCurrentHandleResponse
            {
                Message = "Current başarıyla oluşturuldu.",
                Error = false
            };
        }
    }
}
