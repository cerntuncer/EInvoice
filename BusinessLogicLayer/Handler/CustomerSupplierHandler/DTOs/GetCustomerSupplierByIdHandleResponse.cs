using DatabaseAccessLayer.Enumerations;

namespace BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs
{
    public class GetCustomerSupplierByIdHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public long Id { get; set; }
        public CustomerOrSupplierType Type { get; set; }
        public long PersonId { get; set; }
        public Status Status { get; set; }
    }
}
