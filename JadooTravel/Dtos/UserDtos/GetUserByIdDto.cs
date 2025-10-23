namespace JadooTravel.Dtos.UserDtos
{
    public class GetUserByIdDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
