using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CaseHandler
{
    public class CreateCaseHandleRequest : IRequest<CreateCaseHandleResponse>
    {
        public string Name { get; set; }         // Current için ad
        public string Address { get; set; }      // Case için adres
        public Status Status { get; set; }       // Durum (opsiyonel olabilir)
    }
}
