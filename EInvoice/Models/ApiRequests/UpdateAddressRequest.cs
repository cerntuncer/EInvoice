namespace PresentationLayer.Models.ApiRequests
{
	public class UpdateAddressRequest
	{
		public long Id { get; set; }
		public int AddressType { get; set; }
		public string Text { get; set; }
	}
}
