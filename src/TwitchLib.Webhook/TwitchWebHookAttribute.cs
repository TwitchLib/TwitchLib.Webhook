using Microsoft.AspNetCore.WebHooks;

namespace Treyza.AspNetCore.WebHooks.Receivers.Twitch
{
    public class TwitchWebHookAttribute : WebHookAttribute
    {
        public TwitchWebHookAttribute() 
            : base(TwitchConstants.ReceiverName)
        {
        }
    }
}
