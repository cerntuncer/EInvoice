using DatabaseAccessLayer.Enumerations;

namespace BusinessLogicLayer.Handler.AddressHandler.DTOs
{
    public class GetAddressesByPersonIdResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public List<AddressDto> Addresses { get; set; } = new();
    }

    public class AddressDto
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public AddressType AddressType { get; set; }
        public Status Status { get; set; }
    }

}
