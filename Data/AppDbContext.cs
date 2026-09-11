using Microsoft.EntityFrameworkCore;
using Talk2Me.Models;

namespace Talk2Me.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketPhoto> TicketPhotos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UserA
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.UserA)
                .WithMany(u => u.SentChats)
                .HasForeignKey(c => c.UserAId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserB
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.UserB)
                .WithMany(u => u.ReceivedChats)
                .HasForeignKey(c => c.UserBId)
                .OnDelete(DeleteBehavior.Restrict);

            // Chat -> Message
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Message
            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Notification
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Forum
            modelBuilder.Entity<Forum>()
                .HasOne(f => f.Creator)
                .WithMany(u => u.Forums)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Theme -> Forum
            modelBuilder.Entity<Forum>()
                .HasOne(f => f.Theme)
                .WithMany(t => t.Forums)
                .HasForeignKey(f => f.ThemeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Forum -> Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Forum)
                .WithMany(f => f.Comments)
                .HasForeignKey(c => c.ForumId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment -> Photo
            modelBuilder.Entity<Photo>()
                .HasOne(p => p.Comment)
                .WithMany(c => c.Photos)
                .HasForeignKey(p => p.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Comment -> ParentComment (self-referencing)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany()
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // The user who owns the friendship
            modelBuilder.Entity<Friend>()
                .HasOne(f => f.User)
                .WithMany(u => u.Friends)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // The user who is the friend
            modelBuilder.Entity<Friend>()
                .HasOne(f => f.FriendUser)
                .WithMany()
                .HasForeignKey(f => f.FriendUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Sender
            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Sender)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(fr => fr.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Receiver
            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Reaction
            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.Reactor)
                .WithMany(u => u.Reactions)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Forum -> Reaction
            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.Forum)
                .WithMany(f => f.Reactions)
                .HasForeignKey(r => r.ForumId)
                .OnDelete(DeleteBehavior.Cascade);

            // Comment -> Reaction
            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.Comment)
                .WithMany()
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ticket -> User
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket -> Admin
            modelBuilder.Entity<Ticket>()
               .HasOne(t => t.Admin)
               .WithMany()
               .HasForeignKey(t => t.AdminID)
               .OnDelete(DeleteBehavior.Restrict);

            // Ticket -> TicketPhoto
            modelBuilder.Entity<TicketPhoto>()
             .HasOne(p => p.Ticket)
             .WithMany(t => t.Photos)
             .HasForeignKey(p => p.TicketId)
             .OnDelete(DeleteBehavior.Cascade);

            // Indexes and Constraints

            // Usernames should be unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            // Prevent duplicate friendships
            modelBuilder.Entity<Friend>()
                .HasIndex(f => new { f.UserId, f.FriendUserId })
                .IsUnique();

            // Prevent duplicate pending/created requests
            modelBuilder.Entity<FriendRequest>()
                .HasIndex(fr => new { fr.SenderId, fr.ReceiverId });

            // Useful indexes
            modelBuilder.Entity<Message>()
                .HasIndex(m => new { m.ChatId, m.SendDate });

            modelBuilder.Entity<Comment>()
                .HasIndex(c => c.ForumId);

            modelBuilder.Entity<Notification>()
                .HasIndex(n => new { n.UserId, n.IsRead });
        }
    }
}
