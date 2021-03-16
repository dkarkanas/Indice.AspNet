using Microsoft.EntityFrameworkCore;

namespace Indice.AspNetCore.Fido.EntityFrameworkCore
{
    public class FidoDbContext : DbContext
    {
        public FidoDbContext(DbContextOptions<FidoDbContext> dbContextOptions) : base(dbContextOptions) {
#if DEBUG
            Database.EnsureCreated();
#endif
        }

        public DbSet<DbFidoPublicKeyCredential> FidoPublicKeyCredentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DbFidoPublicKeyCredentialMap());
        }
    }
}
