using DatabaseAccessLayer.Enumerations;

namespace BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs
{
    public class GetCustomerSuppliersHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public List<CustomerSupplierListItemDto> Items { get; set; } = new();
    }

    public class CustomerSupplierListItemDto
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public string PersonName { get; set; }
        public CustomerOrSupplierType Type { get; set; }
        public Status Status { get; set; }
    }
}
