using DatabaseAccessLayer.Enumerations;
namespace BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs
{
    public class GetProductsAndServicesByUserIdHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public List<ProductAndServiceListItemDto> Items { get; set; } = new();
    }

    public class ProductAndServiceListItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public UnitType UnitType { get; set; }
        public Status Status { get; set; }
    }
}