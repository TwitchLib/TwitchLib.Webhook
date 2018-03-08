using Microsoft.AspNetCore.WebHooks;

namespace TwitchLib.Webhook
{
    public class TwitchWebHookAttribute : WebHookAttribute
    {
        public TwitchWebHookAttribute() 
            : base(TwitchConstants.ReceiverName)
        {
        }
    }
}
