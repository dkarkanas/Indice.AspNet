using System;
using Microsoft.Extensions.DependencyInjection;

namespace Indice.AspNetCore.Fido
{
    /// <summary>
    /// A builder that helps register and configure FIDO2 services.
    /// </summary>
    public interface IFidoBuilder
    {
        /// <summary>
        /// Specifies the contract for a collection of service descriptors.
        /// </summary>
        IServiceCollection Services { get; }
    }

    /// <summary>
    /// A builder that helps register and configure FIDO2 services.
    /// </summary>
    public class FidoBuilder : IFidoBuilder
    {
        /// <summary>
        /// Creates a new instance of <see cref="FidoBuilder"/>.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        public FidoBuilder(IServiceCollection services) {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
}
