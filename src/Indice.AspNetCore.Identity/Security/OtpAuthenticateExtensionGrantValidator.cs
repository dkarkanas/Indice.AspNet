﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Indice.AspNetCore.Identity.Data.Models;
using Indice.Configuration;
using Indice.Security;
using Indice.Services;
using Microsoft.AspNetCore.Identity;

namespace Indice.AspNetCore.Identity
{
    /// <summary>
    /// An extension grant that receives a valid token, sends an OTP to the user that when verified issues a new token that is marked approproately.
    /// </summary>
    public sealed class OtpAuthenticateExtensionGrantValidator : IExtensionGrantValidator
    {
        private readonly ITokenValidator _tokenValidator;
        private readonly UserManager<User> _userManager;
        private readonly Rfc6238AuthenticationService _rfc6238AuthenticationService;
        private readonly IdentityMessageDescriber _identityMessageDescriber;
        private readonly ITotpService _totpService;

        /// <summary>
        /// Creates a new instance of <see cref="OtpAuthenticateExtensionGrantValidator"/>.
        /// </summary>
        /// <param name="validator">Validates an access token.</param>
        /// <param name="userManager">Provides the APIs for managing user in a persistence store.</param>
        /// <param name="totpOptions">Configuration used in <see cref="System.Security.Rfc6238AuthenticationService"/> service.</param>
        /// <param name="identityMessageDescriber">Provides an extensibility point for altering localizing used inside the package.</param>
        /// <param name="totpService">Used to generate, send and verify time based one time passwords.</param>
        public OtpAuthenticateExtensionGrantValidator(
            ITokenValidator validator,
            UserManager<User> userManager,
            TotpOptions totpOptions,
            IdentityMessageDescriber identityMessageDescriber,
            ITotpService totpService
        ) {
            _tokenValidator = validator ?? throw new ArgumentNullException(nameof(validator));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _identityMessageDescriber = identityMessageDescriber ?? throw new ArgumentNullException(nameof(identityMessageDescriber));
            _totpService = totpService ?? throw new ArgumentNullException(nameof(totpService));
            _rfc6238AuthenticationService = new Rfc6238AuthenticationService(totpOptions.Timestep, totpOptions.CodeLength);
        }

        /// <inheritdoc />
        public string GrantType => CustomGrantTypes.OtpAuthenticate;

        /// <inheritdoc />
        public async Task ValidateAsync(ExtensionGrantValidationContext context) {
            var rawRequest = context.Request.Raw;
            var accessToken = rawRequest.Get("token");
            /* 1. Check if an access token exists in the request. */
            if (string.IsNullOrWhiteSpace(accessToken)) {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Access token not present in the request.");
                return;
            }
            /* 2. Validate given access token. */
            var tokenValidationResult = await _tokenValidator.ValidateAccessTokenAsync(accessToken);
            if (tokenValidationResult.IsError) {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Access token is not valid.");
                return;
            }
            /* 3. Check if given access token contains a subject. */
            var subject = tokenValidationResult.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject).Value;
            if (string.IsNullOrWhiteSpace(subject)) {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Claim 'sub' was not found.");
                return;
            }
            /* 4. Query the store for a user with an id equal to the one found in the token. */
            var user = await _userManager.FindByIdAsync(subject);
            if (user == null || !user.PhoneNumberConfirmed || user.Blocked) {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }
            /* 5. Check if an OTP is provided in the request. */
            var purpose = $"{TotpConstants.TokenGenerationPurpose.SessionOtp}:{user.Id}";
            var otp = rawRequest.Get("otp");
            var principal = Principal.Create("OtpAuthenticatedUser", new List<Claim> { 
                new Claim(JwtClaimTypes.Subject, subject) 
            }
            .ToArray());
            /* 5.1 If an OTP is not provided, then we must send one to the user's confirmed phone number. */
            if (string.IsNullOrWhiteSpace(otp)) {
                /* 5.1.1 In order to send the OTP we have to decide the delivery channel. Delivery channel can optionally be sent in the request. */
                var providedChannel = rawRequest.Get("channel");
                var channel = TotpDeliveryChannel.Sms;
                if (!string.IsNullOrWhiteSpace(providedChannel)) {
                    if (Enum.TryParse<TotpDeliveryChannel>(providedChannel, ignoreCase: true, out var parsedChannel)) {
                        channel = parsedChannel;
                    } else {
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid delivery channel.");
                        return;
                    }
                }
                await _totpService.Send(builder => 
                    builder.UsePrincipal(principal)
                           .WithMessage(_identityMessageDescriber.OtpSecuredValidatorOtpBody())
                           .ToPhoneNumber(user.PhoneNumber)
                           .UsingDeliveryChannel(channel, _identityMessageDescriber.OtpSecuredValidatorOtpSubject)
                           .WithPurpose(purpose));
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "An OTP code was sent to the user. Please replay the request and include the verification code.", new Dictionary<string, object> {
                    { "otp_sent", true }
                });
                return;
            }
            /* 5.2 If an OTP is provided, then we must verify it at first. */
            var totpVerificationResult = await _totpService.Verify(principal, otp, purpose: purpose);
            /* If OTP verification code is not valid respond accordingly. */
            if (!totpVerificationResult.Success) {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "OTP verification code could not be validated.");
                return;
            }
            /* If OTP verification code is valid add the same claims that were present in the token and a new one to mark that OTP verification has been successfully completed. */
            var claims = tokenValidationResult.Claims.ToList();
            claims.Add(new Claim(BasicClaimTypes.OtpAuthenticated, "true", ClaimValueTypes.Boolean));
            context.Result = new GrantValidationResult(subject, GrantType, claims);
        }
    }
}
