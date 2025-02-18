﻿using Indice.AspNetCore.EmbeddedUI;

namespace Indice.AspNetCore.Features.Campaigns.UI
{
    /// <summary>
    /// Options for configuring <see cref="SpaUIMiddleware{TOptions}"/> middleware.
    /// </summary>
    public class CampaignUIOptions : SpaUIOptions
    {
        /// <summary>
        /// Creates a new instance <see cref="CampaignUIOptions"/>.
        /// </summary>
        public CampaignUIOptions() {
            ConfigureIndexParameters = (args) => { }; 
        }
    }
}
