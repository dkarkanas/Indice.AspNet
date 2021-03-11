using System;
using System.Threading.Tasks;
using IdentityModel;

namespace Indice.AspNetCore.Fido
{
    /// <summary>A service that contains WebAuthn Relying Party operations, regarding registration or authentication ceremony.</summary>
    public class FidoAuthenticationService : IFidoAuthenticationService
    {
        private readonly IRelyingPartyResolver _relyingPartyInfoResolver;

        /// <summary>Creates a new instance of <see cref="FidoAuthenticationService"/>.</summary>
        /// <param name="relyingPartyInfoResolver">A service that helps discover information about the Relying Party (e.x. Identity Server).</param>
        public FidoAuthenticationService(IRelyingPartyResolver relyingPartyInfoResolver) {
            _relyingPartyInfoResolver = relyingPartyInfoResolver ?? throw new ArgumentNullException(nameof(relyingPartyInfoResolver));
        }

        /// <inheritdoc />
        public Task<PublicKeyCredentialCreationOptions> InitiateRegistration(string userId, string deviceFriendlyName = null) {
            if (string.IsNullOrWhiteSpace(userId)) {
                throw new ArgumentNullException(nameof(userId), $"Parameter {nameof(userId)} cannot be null or empty during registration initiation process.");
            }
            /* https://www.w3.org/TR/webauthn/#sctn-cryptographic-challenges */
            var challenge = CryptoRandom.CreateRandomKey(32);
            var relyingParty = _relyingPartyInfoResolver.Resolve();
            return Task.FromResult(new PublicKeyCredentialCreationOptions {
                RelyingParty = relyingParty
            });
        }
    }
}
