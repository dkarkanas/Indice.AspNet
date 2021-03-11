using System;
using Indice.AspNetCore.Fido;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IFidoBuilder AddFido(this IServiceCollection services, Action<FidoOptions> configureOptions = null) {
            var fidoOptions = new FidoOptions { Services = services };
            configureOptions?.Invoke(fidoOptions);
            fidoOptions.Services = null;
            services.AddSingleton(fidoOptions);
            services.AddScoped<IFidoAuthenticationService, FidoAuthenticationService>();
            services.AddScoped<IRelyingPartyResolver, RelyingPartyResolver>();
            return new FidoBuilder(services);
        }
    }
}
