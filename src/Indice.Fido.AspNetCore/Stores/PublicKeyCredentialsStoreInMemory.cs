using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indice.AspNetCore.Fido.Models;

namespace Indice.AspNetCore.Fido.Stores
{
    /// <summary>
    /// An implementation of <see cref="IPublicKeyCredentialsStore"/> where keys are stored in memory.
    /// It should be used for testing or development purposes and not recommended for production scenarios.
    /// </summary>
    public class PublicKeyCredentialsStoreInMemory : IPublicKeyCredentialsStore
    {
        private static readonly IDictionary<string, IEnumerable<FidoPublicKeyCredential>> _keys;

        /// <summary>
        /// Creates a new instance of <see cref="PublicKeyCredentialsStoreInMemory"/>.
        /// </summary>
        /// <inheritdoc />
        static PublicKeyCredentialsStoreInMemory() {
            /* https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2 */
            _keys = new ConcurrentDictionary<string, IEnumerable<FidoPublicKeyCredential>>();
        }

        /// <summary>
        /// Holds the keys that been created for all users.
        /// </summary>
        public static IDictionary<string, IEnumerable<FidoPublicKeyCredential>> Keys => _keys;

        /// <inheritdoc />
        public Task<FidoPublicKeyCredential> GetById(byte[] id) {
            if (id == null) {
                throw new ArgumentNullException(nameof(id), $"Public key credential id cannot be null.");
            }
            /* https://docs.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 */
            var key = _keys.SelectMany(x => x.Value).SingleOrDefault(x => ((ReadOnlySpan<byte>)x.Id).SequenceEqual(id));
            return Task.FromResult(key);
        }

        /// <inheritdoc />
        public Task<IEnumerable<byte[]>> GetUserCredentials(string userId) {
            if (string.IsNullOrWhiteSpace(userId)) {
                throw new ArgumentNullException(nameof(userId), $"Parameter {nameof(userId)} cannot be null or empty.");
            }
            if (!_keys.ContainsKey(userId)) {
                return Task.FromResult((IEnumerable<byte[]>)null);
            }
            var existingCredentials = _keys[userId].Select(x => x.Id);
            return Task.FromResult(existingCredentials);
        }
    }
}
