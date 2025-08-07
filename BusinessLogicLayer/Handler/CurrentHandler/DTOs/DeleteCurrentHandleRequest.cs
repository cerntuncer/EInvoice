using MediatR;

public class DeleteCurrentHandleRequest : IRequest<DeleteCurrentHandleResponse>
{
    public long Id { get; set; }
    public long UserId { get; set; } // Güvenlik kontrolü
}
