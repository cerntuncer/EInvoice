using DatabaseAccessLayer.Enumerations;

namespace PresentationLayer.Models.ApiRequests
{
    public class UpdateBankProxyRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Iban { get; set; }
        public int BranchCode { get; set; }
        public int AccountNo { get; set; }
        public Status Status { get; set; }
        public long CurrentId { get; set; }
    }
}

