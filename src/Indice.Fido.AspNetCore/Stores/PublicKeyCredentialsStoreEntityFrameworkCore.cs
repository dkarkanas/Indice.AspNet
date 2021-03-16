using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indice.AspNetCore.Fido.Models;
using Indice.AspNetCore.Fido.Stores;
using Microsoft.EntityFrameworkCore;

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
        public async Task<FidoPublicKeyCredential> GetById(byte[] id) {
            if (id == null) {
                throw new ArgumentNullException(nameof(id), $"Public key credential id cannot be null.");
            }
            var key = await _dbContext.FidoPublicKeyCredentials.SingleOrDefaultAsync(x => x.Id == id);
            return key?.ToModel();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<byte[]>> GetUserCredentials(string userId) {
            if (string.IsNullOrWhiteSpace(userId)) {
                throw new ArgumentNullException(nameof(userId), $"Parameter {nameof(userId)} cannot be null or empty.");
            }
            var existingCredentials = await _dbContext.FidoPublicKeyCredentials.Where(x => x.UserId == userId).Select(x => x.Id).ToListAsync();
            return existingCredentials;
        }
    }
}
