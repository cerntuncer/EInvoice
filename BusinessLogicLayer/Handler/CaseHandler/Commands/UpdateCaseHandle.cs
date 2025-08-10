using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.CaseHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CaseHandler.Commands
{
    public class UpdateCaseHandle : IRequestHandler<UpdateCaseHandleRequest, UpdateCaseHandleResponse>
    {
        private readonly ICaseRepository _caseRepository;

        public UpdateCaseHandle(ICaseRepository caseRepository)
        {
            _caseRepository = caseRepository;
        }

        public async Task<UpdateCaseHandleResponse> Handle(UpdateCaseHandleRequest request, CancellationToken cancellationToken)
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

                // --- Adres kontrolü ---
                if (message == null)
                {
                    var addr = request.Address?.Trim();
                    if (string.IsNullOrWhiteSpace(addr) || addr!.Length > 100)
                        message = "Adres boş olamaz ve 100 karakteri geçemez.";
                }

                // --- Status enum kontrolü ---
                if (message == null && !Enum.IsDefined(typeof(Status), request.Status))
                    message = "Geçersiz durum bilgisi.";
            }

            if (message != null)
            {
                return new UpdateCaseHandleResponse
                {
                    Error = true,
                    Message = message
                };
            }

            // --- Güncelleme ---
            kasa!.Address = request.Address!.Trim();
            kasa.Status = request.Status;

            _caseRepository.Update(kasa);

            return new UpdateCaseHandleResponse
            {
                Error = false,
                Message = "Kasa başarıyla güncellendi."
            };
        }
    }
}
