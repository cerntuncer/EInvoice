using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.CurrentHandler.DTOs;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CurrentHandler.Queries
{
    public class GetCurrentsByUserIdHandle : IRequestHandler<GetCurrentsByUserIdHandleRequest, GetCurrentsByUserIdHandleResponse>
    {
        private readonly ICurrentRepository _currentRepository;
        private readonly IBankRepository _bankRepository;
        private readonly ICaseRepository _caseRepository;

        public GetCurrentsByUserIdHandle(
            ICurrentRepository currentRepository,
            IBankRepository bankRepository,
            ICaseRepository caseRepository)
        {
            _currentRepository = currentRepository;
            _bankRepository = bankRepository;
            _caseRepository = caseRepository;
        }

        public async Task<GetCurrentsByUserIdHandleResponse> Handle(GetCurrentsByUserIdHandleRequest request, CancellationToken cancellationToken)
        {
            var currents = _currentRepository
                .Where(c => c.UserId == request.UserId && c.Status != Status.Passive)
                .ToList();

            if (!currents.Any())
            {
                return new GetCurrentsByUserIdHandleResponse
                {
                    Error = true,
                    Message = "Bu kullanıcıya ait cari hesap bulunamadı."
                };
            }

            var result = new GetCurrentsByUserIdHandleResponse
            {
                Error = false,
                Message = "Cari hesaplar başarıyla getirildi.",
                Currents = new List<CurrentWithDetailsDto>()
            };

            foreach (var current in currents)
            {
                var dto = new CurrentWithDetailsDto
                {
                    CurrentId = current.Id,
                    CurrentName = current.Name,
                    Balance = current.Balance,
                    CurrencyType = current.CurrencyType,
                    CurrentType = current.CurrentType
                };

                if (current.CurrentType == CurrentType.Bank)
                {
                    dto.BankInfo = _bankRepository.FirstOrDefault(b => b.CurrentId == current.Id);
                }
                else if (current.CurrentType == CurrentType.Case)
                {
                    dto.CaseInfo = _caseRepository.FirstOrDefault(c => c.CurrentId == current.Id);
                }

                result.Currents.Add(dto);
            }

            return result;
        }
    }
}
