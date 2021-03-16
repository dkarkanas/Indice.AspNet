using System;
using Indice.AspNetCore.Fido;
using Indice.AspNetCore.Fido.EntityFrameworkCore;
using Indice.AspNetCore.Fido.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains methods in order to register and configure FIDO2 services.
    /// </summary>
    public static class FidoConfiguration
    {
        /// <summary>
        /// Creates a new instance for <see cref="IFidoBuilder"/>.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        public static IFidoBuilder AddFidoBuilder(this IServiceCollection services) => new FidoBuilder(services);

        /// <summary>
        /// Adds FIDO2 services in your application.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="configureOptions">A setup <see cref="Action"/> used to configure FIDO2 services.</param>
        public static IFidoBuilder AddFido(this IServiceCollection services, Action<FidoOptions> configureOptions = null) {
            services.Configure(configureOptions);
            return services.AddFido();
        }

        /// <summary>
        /// Adds FIDO2 services in your application.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        public static IFidoBuilder AddFido(this IServiceCollection services, IConfiguration configuration) {
            services.Configure<FidoOptions>(configuration);
            return services.AddFido();
        }

        /// <summary>
        /// Adds FIDO2 services in your application.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        public static IFidoBuilder AddFido(this IServiceCollection services) {
            var fidoBuilder = services.AddFidoBuilder();
            fidoBuilder.AddCoreServices();
            fidoBuilder.AddRequiredPlatformServices();
            fidoBuilder.UseInMemoryPublicKeyCredentialsStore();
            fidoBuilder.UseCookieRegistrationChallengeStore();
            return fidoBuilder;
        }

        /// <summary>
        /// Registers core services for FIDO2.
        /// </summary>
        /// <param name="fidoBuilder">A builder that helps register and configure FIDO2 services.</param>
        public static IFidoBuilder AddCoreServices(this IFidoBuilder fidoBuilder) {
            fidoBuilder.Services.AddTransient<IFidoAuthenticationService, FidoAuthenticationService>();
            fidoBuilder.Services.AddTransient<IRelyingPartyResolver, RelyingPartyResolver>();
            return fidoBuilder;
        }

        /// <summary>
        /// Registers required platform services used in FIDO2 services.
        /// </summary>
        /// <param name="fidoBuilder">A builder that helps register and configure FIDO2 services.</param>
        public static IFidoBuilder AddRequiredPlatformServices(this IFidoBuilder fidoBuilder) {
            fidoBuilder.Services.AddHttpContextAccessor();
            fidoBuilder.Services.AddDataProtection();
            return fidoBuilder;
        }

        /// <summary>
        /// Registers a service in order to persist public key credentials in-memory. It should be used for testing or development purposes and not recommended for production scenarios.
        /// </summary>
        /// <param name="fidoBuilder">A builder that helps register and configure FIDO2 services.</param>
        public static IFidoBuilder UseInMemoryPublicKeyCredentialsStore(this IFidoBuilder fidoBuilder) => UsePublicKeyCredentialsStore<PublicKeyCredentialsStoreInMemory>(fidoBuilder);

        /// <summary>
        /// Registers a service in order to persist public key credentials a database using Entity Framework Core.
        /// </summary>
        /// <param name="fidoBuilder">A builder that helps register and configure FIDO2 services.</param>
        /// <param name="optionsAction">An action to configure the <see cref="DbContextOptions"/> for the context.</param>
        public static IFidoBuilder UseEntityFrameworkCorePublicKeyCredentialsStore(this IFidoBuilder fidoBuilder, Action<DbContextOptionsBuilder> optionsAction) {
            fidoBuilder.Services.AddDbContext<FidoDbContext>(optionsAction);
            return UsePublicKeyCredentialsStore<PublicKeyCredentialsStoreEntityFrameworkCore>(fidoBuilder);
        }

        /// <summary>
        /// Registers a service in order to persist public key credentials a database using Entity Framework Core.
        /// </summary>
        /// <param name="fidoBuilder">A builder that helps register and configure FIDO2 services.</param>
        /// <param name="optionsAction">An action to configure the <see cref="DbContextOptions"/> for the context.</param>
        public static IFidoBuilder UseEntityFrameworkCorePublicKeyCredentialsStore(this IFidoBuilder fidoBuilder, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction) {
            fidoBuilder.Services.AddDbContext<FidoDbContext>(optionsAction);
            return UsePublicKeyCredentialsStore<PublicKeyCredentialsStoreEntityFrameworkCore>(fidoBuilder);
        }

        /// <summary>
        /// Registers a service in order to persist public key credentials.
        /// </summary>
        /// <typeparam name="TStore">The CLR type of store to register as an implementation of <see cref="IPublicKeyCredentialsStore"/>.</typeparam>
        /// <param name="fidoBuilder">A builder that helps register and configure FIDO2 services.</param>
        public static IFidoBuilder UsePublicKeyCredentialsStore<TStore>(this IFidoBuilder fidoBuilder) where TStore : IPublicKeyCredentialsStore {
            fidoBuilder.Services.AddTransient(typeof(IPublicKeyCredentialsStore), typeof(TStore));
            return fidoBuilder;
        }

        /// <summary>
        /// Registers a service in order to persist the generated challenge during registration initiation in-memory. It should be used for testing or development purposes and not recommended for production scenarios.
        /// </summary>
        /// <param name="fidoBuilder">A builder that helps register and configure FIDO2 services.</param>
        public static IFidoBuilder UseInMemoryRegistrationChallengeStore(this IFidoBuilder fidoBuilder) => UseRegistrationChallengeStore<RegistrationChallengeStoreInMemory>(fidoBuilder);

        /// <summary>
        /// Registers a service in order to persist the generated challenge during registration initiation in a cookie.
        /// </summary>
        /// <param name="fidoBuilder">A builder that helps register and configure FIDO2 services.</param>
        public static IFidoBuilder UseCookieRegistrationChallengeStore(this IFidoBuilder fidoBuilder) => UseRegistrationChallengeStore<RegistrationChallengeStoreCookie>(fidoBuilder);

        /// <summary>
        /// Registers a service in order to persist the generated challenge during registration initiation.
        /// </summary>
        /// <typeparam name="TStore">The CLR type of store to register as an implementation of <see cref="IRegistrationChallengeStore"/>.</typeparam>
        /// <param name="fidoBuilder">A builder that helps register and configure FIDO2 services.</param>
        public static IFidoBuilder UseRegistrationChallengeStore<TStore>(this IFidoBuilder fidoBuilder) where TStore : IRegistrationChallengeStore {
            fidoBuilder.Services.AddTransient(typeof(IRegistrationChallengeStore), typeof(TStore));
            return fidoBuilder;
        }
    }
}
