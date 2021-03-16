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
        /// Get the persisted challenge.
        /// </summary>
        /// <returns>The found registration challenge, otherwise null.</returns>
        Task<PublicKeyCredentialCreationOptionsBase64> Get();
        /// <summary>
        /// Persists the given challenge.
        /// </summary>
        /// <param name="publicKeyCredentialCreationOptions">Models the data used in the creation of a new public key credential source, bound to an authenticator.</param>
        Task Persist(PublicKeyCredentialCreationOptionsBase64 publicKeyCredentialCreationOptions);
    }
}
