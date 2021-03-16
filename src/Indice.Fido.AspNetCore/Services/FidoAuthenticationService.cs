using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Indice.AspNetCore.Fido.Models;
using Indice.AspNetCore.Fido.Stores;

namespace Indice.AspNetCore.Fido
{
    /// <summary>
    /// A service that contains WebAuthn Relying Party operations, regarding registration or authentication ceremony.
    /// </summary>
    public class FidoAuthenticationService : IFidoAuthenticationService
    {
        // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator
        private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();
        private readonly IPublicKeyCredentialsStore _publicKeyCredentialsStore;
        private readonly IRegistrationChallengeStore _registrationChallengeStore;
        private readonly IRelyingPartyResolver _relyingPartyInfoResolver;

        /// <summary>
        /// Creates a new instance of <see cref="FidoAuthenticationService"/>.
        /// </summary>
        /// <param name="publicKeyCredentialsStore">An abstraction for the store that manages the public key credentials used in FIDO2.</param>
        /// <param name="registrationChallengeStore">Abstracts the way of persisting the generated challenge during registration initiation.</param>
        /// <param name="relyingPartyInfoResolver">A service that helps discover information about the Relying Party.</param>
        public FidoAuthenticationService(
            IPublicKeyCredentialsStore publicKeyCredentialsStore,
            IRegistrationChallengeStore registrationChallengeStore,
            IRelyingPartyResolver relyingPartyInfoResolver
        ) {
            _publicKeyCredentialsStore = publicKeyCredentialsStore ?? throw new ArgumentNullException(nameof(publicKeyCredentialsStore));
            _registrationChallengeStore = registrationChallengeStore ?? throw new ArgumentNullException(nameof(registrationChallengeStore));
            _relyingPartyInfoResolver = relyingPartyInfoResolver ?? throw new ArgumentNullException(nameof(relyingPartyInfoResolver));
        }

        /// <inheritdoc />
        public async Task<PublicKeyCredentialCreationOptionsBase64> InitiateRegistration(string userId, string deviceFriendlyName = null) {
            if (string.IsNullOrWhiteSpace(userId)) {
                throw new ArgumentNullException(nameof(userId), $"Parameter {nameof(userId)} cannot be null or empty during registration initiation process.");
            }
            /* https://www.w3.org/TR/webauthn/#dom-publickeycredentialcreationoptions-challenge */
            var challenge = GenerateRandomKey(32);
            /* 
             * https://www.w3.org/TR/webauthn/#dom-publickeycredentialuserentity-id 
             * https://www.w3.org/TR/webauthn/#sctn-user-handle-privacy 
             * https://www.w3.org/TR/webauthn/#user-handle 
             */
            var userHandle = GenerateRandomKey(64);
            var relyingParty = _relyingPartyInfoResolver.Resolve();
            var excludeCredentials = await _publicKeyCredentialsStore.GetUserCredentials(userId);
            var publicKeyCredentialCreationOptions = new PublicKeyCredentialCreationOptionsBase64(relyingParty, userHandle, challenge, userId, deviceFriendlyName, excludeCredentials);
            // We need to persist the challenge because we need to retrieve it when completing the registration.
            await _registrationChallengeStore.Persist(publicKeyCredentialCreationOptions);
            return publicKeyCredentialCreationOptions;
        }

        /// <inheritdoc />
        public Task<FidoRegistrationResult> CompleteRegistration(PublicKeyCredential response) {
            if (response == null) {
                throw new ArgumentNullException(nameof(response), $"Parameter {nameof(response)} cannot be null during registration completion process.");
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a random key as a <see cref="byte[]"/>.
        /// </summary>
        /// <param name="length">The desired key length.</param>
        public static byte[] GenerateRandomKey(int length) {
            var bytes = new byte[length];
            RandomNumberGenerator.GetBytes(bytes);
            return bytes;
        }
    }
}
