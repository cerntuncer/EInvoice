using DatabaseAccessLayer.Enumerations;

namespace BusinessLogicLayer.Handler.UserHandler.DTOs
{
    public class GetUsersWithPersonListHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public List<UserWithPersonListItemDto> Users { get; set; } = new();
    }

    public class UserWithPersonListItemDto
    {
        public long UserId { get; set; }
        public UserType UserType { get; set; }
        public long PersonId { get; set; }
        public string PersonName { get; set; }
    }
}

