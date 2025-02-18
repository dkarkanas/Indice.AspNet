﻿using System;
using Indice.AspNetCore.Identity.Api.Models;
using Indice.Services;

namespace Indice.AspNetCore.Identity.Api.Events
{
    /// <summary>
    /// An event that is raised when a new user is registered using IdentityServer API.
    /// </summary>
    public class UserRegisteredEvent : IPlatformEvent
    {
        /// <summary>
        /// Creates a new instance of <see cref="UserRegisteredEvent"/>.
        /// </summary>
        /// <param name="user">The instance of the user that was registered.</param>
        /// <param name="confirmationToken">The generated email confirmation token.</param>
        public UserRegisteredEvent(SingleUserInfo user, string confirmationToken) {
            User = user ?? throw new ArgumentNullException(nameof(user));
            ConfirmationToken = confirmationToken ?? throw new ArgumentNullException(nameof(confirmationToken));
        }

        /// <summary>
        /// The instance of the user that was registered.
        /// </summary>
        public SingleUserInfo User { get; }
        /// <summary>
        /// The generated email confirmation token.
        /// </summary>
        public string ConfirmationToken { get; }
    }
}
