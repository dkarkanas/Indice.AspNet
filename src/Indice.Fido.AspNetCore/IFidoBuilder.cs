using System;
using Microsoft.Extensions.DependencyInjection;

namespace Indice.AspNetCore.Fido
{
    public interface IFidoBuilder
    {
        IServiceCollection Services { get; set; }
    }

    public class FidoBuilder : IFidoBuilder
    {
        public FidoBuilder(IServiceCollection services) {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; set; }
    }
}
