namespace PresentationLayer.Models.ApiResponses
{
    public class GetUsersWithPersonListResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public List<UserWithPersonListItem> Users { get; set; } = new();
    }

    public class UserWithPersonListItem
    {
        public long UserId { get; set; }
        public int UserType { get; set; }
        public long PersonId { get; set; }
        public string PersonName { get; set; }
    }
}

