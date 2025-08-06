using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CurrentHandler
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
            var current = _repository.Find(request.Id);

            if (current == null)
            {
                return new DeleteCurrentHandleResponse
                {
                    Error = true,
                    Message = "Cari hesap bulunamadı."
                };
            }

            if (current.UserId != request.UserId)
            {
                return new DeleteCurrentHandleResponse
                {
                    Error = true,
                    Message = "Bu cari hesap belirtilen kullanıcıya ait değil."
                };
            }

            current.Status = Status.Passive;
            _repository.Update(current);

            return new DeleteCurrentHandleResponse
            {
                Error = false,
                Message = "Cari hesap başarıyla silindi (pasif hale getirildi)."
            };
        }
    }
}
