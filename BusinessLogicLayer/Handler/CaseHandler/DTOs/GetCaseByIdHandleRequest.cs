using MediatR;

namespace BusinessLogicLayer.Handler.CaseHandler.DTOs
{
    public class GetCaseByIdHandleRequest : IRequest<GetCaseByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
