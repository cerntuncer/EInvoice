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
            string? message = null;

            // --- Varlık kontrolü ---
            var kasa = _caseRepository.Find(request.Id);
            if (kasa == null)
            {
                message = "Kasa bulunamadı.";
            }
            else
            {
                // --- Sahiplik kontrolü ---
                if (kasa.CurrentId != request.CurrentId)
                    message = "Bu kasa belirtilen cari hesaba ait değil.";

                // (opsiyonel) Zaten pasif mi?
                if (message == null && kasa.Status == Status.Passive)
                    message = "Kasa zaten pasif durumda.";
            }

            if (message != null)
            {
                return new DeleteCaseHandleResponse
                {
                    Error = true,
                    Message = message
                };
            }

            // --- Soft delete ---
            kasa!.Status = Status.Passive;
            _caseRepository.Update(kasa);

            return new DeleteCaseHandleResponse
            {
                Error = false,
                Message = "Kasa başarıyla silindi (pasif hale getirildi)."
            };
        }
    }
}
