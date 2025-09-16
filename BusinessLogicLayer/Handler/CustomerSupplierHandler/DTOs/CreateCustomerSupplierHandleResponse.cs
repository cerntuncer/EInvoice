namespace BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs
{
    public class CreateCustomerSupplierHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public long? Id { get; set; }
        public long? PersonId { get; set; }
    }
}
