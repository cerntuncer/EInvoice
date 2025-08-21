using DatabaseAccessLayer.Enumerations;

namespace PresentationLayer.Models.ApiRequests
{
    public class CreateCurrentRequest
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public CurrentType CurrentType { get; set; }
        public long UserId { get; set; }
    }
}
