/* WebAuthn Relying Party Operations: https://www.w3.org/TR/webauthn/#sctn-rp-operations */

using System.Threading.Tasks;

namespace Indice.AspNetCore.Fido
{
    /// <summary>
    /// A service that contains WebAuthn Relying Party operations, regarding registration or authentication ceremony.
    /// </summary>
    public interface IFidoAuthenticationService
    {
        /// <summary>
        /// Initiates the ceremony where a user, a Relying Party, and the user’s client application work in concert to create a public key credential and associate it with the user’s Relying Party account.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <param name="deviceFriendlyName">A friendly name for the authenticator to register.</param>
        /// <returns>A cryptographic challenge value.</returns>
        Task<InitiateRegistrationChallenge> InitiateRegistration(string userId, string deviceFriendlyName = null);
    }
}
