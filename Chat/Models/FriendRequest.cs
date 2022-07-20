namespace Chat.Models
{
    public class FriendRequest
    {
        public uint Id { get; set; }
        public User? Sender { get; set; }
        public User? Receiver { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
