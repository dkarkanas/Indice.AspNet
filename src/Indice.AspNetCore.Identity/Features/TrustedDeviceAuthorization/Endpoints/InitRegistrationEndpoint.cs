﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using IdentityServer4.Models;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Indice.AspNetCore.Extensions;
using Indice.AspNetCore.Identity.TrustedDeviceAuthorization.Configuration;
using Indice.AspNetCore.Identity.TrustedDeviceAuthorization.Endpoints.Results;
using Indice.AspNetCore.Identity.TrustedDeviceAuthorization.ResponseHandling;
using Indice.AspNetCore.Identity.TrustedDeviceAuthorization.Stores;
using Indice.AspNetCore.Identity.TrustedDeviceAuthorization.Validation;
using Indice.Security;
using Indice.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Indice.AspNetCore.Identity.TrustedDeviceAuthorization.Endpoints
{
    internal class InitRegistrationEndpoint : IEndpointHandler
    {
        public InitRegistrationEndpoint(BearerTokenUsageValidator tokenUsageValidator, ILogger<InitRegistrationEndpoint> logger, InitRegistrationRequestValidator requestValidator,
            InitRegistrationResponseGenerator responseGenerator, IProfileService profileService, IResourceStore resourceStore, ITotpService totpService, IUserDeviceStore userDeviceStore,
            IdentityMessageDescriber identityMessageDescriber
        ) {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ProfileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            Request = requestValidator ?? throw new ArgumentNullException(nameof(requestValidator));
            ResourceStore = resourceStore;
            Response = responseGenerator ?? throw new ArgumentNullException(nameof(responseGenerator));
            Token = tokenUsageValidator ?? throw new ArgumentNullException(nameof(tokenUsageValidator));
            TotpService = totpService ?? throw new ArgumentNullException(nameof(totpService));
            UserDeviceStore = userDeviceStore ?? throw new ArgumentNullException(nameof(userDeviceStore));
            IdentityMessageDescriber = identityMessageDescriber ?? throw new ArgumentNullException(nameof(identityMessageDescriber));
        }

        public BearerTokenUsageValidator Token { get; }
        public ILogger<InitRegistrationEndpoint> Logger { get; }
        public InitRegistrationRequestValidator Request { get; }
        public InitRegistrationResponseGenerator Response { get; }
        public IProfileService ProfileService { get; }
        public IResourceStore ResourceStore { get; }
        public ITotpService TotpService { get; }
        public IUserDeviceStore UserDeviceStore { get; }
        public IdentityMessageDescriber IdentityMessageDescriber { get; }

        public async Task<IEndpointResult> ProcessAsync(HttpContext httpContext) {
            Logger.LogInformation($"[{nameof(InitRegistrationEndpoint)}] Started processing trusted device registration initiation endpoint.");
            var isPostRequest = HttpMethods.IsPost(httpContext.Request.Method);
            var isApplicationFormContentType = httpContext.Request.HasApplicationFormContentType();
            // Validate HTTP request type and method.
            if (!isPostRequest || !isApplicationFormContentType) {
                return Error(OidcConstants.TokenErrors.InvalidRequest, "Request must be of type 'POST' and have a Content-Type equal to 'application/x-www-form-urlencoded'.");
            }
            // Ensure that a valid 'Authorization' header exists.
            var tokenUsageResult = await Token.Validate(httpContext);
            if (!tokenUsageResult.TokenFound) {
                return Error(OidcConstants.ProtectedResourceErrors.InvalidToken, "No access token is present in the request.");
            }
            // Validate request data and access token.
            var parameters = (await httpContext.Request.ReadFormAsync()).AsNameValueCollection();
            var requestValidationResult = await Request.Validate(parameters, tokenUsageResult.Token);
            if (requestValidationResult.IsError) {
                return Error(requestValidationResult.Error, requestValidationResult.ErrorDescription);
            }
            // Ensure device is not already registered or belongs to any other user.
            var existingDevice = await UserDeviceStore.GetByDeviceId(requestValidationResult.DeviceId);
            var isNewDeviceOrOwnedByUser = existingDevice == null || existingDevice.UserId.Equals(requestValidationResult.UserId, StringComparison.OrdinalIgnoreCase);
            if (!isNewDeviceOrOwnedByUser) {
                return Error(OidcConstants.ProtectedResourceErrors.InvalidToken, "Device does not belong to the this user.");
            }
            // Ensure that the principal has declared a phone number which is also confirmed.
            // We will get these 2 claims by retrieving the identity resources from the store (using the requested scopes existing in the access token) and then calling the profile service.
            // This will help us make sure that the 'phone' scope was requested and finally allowed in the token endpoint.
            var identityResources = await ResourceStore.FindEnabledIdentityResourcesByScopeAsync(requestValidationResult.RequestedScopes);
            var resources = new Resources(identityResources, Enumerable.Empty<ApiResource>(), Enumerable.Empty<ApiScope>());
            var validatedResources = new ResourceValidationResult(resources);
            if (!validatedResources.Succeeded) {
                return Error(OidcConstants.ProtectedResourceErrors.InvalidToken, "Identity resources could be validated.");
            }
            var requestedClaimTypes = resources.IdentityResources.SelectMany(x => x.UserClaims).Distinct();
            var profileDataRequestContext = new ProfileDataRequestContext(requestValidationResult.Principal, requestValidationResult.Client, IdentityServerConstants.ProfileDataCallers.UserInfoEndpoint, requestedClaimTypes) {
                RequestedResources = validatedResources
            };
            await ProfileService.GetProfileDataAsync(profileDataRequestContext);
            var profileClaims = profileDataRequestContext.IssuedClaims;
            var phoneNumberClaim = profileClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.PhoneNumber);
            var phoneNumberVerifiedClaim = profileClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.PhoneNumberVerified);
            if (string.IsNullOrWhiteSpace(phoneNumberClaim?.Value) || phoneNumberVerifiedClaim == null || (bool.TryParse(phoneNumberVerifiedClaim.Value, out var phoneNumberVerified) && !phoneNumberVerified)) {
                return Error(OidcConstants.ProtectedResourceErrors.InvalidToken, "User does not have a phone number or the phone number is not verified.");
            }
            var otpAuthenticatedValue = profileClaims.FirstOrDefault(x => x.Type == BasicClaimTypes.OtpAuthenticated)?.Value;
            var otpAuthenticated = !string.IsNullOrWhiteSpace(otpAuthenticatedValue) && bool.Parse(otpAuthenticatedValue);
            if (!otpAuthenticated) {
                // Send OTP code.
                void messageBuilder(TotpMessageBuilder message) {
                    var builder = message.UsePrincipal(requestValidationResult.Principal).WithMessage(IdentityMessageDescriber.DeviceRegistrationCodeMessage(existingDevice?.Name, requestValidationResult.InteractionMode));
                    if (requestValidationResult.DeliveryChannel == TotpDeliveryChannel.Sms) {
                        builder.UsingSms();
                    } else {
                        builder.UsingViber();
                    }
                    builder.WithPurpose(Constants.TrustedDeviceOtpPurpose(requestValidationResult.UserId, requestValidationResult.DeviceId));
                }
                var totpResult = await TotpService.Send(messageBuilder);
                if (!totpResult.Success) {
                    return Error(totpResult.Error);
                }
            }
            // Create endpoint response.
            var response = await Response.Generate(requestValidationResult);
            Logger.LogInformation($"[{nameof(InitRegistrationEndpoint)}] Trusted device registration initiation endpoint success.");
            return new InitRegistrationResult(response);
        }

        private AuthorizationErrorResult Error(string error, string errorDescription = null, Dictionary<string, object> custom = null) {
            var response = new TokenErrorResponse {
                Error = error,
                ErrorDescription = errorDescription,
                Custom = custom
            };
            Logger.LogError("[{EndpointName}] Trusted device registration initiation endpoint error: {Error}:{ErrorDescription}", nameof(InitRegistrationEndpoint), error, errorDescription ?? " -no message-");
            return new AuthorizationErrorResult(response);
        }
    }
}
