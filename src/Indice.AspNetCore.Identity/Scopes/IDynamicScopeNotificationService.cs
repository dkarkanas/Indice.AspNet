﻿using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Validation;

namespace Indice.AspNetCore.Identity.Scopes
{
    /// <summary>
    /// Contains methods to notify an API when a dynamic consent is altered.
    /// </summary>
    public interface IDynamicScopeNotificationService
    {
        /// <summary>
        /// Abstracts the way that an API is notified.
        /// </summary>
        /// <param name="clientId">The id of the client.</param>
        /// <param name="parsedScopes">The scopes that were altered.</param>
        /// <param name="notificationType">Describes the way that a dynamic consent was altered.</param>
        Task Notify(string clientId, IEnumerable<ParsedScopeValue> parsedScopes, DynamicScopeNotificationType notificationType);
    }

    /// <summary>
    /// Describes the way that a dynamic consent was altered.
    /// </summary>
    public enum DynamicScopeNotificationType
    {
        /// <summary>
        /// Grants revoked.
        /// </summary>
        GrantsRevoked,
        /// <summary>
        /// Consent granted.
        /// </summary>
        ConsentGranted
    }
}
