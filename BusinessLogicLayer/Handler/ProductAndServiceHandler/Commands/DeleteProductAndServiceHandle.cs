// DeleteProductAndServiceHandle.cs
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class DeleteProductAndServiceHandle : IRequestHandler<DeleteProductAndServiceHandleRequest, DeleteProductAndServiceHandleResponse>
{
    private readonly IProductAndServiceRepository _repository;

    public DeleteProductAndServiceHandle(IProductAndServiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeleteProductAndServiceHandleResponse> Handle(DeleteProductAndServiceHandleRequest request, CancellationToken cancellationToken)
    {
        var entity = _repository.Find(request.Id);
        if (entity == null || entity.Status == Status.Passive)
        {
            return new DeleteProductAndServiceHandleResponse
            {
                Error = true,
                Message = "Ürün/Hizmet bulunamadı veya zaten pasif durumda."
            };
        }

        entity.Status = Status.Passive;
        _repository.Update(entity);

        return new DeleteProductAndServiceHandleResponse
        {
            Error = false,
            Message = "Ürün/Hizmet silindi."
        };
    }
}
