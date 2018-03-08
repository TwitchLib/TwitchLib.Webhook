using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Treyza.AspNetCore.WebHooks.Receivers.Twitch.Internal;


namespace Treyza.AspNetCore.WebHooks.Receivers.Twitch.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TwitchMvcCoreBuilderExtensions
    {
        public static IMvcCoreBuilder AddTwitchWebHooks(this IMvcCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            TwitchServiceCollectionSetup.AddTwitchServices(builder.Services);

            return builder
                .AddJsonFormatters()
                .AddWebHooks();
        }
    }
}
