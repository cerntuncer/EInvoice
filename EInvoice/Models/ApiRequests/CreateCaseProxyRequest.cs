using DatabaseAccessLayer.Enumerations;

namespace PresentationLayer.Models.ApiRequests
{
    public class CreateCaseProxyRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public Status Status { get; set; }
        public long? CurrentId { get; set; }
        public CreateCurrentProxyRequest? Current { get; set; }
    }
}

