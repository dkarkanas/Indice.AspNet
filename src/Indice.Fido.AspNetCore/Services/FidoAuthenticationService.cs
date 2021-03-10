using System;
using System.Threading.Tasks;
using IdentityModel;

namespace Indice.AspNetCore.Fido
{
    public class FidoAuthenticationService : IFidoAuthenticationService
    {
        private readonly IRelyingPartyIdResolver _relyingPartyIdResolver;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relyingPartyIdResolver"></param>
        public FidoAuthenticationService(IRelyingPartyIdResolver relyingPartyIdResolver) {
            _relyingPartyIdResolver = relyingPartyIdResolver ?? throw new ArgumentNullException(nameof(relyingPartyIdResolver));
        }

        /// <inheritdoc />
        public Task<InitiateRegistrationChallenge> InitiateRegistration(string userId, string deviceFriendlyName = null) {
            if (string.IsNullOrWhiteSpace(userId)) {
                throw new ArgumentNullException(nameof(userId), $"Paramater {nameof(userId)} cannot be null or empty during registration initiation process.");
            }
            /* https://www.w3.org/TR/webauthn/#sctn-cryptographic-challenges */
            var challenge = CryptoRandom.CreateRandomKey(64);
            var relyingPartyId = _relyingPartyIdResolver.Resolve();
            return null;
        }
    }
}
