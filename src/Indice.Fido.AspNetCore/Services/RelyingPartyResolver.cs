using System;
using Microsoft.AspNetCore.Http;

namespace Indice.AspNetCore.Fido
{
    /// <inheritdoc />
    public class RelyingPartyResolver : IRelyingPartyResolver
    {
        private readonly FidoOptions _fidoOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RelyingPartyResolver(
            FidoOptions fidoOptions,
            IHttpContextAccessor httpContextAccessor
        ) {
            _fidoOptions = fidoOptions ?? throw new ArgumentNullException(nameof(fidoOptions));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public RelyingPartyEntity Resolve() {
            string relyingPartyId;
            var relyingPartyName = _fidoOptions.RelyingPartyName;
            if (!string.IsNullOrWhiteSpace(_fidoOptions.RelyingPartyId)) {
                relyingPartyId = new Uri(_fidoOptions.RelyingPartyId).Host;
            } else {
                var host = _httpContextAccessor.HttpContext.Request.Host;
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
