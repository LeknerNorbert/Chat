namespace Chat.Models.DTOs
{
    public class FriendRequestDto
    {
        public uint Id { get; set; }
        public UserDto Sender { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime Created { get; set; }
    }
}
