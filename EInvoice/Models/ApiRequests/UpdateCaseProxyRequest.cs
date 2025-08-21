using DatabaseAccessLayer.Enumerations;

namespace PresentationLayer.Models.ApiRequests
{
    public class UpdateCaseProxyRequest
    {
        public long Id { get; set; }
        public long CurrentId { get; set; }
        public string Address { get; set; }
        public Status Status { get; set; }
    }
}

