using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Models;

namespace TicketSystem.Data
{
    public class MyDbContext : IdentityDbContext<User,Role,int>
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketFeedBack> TicketFeedBacks { get; set; }
        public DbSet<TicketAssignment> TicketAssignments { get; set; }
        public DbSet<TicketFeedbackAssignee> TicketFeedbackAssignees { get; set; }
        public DbSet<SearchTicketResult> SearchTicketResults { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Message>()
                  .HasOne(m => m.Sender)
                  .WithMany()
                  .HasForeignKey(m => m.SenderID)
                  .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverID)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<IdentityUserToken<int>>().HasKey(userToken => new { userToken.UserId, userToken.LoginProvider, userToken.Name });
            modelBuilder.Entity<IdentityUserLogin<int>>().HasKey(login => login.UserId);
            modelBuilder.Entity<IdentityUserRole<int>>().HasKey(userRole => new { userRole.UserId, userRole.RoleId });
            modelBuilder.Entity<IdentityUserClaim<int>>().HasKey(userClaim => userClaim.Id);
            modelBuilder.Entity<IdentityRoleClaim<int>>().HasKey(roleClaim => roleClaim.Id);
            modelBuilder.Entity<Role>().HasKey(role => role.Id);
            modelBuilder.Entity<User>()
               .HasIndex(u => u.Email)
               .IsUnique(); 
            modelBuilder.Entity<User>()
                .HasIndex(u => u.NationalID)
                .IsUnique(); 
            modelBuilder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique(); 
            modelBuilder.Entity<Ticket>()
            .HasIndex(t => t.TicketID) 
            .IsUnique(); 
            modelBuilder.Entity<Department>().HasIndex(d => d.DepartmentName).IsUnique();
            modelBuilder.Entity<TicketFeedBack>()
           .HasOne(tf => tf.User)
           .WithMany()
           .HasForeignKey(tf => tf.CreatedBy)
           .OnDelete(DeleteBehavior.NoAction);
         
            modelBuilder.Entity<TicketFeedbackAssignee>()
                .HasOne(tfa => tfa.Ticket)
                .WithMany()  
                .HasForeignKey(tfa => tfa.TicketID)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<TicketFeedbackAssignee>()
                .HasOne(tfa => tfa.User)
                .WithMany()
                .HasForeignKey(tfa => tfa.AssignedTo)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<User>()
           .Property(u => u.CreatedAt)
           .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<User>()
           .ToTable(t => t.HasCheckConstraint("CK_User_Gender", "Gender IN (N'Nam', N'Nữ', N'Khác')"));
            modelBuilder.Entity<User>()
            .ToTable(tb => tb.HasCheckConstraint("CHK_User_Status", "Status IN (N'Active', N'Inactive')")); 

            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Ticket>()
           .ToTable(tb => tb.HasCheckConstraint("CHK_Ticket_Priority", "Priority IN (N'Thấp', N'Trung bình', N'Cao', N'Khẩn cấp')"));
            modelBuilder.Entity<Ticket>()
                .ToTable(tb => tb.HasCheckConstraint("CHK_Ticket_Status", "Status IN (N'Mới', N'Đang xử lý', N'Chờ xác nhận', N'Hoàn thành', N'Đã hủy',N'Cháy Deadline')"));

            modelBuilder.Entity<Category>()
            .HasOne(c => c.Department)
            .WithMany()
            .HasForeignKey(c => c.DepartmentID)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketAssignment>()
           .HasOne(t => t.Ticket)
           .WithMany()  
           .HasForeignKey(t => t.TicketID)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketAssignment>()
                .HasOne(t => t.User)
                .WithMany() 
                .HasForeignKey(t => t.AssignedTo)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SearchTicketResult>().HasNoKey();
        }
    }
}
