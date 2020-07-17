﻿using System.Collections.Generic;

namespace Indice.AspNetCore.Identity.Features
{
    /// <summary>
    /// Models access to an API resource.
    /// </summary>
    public class ScopeInfo
    {
        /// <summary>
        /// Unique identifier for the scope.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the scope.
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// The display name of the scope.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// The description of the resource.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Determines whether this scope is required or not.
        /// </summary>
        public bool? NonEditable { get; set; }
        /// <summary>
        /// Determines whether this scope should be displayed emphasized or not.
        /// </summary>
        public bool? Emphasize { get; set; }
        /// <summary>
        /// Determines whether this scope should be displayed in the discovery document or not.
        /// </summary>
        public bool? ShowInDiscoveryDocument { get; set; }
        /// <summary>
        /// List of allowed signing algorithms for access token. If empty, will use the server default signing algorithm.
        /// </summary>
        public string AllowedAccessTokenSigningAlgorithms { get; set; }
        /// <summary>
        /// List of accociated user claims that should be included when a resource is requested.
        /// </summary>
        public IEnumerable<string> UserClaims { get; set; }
    }
}
