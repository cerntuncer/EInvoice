using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;

namespace BusinessLogicLayer.Handler.CurrentHandler
{
    public class GetCurrentsByUserIdHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }

        public List<CurrentWithDetailsDto> Currents { get; set; } = new();
    }

    public class CurrentWithDetailsDto
    {
        public long CurrentId { get; set; }
        public string CurrentName { get; set; }
        public decimal Balance { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public CurrentType CurrentType { get; set; }

        public Case? CaseInfo { get; set; }
        public Bank? BankInfo { get; set; }
    }

}
