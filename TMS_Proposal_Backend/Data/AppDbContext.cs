using Microsoft.EntityFrameworkCore;
using TMS_Proposal_Backend.Models;
using TMS_Proposal_Backend.Models.Audits;
using TMS_Proposal_Backend.Models.Auth;

namespace TMS_Proposal_Backend.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<AuditLogs> AuditLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.CreatedByUser)
                .WithMany(a => a.Requests)
                .HasForeignKey(r => r.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict); // if admin delete one user ,no deleted all records its .If want to delete use .Cascade

            modelBuilder.Entity<AuditLogs>()
                .HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UserId);
        }



    }


}
