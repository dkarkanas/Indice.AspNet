using System;
using Microsoft.AspNetCore.Http;

namespace Indice.AspNetCore.Fido
{
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
            if (!string.IsNullOrWhiteSpace(_fidoOptions.RelyingPartyId)) {
                relyingPartyId = new Uri(_fidoOptions.RelyingPartyId).Host;
            } else {
                var host = _httpContextAccessor.HttpContext.Request.Host;
                relyingPartyId = host.Host;
            }
            if (string.IsNullOrWhiteSpace(_fidoOptions.RelyingPartyName)) {
            }
            return new RelyingPartyEntity {
                Id = relyingPartyId,
                Name = string.Empty
            };
        }
    }
}