/* https://www.w3.org/TR/webauthn/#dictdef-publickeycredentialcreationoptions */

using System;

namespace Indice.AspNetCore.Fido
{
    /// <summary>
    /// Models the data used in the creation of a new public key credential source, bound to an authenticator.
    /// </summary>
    public class PublicKeyCredentialCreationOptions
    {
        /// <summary>
        /// Creates a new instance of <see cref="PublicKeyCredentialCreationOptions"/>.
        /// </summary>
        /// <param name="relyingParty">Contains data about the Relying Party responsible for the request.</param>
        /// <param name="userHandle">Used to map a specific public key credential to a specific user account with the Relying Party.</param>
        /// <param name="challenge">Intended to be used for generating the newly created credential’s attestation object.</param>
        /// <param name="userId">A unique identifier for the user.</param>
        public PublicKeyCredentialCreationOptions(RelyingPartyEntity relyingParty, byte[] userHandle, byte[] challenge, string userId) {
            RelyingParty = relyingParty;
            UserHandleBase64 = Convert.ToBase64String(userHandle);
            ChallengeBase64 = Convert.ToBase64String(challenge);
            UserId = userId;
        }

        /// <summary>
        /// Contains data about the Relying Party responsible for the request.
        /// </summary>
        public RelyingPartyEntity RelyingParty { get; }
        /// <summary>
        /// Used to map a specific public key credential to a specific user account with the Relying Party.
        /// </summary>
        public string UserHandleBase64 { get; }
        /// <summary>
        /// Intended to be used for generating the newly created credential’s attestation object.
        /// </summary>
        public string ChallengeBase64 { get; }
        /// <summary>
        /// A unique identifier for the user.
        /// </summary>
        public string UserId { get; }
    }
}
