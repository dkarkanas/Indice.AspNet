using System.Threading.Tasks;
using Indice.AspNetCore.Fido.Models;

namespace Indice.AspNetCore.Fido.Stores
{
    /// <summary>
    /// Abstracts the way of persisting the generated challenge during registration initiation.
    /// </summary>
    public interface IRegistrationChallengeStore
    {
        /// <summary>
        /// Get the persisted challenge using the given key.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <returns>The found registration challenge, otherwise null.</returns>
        Task<PublicKeyCredentialCreationOptionsBase64> GetByKey(string key);
        /// <summary>
        /// Persists the given challenge under the specified key.
        /// </summary>
        /// <param name="key">The key to use.</param>
        /// <param name="publicKeyCredentialCreationOptions">Models the data used in the creation of a new public key credential source, bound to an authenticator.</param>
        Task Persist(string key, PublicKeyCredentialCreationOptionsBase64 publicKeyCredentialCreationOptions);
    }
}
