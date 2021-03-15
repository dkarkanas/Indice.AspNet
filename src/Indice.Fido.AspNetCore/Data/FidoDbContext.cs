using Microsoft.EntityFrameworkCore;

namespace Indice.AspNetCore.Fido.EntityFrameworkCore
{
    public class FidoDbContext : DbContext
    {
        public DbSet<DbFidoPublicKeyCredential> FidoPublicKeyCredentials { get; set; }
    }
}
