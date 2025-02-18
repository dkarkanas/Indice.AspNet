﻿using System.Reflection;
using Indice.AspNetCore.Features.Campaigns;
using Indice.AspNetCore.Swagger;
using Indice.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services, GeneralSettings generalSettings) {
            services.AddSwaggerGen(options => {
                options.IndiceDefaults(generalSettings);
                options.AddFluentValidationSupport();
                options.AddOAuth2AuthorizationCodeFlow(generalSettings);
                options.AddFormFileSupport();
                options.IncludeXmlComments(Assembly.Load(CampaignsApi.AssemblyName));
                options.AddDoc(CampaignsApi.Scope, "Campaigns API", "API for managing campaigns in the backoffice tool.");
                options.AddDoc("lookups", "Lookups API", "API for various lookups.");
            });
            return services;
        }
    }
}
