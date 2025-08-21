namespace PresentationLayer.Models.ApiRequests
{
    public class UpdateUserRequest
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
    }
}

