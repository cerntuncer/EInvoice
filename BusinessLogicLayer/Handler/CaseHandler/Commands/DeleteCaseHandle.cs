using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.CaseHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CaseHandler.Commands
{
    public class DeleteCaseHandle : IRequestHandler<DeleteCaseHandleRequest, DeleteCaseHandleResponse>
    {
        private readonly ICaseRepository _caseRepository;

        public DeleteCaseHandle(ICaseRepository caseRepository)
        {
            _caseRepository = caseRepository;
        }

        public async Task<DeleteCaseHandleResponse> Handle(DeleteCaseHandleRequest request, CancellationToken cancellationToken)
        {
            var kasa = _caseRepository.Find(request.Id);

            if (kasa == null)
            {
                return new DeleteCaseHandleResponse
                {
                    Error = true,
                    Message = "Kasa bulunamadı."
                };
            }

            if (kasa.CurrentId != request.CurrentId)
            {
                return new DeleteCaseHandleResponse
                {
                    Error = true,
                    Message = "Bu kasa belirtilen cari hesaba ait değil."
                };
            }

            kasa.Status = Status.Passive;
            _caseRepository.Update(kasa);

            return new DeleteCaseHandleResponse
            {
                Error = false,
                Message = "Kasa başarıyla silindi (pasif hale getirildi)."
            };
        }
    }
}
