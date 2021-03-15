using System.Collections.Generic;
using System.Threading.Tasks;
using Indice.AspNetCore.Fido.Models;

namespace Indice.AspNetCore.Fido.Stores
{
    /// <summary>
    /// An abstraction for the store that manages the public key credentials used in FIDO2.
    /// </summary>
    public interface IPublicKeyCredentialsStore
    {
        /// <summary>
        /// Retrieves a public key credential by it's id.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>The found key, otherwise null.</returns>
        Task<FidoPublicKeyCredential> GetById(byte[] id);
        /// <summary>
        /// Gets the public key credential that have been created for the specified user.
        /// </summary>
        /// <param name="userId">A unique identifier for the user.</param>
        Task<IEnumerable<byte[]>> GetUserCredentials(string userId);
    }
}
