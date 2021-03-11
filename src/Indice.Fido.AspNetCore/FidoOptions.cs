using Microsoft.Extensions.DependencyInjection;

namespace Indice.AspNetCore.Fido
{
    public class FidoOptions
    {
        internal IServiceCollection Services;
        public string RelyingPartyId { get; set; }
        public string RelyingPartyName { get; set; } = Constants.DefaultRelyingPartyName;
    }
}
