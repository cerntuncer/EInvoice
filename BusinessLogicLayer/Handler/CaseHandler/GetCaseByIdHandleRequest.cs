using MediatR;

namespace BusinessLogicLayer.Handler.CaseHandler
{
    public class GetCaseByIdHandleRequest : IRequest<GetCaseByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
