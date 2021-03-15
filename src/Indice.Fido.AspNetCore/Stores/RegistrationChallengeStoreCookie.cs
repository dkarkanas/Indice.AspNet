using System;
using System.Text.Json;
using System.Threading.Tasks;
using Indice.AspNetCore.Fido.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace Indice.AspNetCore.Fido.Stores
{
    /// <summary>
    /// 
    /// </summary>
    public class RegistrationChallengeStoreCookie : IRegistrationChallengeStore
    {
        public const string DEFAULT_COOKIE_NAME = "fido.challenge";
        private readonly FidoOptions _fidoOptions;
        private readonly HttpContext _httpContext;
        private readonly IDataProtector _dataProtector;
        private readonly CookieBuilder DefaultCookieBuilder = new() {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            SecurePolicy = CookieSecurePolicy.Always,
            Expiration = TimeSpan.FromMinutes(5),
            Name = DEFAULT_COOKIE_NAME
        };

        /// <summary>
        /// Creates a new instance of <see cref="RegistrationChallengeStoreCookie"/>.
        /// </summary>
        /// <param name="dataProtectionProvider">An interface that can be used to create <see cref="IDataProtector"/> instances.</param>
        /// <param name="httpContextAccessor">Provides access to the current <see cref="HttpContext"/>.</param>
        /// <param name="fidoOptions">Options used when configuring FIDO2 services</param>
        public RegistrationChallengeStoreCookie(
            IDataProtectionProvider dataProtectionProvider,
            IHttpContextAccessor httpContextAccessor,
            IOptions<FidoOptions> fidoOptions
        ) {
            _dataProtector = dataProtectionProvider.CreateProtector(typeof(RegistrationChallengeStoreCookie).Name);
            _fidoOptions = fidoOptions?.Value ?? throw new ArgumentNullException(nameof(fidoOptions));
            _httpContext = httpContextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <inheritdoc />
        public Task<PublicKeyCredentialCreationOptionsBase64> GetByKey(string key) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task Persist(string key, PublicKeyCredentialCreationOptionsBase64 publicKeyCredentialCreationOptions) {
            if (string.IsNullOrWhiteSpace(key)) {
                throw new ArgumentNullException(nameof(key), $"Parameter {nameof(key)} cannot be null or empty.");
            }
            if (publicKeyCredentialCreationOptions == null) {
                throw new ArgumentNullException(nameof(publicKeyCredentialCreationOptions), $"Parameter {nameof(publicKeyCredentialCreationOptions)} cannot be null.");
            }
            var serializedCredential = JsonSerializer.Serialize(publicKeyCredentialCreationOptions);
            var protectedCredential = _dataProtector.Protect(serializedCredential);
            var cookieOptions = (_fidoOptions.ChallengeCookie ?? DefaultCookieBuilder).Build(_httpContext);
            _httpContext.Response.Cookies.Append(key, protectedCredential, cookieOptions);
            return Task.CompletedTask;
        }
    }
}
