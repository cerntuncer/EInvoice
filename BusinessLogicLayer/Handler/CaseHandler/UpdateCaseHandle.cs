using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CaseHandler
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
            var kasa = _caseRepository.Find(request.Id);

            if (kasa == null)
            {
                return new UpdateCaseHandleResponse
                {
                    Error = true,
                    Message = "Kasa bulunamadı."
                };
            }

            if (kasa.CurrentId != request.CurrentId)
            {
                return new UpdateCaseHandleResponse
                {
                    Error = true,
                    Message = "Bu kasa belirtilen cari hesaba ait değil."
                };
            }

            if (string.IsNullOrWhiteSpace(request.Address) || request.Address.Length > 100)
            {
                return new UpdateCaseHandleResponse
                {
                    Error = true,
                    Message = "Adres boş olamaz ve 100 karakteri geçemez."
                };
            }

            kasa.Address = request.Address;
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
