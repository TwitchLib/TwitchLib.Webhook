using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Treyza.AspNetCore.WebHooks.Receivers.Twitch.Internal;

namespace Treyza.AspNetCore.WebHooks.Receivers.Twitch.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TwitchMvcBuilderExtensions
    {
        public static IMvcBuilder AddTwitchWebHooks(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            TwitchServiceCollectionSetup.AddTwitchServices(builder.Services);

            return builder.AddWebHooks();
        }
    }
}
