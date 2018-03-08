using Microsoft.AspNetCore.WebHooks.Metadata;

namespace Treyza.AspNetCore.WebHooks.Receivers.Twitch.Metadata
{
    public class TwitchMetadata : 
        WebHookMetadata,
        IWebHookBodyTypeMetadataService,
        IWebHookGetHeadRequestMetadata

    {
        public TwitchMetadata() : base(TwitchConstants.ReceiverName)
        {
        }

        public WebHookBodyType BodyType => WebHookBodyType.Json;

        public bool AllowHeadRequests => false;
        public string ChallengeQueryParameterName => TwitchConstants.ChallengeQueryParameterName;
        public int SecretKeyMinLength => 8;
        public int SecretKeyMaxLength => 128;
    }
}
