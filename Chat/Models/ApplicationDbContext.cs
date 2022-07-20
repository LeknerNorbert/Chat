using Microsoft.EntityFrameworkCore;

namespace Chat.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        public DbSet<Emoji> Emojis { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
