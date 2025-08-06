using DatabaseAccessLayer.Enumerations;

namespace BusinessLogicLayer.Handler.CurrentHandler
{
    public class GetCurrentByIdHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public CurrentType CurrentType { get; set; }
        public long UserId { get; set; }
        public Status Status { get; set; }
    }

}
