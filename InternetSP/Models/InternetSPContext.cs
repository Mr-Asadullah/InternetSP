using Microsoft.EntityFrameworkCore;

namespace InternetSP.Models
{
    public class InternetSPContext:DbContext
    {
        public InternetSPContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
                
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Packge> Packges { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Speed> Speeds { get; set; }
        public DbSet<Volume> Volumes { get; set; }
        public DbSet<SubscribePkg> SubscribePkgs { get; set; }
    }
}
