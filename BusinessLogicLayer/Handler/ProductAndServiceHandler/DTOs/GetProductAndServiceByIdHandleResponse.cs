using DatabaseAccessLayer.Enumerations;

namespace BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs
{
    public class GetProductAndServiceByIdHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public UnitType UnitType { get; set; }
        public Status Status { get; set; }
    }
}
