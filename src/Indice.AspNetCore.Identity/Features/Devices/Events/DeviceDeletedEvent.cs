﻿using Indice.AspNetCore.Identity.Api.Models;
using Indice.Services;

namespace Indice.AspNetCore.Identity.Api.Events
{
    /// <summary>
    /// An event that is raised when a device is deleted on IdentityServer.
    /// </summary>
    public class DeviceDeletedEvent : IPlatformEvent
    {
        /// <summary>
        /// Creates a new instance of <see cref="DeviceCreatedEvent"/>.
        /// </summary>
        /// <param name="device">The device deleted.</param>
        /// <param name="user">Related user.</param>
        public DeviceDeletedEvent(DeviceInfo device, SingleUserInfo user) {
            Device = device;
            User = user;
        }

        /// <summary>
        /// The device deleted.
        /// </summary>
        public DeviceInfo Device { get; }
        /// <summary>
        /// Related user.
        /// </summary>
        public SingleUserInfo User { get; }
    }
}
