/* 
 * https://www.w3.org/TR/webauthn/#dictdef-publickeycredentialcreationoptions 
 * https://developer.mozilla.org/en-US/docs/Web/API/PublicKeyCredentialCreationOptions
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Indice.AspNetCore.Fido.Models
{
    /// <summary>
    /// Models the data used in the creation of a new public key credential source, bound to an authenticator.
    /// </summary>
    public class PublicKeyCredentialCreationOptionsBase64
    {
        /// <summary>
        /// Creates a new instance of <see cref="PublicKeyCredentialCreationOptionsBase64"/>.
        /// </summary>
        /// <param name="relyingParty">Contains data about the Relying Party responsible for the request.</param>
        /// <param name="userHandle">Used to map a specific public key credential to a specific user account with the Relying Party.</param>
        /// <param name="challenge">Intended to be used for generating the newly created credential’s attestation object.</param>
        /// <param name="userId">A unique identifier for the user.</param>
        /// <param name="deviceFriendlyName">A friendly name for the authenticator that is about to register.</param>
        /// <param name="excludeCredentials">Descriptors for the public keys already existing for the user.</param>
        public PublicKeyCredentialCreationOptionsBase64(RelyingPartyEntity relyingParty, byte[] userHandle, byte[] challenge, string userId, string deviceFriendlyName, IEnumerable<byte[]> excludeCredentials = null) {
            RelyingParty = relyingParty ?? throw new ArgumentNullException(nameof(relyingParty));
            UserHandleBase64 = userHandle != null ? Convert.ToBase64String(userHandle) : throw new ArgumentNullException(nameof(userHandle));
            ChallengeBase64 = challenge != null ? Convert.ToBase64String(challenge) : throw new ArgumentNullException(nameof(challenge));
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            DeviceFriendlyName = deviceFriendlyName;
            ExcludeCredentialsBase64 = excludeCredentials?.Select(credential => Convert.ToBase64String(credential));
        }

        /// <summary>
        /// Contains data about the Relying Party responsible for the request.
        /// </summary>
        /// <remarks>https://www.w3.org/TR/webauthn/#dom-publickeycredentialcreationoptions-rp</remarks>
        public RelyingPartyEntity RelyingParty { get; }
        /// <summary>
        /// Used to map a specific public key credential to a specific user account with the Relying Party.
        /// </summary>
        /// <remarks>https://www.w3.org/TR/webauthn/#dictdef-publickeycredentialuserentity</remarks>
        public string UserHandleBase64 { get; }
        /// <summary>
        /// Intended to be used for generating the newly created credential’s attestation object.
        /// </summary>
        /// <remarks>https://www.w3.org/TR/webauthn/#dom-publickeycredentialcreationoptions-challenge</remarks>
        public string ChallengeBase64 { get; }
        /// <summary>
        /// A unique identifier for the user.
        /// </summary>
        public string UserId { get; }
        /// <summary>
        /// A friendly name for the authenticator that is about to register.
        /// </summary>
        public string DeviceFriendlyName { get; set; }
        /// <summary>
        /// Descriptors for the public keys already existing for the user.
        /// </summary>
        /// <remarks>
        /// https://www.w3.org/TR/webauthn/#dom-publickeycredentialcreationoptions-excludecredentials
        /// https://developer.mozilla.org/en-US/docs/Web/API/PublicKeyCredentialCreationOptions/excludeCredentials
        /// </remarks>
        public IEnumerable<string> ExcludeCredentialsBase64 { get; set; }
    }
}
