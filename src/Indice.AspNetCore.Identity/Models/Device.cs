﻿using System;
using Indice.Types;

namespace Indice.AspNetCore.Identity.Models
{
    /// <summary>
    /// Models a user device.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// The device id to register for push notifications.
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// Device name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Client device platform.
        /// </summary>
        public DevicePlatform Platform { get; set; }
        /// <summary>
        /// Flag that determines if push notifications are enabled for this device.
        /// </summary>
        public bool IsPushNotificationsEnabled { get; set; }
        /// <summary>
        /// The date this device was created.
        /// </summary>
        public DateTimeOffset? DateCreated { get; set; }
        /// <summary>
        /// Device model.
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// Device OS version.
        /// </summary>
        public string OsVersion { get; set; }
        /// <summary>
        /// Gets or sets the date and time, in UTC, when the device last signed in.
        /// </summary>
        public DateTimeOffset? LastSignInDate { get; set; }
        /// <summary>
        /// Extra metadata for the device.
        /// </summary>
        public dynamic Data { get; set; }
    }
}
