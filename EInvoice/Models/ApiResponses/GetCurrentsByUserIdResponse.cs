namespace PresentationLayer.Models.ApiResponses
{
    public class GetCurrentsByUserIdResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }

        public List<CurrentWithDetailsItem> Currents { get; set; } = new();
    }

    public class CurrentWithDetailsItem
    {
        public long CurrentId { get; set; }
        public string CurrentName { get; set; }
        public decimal Balance { get; set; }
        public int CurrencyType { get; set; }
        public int CurrentType { get; set; }
    }
}