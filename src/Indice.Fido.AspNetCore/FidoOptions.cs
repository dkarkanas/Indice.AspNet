using System;
using Indice.AspNetCore.Fido.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Indice.AspNetCore.Fido
{
    /// <summary>
    /// Options used when configuring FIDO2 services using <see cref="FidoConfiguration.AddFido(IServiceCollection, System.Action{FidoOptions})"/> extension method.
    /// </summary>
    public class FidoOptions
    {
        private string _relyingPartyId;

        /// <summary>
        /// A valid domain string identifying the WebAuthn Relying Party. If not set it is calculated based on the origin's effective domain.
        /// </summary>
        /// <example>
        /// For example, given a Relying Party whose origin is https://login.example.com:1337, then the following RP IDs are valid: login.example.com (default) and example.com, but not m.login.example.com and not com.
        /// </example>
        public string RelyingPartyId {
            get => _relyingPartyId;
            set => _relyingPartyId = Uri.IsWellFormedUriString(value, UriKind.Relative) ? value : throw new ArgumentException("Relying Party Id must be a valid domain string.", nameof(RelyingPartyId));
        }
        /// <summary>
        /// The name of the Relying Party. If not set it defaults to 'Indice FIDO2 Server'.
        /// </summary>
        public string RelyingPartyName { get; set; } = RelyingPartyEntity.DEFAULT_NAME;
        /// <summary>
        /// Configures the cookie used when registration process is initiated.
        /// </summary>
        public CookieBuilder ChallengeCookie { get; set; }
    }
}
