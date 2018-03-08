using Newtonsoft.Json;

namespace Treyza.AspNetCore.WebHooks.Receivers.Twitch.Models
{
    public class FollowerData
    {
        [JsonProperty("from_id")]
        public string FromId { get; set; }

        [JsonProperty("to_id")]
        public string ToId { get; set; }
    }
}
