namespace PresentationLayer.Models.ApiRequests
{
    public class UpdatePersonRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long IdentityNumber { get; set; }
        public string TaxOffice { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
    }
}