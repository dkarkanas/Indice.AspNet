using Microsoft.Extensions.DependencyInjection;

namespace Indice.AspNetCore.Fido
{
    /// <summary>
    /// 
    /// </summary>
    public class FidoOptions
    {
        internal IServiceCollection Services;
        public string RelyingPartyId { get; set; }
        public string RelyingPartyName { get; set; } = RelyingPartyEntity.DEFAULT_NAME;
    }
}
