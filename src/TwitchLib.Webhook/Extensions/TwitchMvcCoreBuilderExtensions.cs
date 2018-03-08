using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Webhook.Internal;

namespace TwitchLib.Webhook.Extensions
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
