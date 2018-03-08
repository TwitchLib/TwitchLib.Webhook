using System.Collections.Generic;
using Newtonsoft.Json;

namespace Treyza.AspNetCore.WebHooks.Receivers.Twitch.Models
{
    public class StreamData
    {

        [JsonProperty("data")]
        public IList<Stream> Data { get; set; }
    }
}
