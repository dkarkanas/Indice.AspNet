using System;
using Indice.AspNetCore.Fido.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Indice.AspNetCore.Fido
{
    /// <summary>
    /// A service that helps discover information about the Relying Party.
    /// </summary>
    public class RelyingPartyResolver : IRelyingPartyResolver
    {
        private readonly FidoOptions _fidoOptions;
        private readonly HttpContext _httpContext;

        /// <summary>
        /// Creates a new instance of <see cref="RelyingPartyResolver"/>.
        /// </summary>
        /// <param name="httpContextAccessor">Provides access to the current <see cref="HttpContext"/>.</param>
        /// <param name="fidoOptions">Options used when configuring FIDO2 services</param>
        public RelyingPartyResolver(
            IHttpContextAccessor httpContextAccessor,
            IOptions<FidoOptions> fidoOptions
        ) {
            _fidoOptions = fidoOptions?.Value ?? throw new ArgumentNullException(nameof(fidoOptions));
            _httpContext = httpContextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <inheritdoc />
        public RelyingPartyEntity Resolve() {
            string relyingPartyId;
            var relyingPartyName = _fidoOptions.RelyingPartyName;
            if (!string.IsNullOrWhiteSpace(_fidoOptions.RelyingPartyId)) {
                relyingPartyId = _fidoOptions.RelyingPartyId;
            } else {
                var host = _httpContext.Request.Host;
                relyingPartyId = host.Host;
            }
            if (string.IsNullOrWhiteSpace(relyingPartyName)) {
                relyingPartyName = RelyingPartyEntity.DEFAULT_NAME;
            }
            return new RelyingPartyEntity {
                Id = relyingPartyId,
                Name = relyingPartyName
            };
        }
    }
}
