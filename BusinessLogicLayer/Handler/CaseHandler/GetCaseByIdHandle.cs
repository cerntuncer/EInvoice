using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using MediatR;

namespace BusinessLogicLayer.Handler.CaseHandler
{
    public class GetCaseByIdHandle : IRequestHandler<GetCaseByIdHandleRequest, GetCaseByIdHandleResponse>
    {
        private readonly ICaseRepository _caseRepository;

        public GetCaseByIdHandle(ICaseRepository caseRepository)
        {
            _caseRepository = caseRepository;
        }

        public async Task<GetCaseByIdHandleResponse> Handle(GetCaseByIdHandleRequest request, CancellationToken cancellationToken)
        {
            var kasa =  _caseRepository.GetById(request.Id);

            if (kasa == null)
            {
                return new GetCaseByIdHandleResponse
                {
                    Error = true,
                    Message = "Kasa bulunamadı."
                };
            }

            return new GetCaseByIdHandleResponse
            {
                Error = false,
                Message = "Kasa başarıyla getirildi.",
                Id = kasa.Id,
                Address = kasa.Address,
                CurrentId = kasa.CurrentId,
                Status = kasa.Status
            };
        }
    }
}
