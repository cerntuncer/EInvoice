namespace PresentationLayer.Models.ApiRequests
{
    public class CreateBankProxyRequest
    {
        public string Name { get; set; }
        public string Iban { get; set; }
        public int BranchCode { get; set; }
        public int AccountNo { get; set; }
        public long? CurrentId { get; set; }
        public CreateCurrentProxyRequest? Current { get; set; }
    }
}

