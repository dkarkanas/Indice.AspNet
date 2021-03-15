/* WebAuthn Relying Party Operations: https://www.w3.org/TR/webauthn/#sctn-rp-operations */

using System.Threading.Tasks;
using Indice.AspNetCore.Fido.Models;

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
        /// <param name="userId">A unique identifier for the user.</param>
        /// <param name="deviceFriendlyName">A friendly name for the authenticator that is about to register.</param>
        /// <returns>A cryptographic challenge value.</returns>
        Task<PublicKeyCredentialCreationOptionsBase64> InitiateRegistration(string userId, string deviceFriendlyName = null);
        /// <summary>
        /// Receives the result of the client which created the credentials and validates it.
        /// </summary>
        /// <returns>The result as a <see cref="FidoRegistrationResult"/> which determines if the operation was successful or not and the errors occured, if any.</returns>
        Task<FidoRegistrationResult> CompleteRegistration(PublicKeyCredential response);
    }
}
