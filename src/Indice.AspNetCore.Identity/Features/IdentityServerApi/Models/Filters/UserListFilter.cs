﻿using System.Collections.Generic;

namespace Indice.AspNetCore.Identity.Api.Models
{
    /// <summary>
    /// Contains filter when querying for users list.
    /// </summary>
    public class UserListFilter
    {
        /// <summary>
        /// The claim type and value to search for.
        /// </summary>
        public BasicClaimInfo Claim { get; set; }
        /// <summary>
        /// A list of user identifiers to search for.
        /// </summary>
        public List<string> UserId { get; set; } = new List<string>();
    }
}
