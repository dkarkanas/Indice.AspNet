﻿using System;
using FluentValidation.AspNetCore;
using Indice.AspNetCore.Identity.Api;
using Indice.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Adds feature extensions to the <see cref="IMvcBuilder"/>.
    /// </summary>
    public static class PushNotificationsFeatureExtensions
    {
        /// <summary>
        /// Adds all required services in order for Push notifications to work.
        /// </summary>
        /// <param name="builder">An interface for configuring MVC services.</param>
        /// <param name="configure">Configuration used in the implementation of <see cref="IPushNotificationService"/> service.</param>
        public static IMvcBuilder AddPushNotifications(this IMvcBuilder builder, Action<IServiceProvider, PushNotificationAzureOptions> configure = null) {
            var options = new PushNotificationAzureOptions();
            configure?.Invoke(builder.Services.BuildServiceProvider(), options);
            builder.Services.AddPushNotificationServiceAzure(configure);
            builder.Services.TryAddTransient<IPlatformEventService, PlatformEventService>();
            builder.ConfigureApplicationPartManager(x => x.FeatureProviders.Add(new PushNotificationsFeatureProvider()));
            builder.AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<PushNotificationsFeatureProvider>());
            return builder;
        }
    }
}
