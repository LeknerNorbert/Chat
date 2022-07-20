namespace Chat.Models
{
    public class PrivateMessage
    {
        public uint Id { get; set; }
        public string? Message { get; set; }
        public User? Sender { get; set; }
        public User? Receiver { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
