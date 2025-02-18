﻿namespace Indice.AspNetCore.Features.Campaigns.Models
{
    /// <summary>
    /// Models a campaign.
    /// </summary>
    public class CampaignDetails : Campaign
    {
        /// <summary>
        /// The attachment details of the campaign.
        /// </summary>
        public AttachmentLink Attachment { get; set; }
    }
}
