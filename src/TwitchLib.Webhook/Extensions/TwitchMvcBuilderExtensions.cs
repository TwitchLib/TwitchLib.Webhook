using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Webhook.Internal;

namespace TwitchLib.Webhook.Extensions
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
