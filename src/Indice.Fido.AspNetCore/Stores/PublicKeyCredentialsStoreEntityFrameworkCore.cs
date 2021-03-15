using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Indice.AspNetCore.Fido.Models;
using Indice.AspNetCore.Fido.Stores;

namespace Indice.AspNetCore.Fido.EntityFrameworkCore
{
    /// <summary>
    /// An implementation of <see cref="IPublicKeyCredentialsStore"/> where keys are stored in a database using Entity Framework Core.
    /// </summary>
    public class PublicKeyCredentialsStoreEntityFrameworkCore : IPublicKeyCredentialsStore
    {
        private readonly FidoDbContext _dbContext;

        public PublicKeyCredentialsStoreEntityFrameworkCore(FidoDbContext dbContext) {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc />
        public Task<FidoPublicKeyCredential> GetById(byte[] id) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IEnumerable<byte[]>> GetUserCredentials(string userId) {
            throw new NotImplementedException();
        }
    }
}
