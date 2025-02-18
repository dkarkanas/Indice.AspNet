﻿using System;

namespace Indice.Services
{
    /// <summary>
    /// Models the data that are sent in an push notification message.
    /// </summary>
    public class PushNotificationMessage
    {
        /// <summary>
        /// The payload data that will be sent to the mobile client (not visible to the push notification Title or Message).
        /// If the data is null then only the token will be sent as data.
        /// </summary>
        public string Data { get; }
        /// <summary>
        /// The title of the push notification.
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// The title of the push notification.
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// The user identifier that correlates devices with users. This can be any identifier like user id, username, user email, customer code etc.
        /// </summary>
        public string UserTag { get; }
        /// <summary>
        /// The tags of the push notification.
        /// </summary>
        public string[] Tags { get; }
        /// <summary>
        /// The type of the push notification.
        /// </summary>
        public string Classification { get; }
        /// <summary>
        /// The OTP token that must be passed to the client.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Constructs a <see cref="PushNotificationMessage"/>.
        /// </summary>
        /// <param name="title">The title of the push notification.</param>
        /// <param name="body">The body of the push notification.</param>
        /// <param name="token">The token.</param>
        /// <param name="data">The payload data that will be sent to the mobile client (not visible to the push notification Title or Message).  If the data is null then only the token will be sent as data.</param>
        /// <param name="userTag">The UserId to send the push notification.</param>
        /// <param name="tags">The tags of the push notification.</param>
        /// <param name="classification">The type of the push notification.</param>
        public PushNotificationMessage(string title, string body, string token, string data, string userTag, string[] tags, string classification) {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Body = body ?? throw new ArgumentNullException(nameof(body));
            Token = token;
            Data = data;
            UserTag = userTag ?? throw new ArgumentNullException(nameof(userTag));
            Tags = tags ?? Array.Empty<string>();
            Classification = classification;
        }
    }
}