using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CurrentHandler.Commands
{
    public class DeleteCurrentHandle : IRequestHandler<DeleteCurrentHandleRequest, DeleteCurrentHandleResponse>
    {
        private readonly ICurrentRepository _repository;

        public DeleteCurrentHandle(ICurrentRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeleteCurrentHandleResponse> Handle(DeleteCurrentHandleRequest request, CancellationToken cancellationToken)
        {
            string? message = null;

            // --- Varlık kontrolü ---
            var current = _repository.Find(request.Id);
            if (current == null)
            {
                message = "Cari hesap bulunamadı.";
            }
            else
            {
                // --- Sahiplik kontrolü ---
                if (current.UserId != request.UserId)
                    message = "Bu cari hesap belirtilen kullanıcıya ait değil.";

                // (opsiyonel) Zaten pasif mi?
                if (message == null && current.Status == Status.Passive)
                    message = "Cari hesap zaten pasif durumda.";
            }

            if (message != null)
            {
                return new DeleteCurrentHandleResponse
                {
                    Error = true,
                    Message = message
                };
            }

            // --- Soft delete ---
            current!.Status = Status.Passive;
            _repository.Update(current);

            return new DeleteCurrentHandleResponse
            {
                Error = false,
                Message = "Cari hesap başarıyla silindi (pasif hale getirildi)."
            };
        }
    }
}
