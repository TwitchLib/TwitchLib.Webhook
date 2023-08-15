# Deprecated! Please use https://github.com/TwitchLib/TwitchLib.EventSub.Webhooks instead

# TwitchLib.Webhook

## \*\* **This Library Requires dotnet core 2.1 preview 1 installed** \*\*

This library implements ASP.NET Core Webhook Receiver (https://github.com/aspnet/WebHooks).

It's meant to make creating a Webhook with ASP.NET Core Web API trivial.

In your Startup file just add `.AddTwitchWebHooks();` to your `ConfigureServices(...)`

```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddTwitchWebHooks();
        }
```

Then in your Controller you can create a `[TwitchWebHook]` attribute, with an optional Id parameter.

If you specify the Id parameter value, that will be part of your `hub.callback`

## New Followers

```csharp
        [TwitchWebHook(Id="followers")]
        public IActionResult Twitch([FromBody]Follower data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Handle Follower object here
            _logger.LogInformation($"New Follower: {data.Data.FromId}");

            return Ok();
        }
```

## Stream Online/Offline

```csharp
        [TwitchWebHook(Id="stream")]
        public IActionResult Stream([FromBody]StreamData streamData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Handle StreamData object here


            return Ok();
        }
```

The base URL is `http://hostname/api/webhooks/incoming/twitch`

Following our example above, the full URL will be `http://hostname/api/webhooks/incoming/twitch/followers` , the `Follower` object passed into Twitch(...) , will handle the New Follower JSON payload sent from Twitch. There is an object to handle Stream Online/Offline named `StreamData`.

### Subscribing for New Followers

```
https://api.twitch.tv/helix/webhooks/hub
?hub.topic=https://api.twitch.tv/helix/users/follows?to_id=26301881
&hub.lease_seconds=864000
&hub.callback=http://hostname/api/webhooks/incoming/twitch/followers
&hub.mode=subscribe
&hub.secret=tw1tch_t3st
```

Notice the `&hub.callback=http://hostname/api/webhooks/incoming/twitch/followers` URL passed in the `hub.callback`

### Subscribing for Stream Online/Offline

```
https://api.twitch.tv/helix/webhooks/hub
?hub.topic=https://api.twitch.tv/helix/streams?user_id=63164470
&hub.lease_seconds=864000
&hub.callback=http://hostname/api/webhooks/incoming/twitch/stream
&hub.mode=subscribe
&hub.secret=tw1tch_t3st
```

Notice the `&hub.callback=http://hostname/api/webhooks/incoming/twitch/stream` URL passed in the `hub.callback`

If you are passing a hub.secret, then you must add the secret to the app.settings file in your ASP.NET Core Web Api

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "Twitch": {
    "SecretKey": "tw1tch_t3st"
  }
}
```

Notice the Twitch section, it must be exactly in this format, and again must match hub.secret sent to twitch when subscribing.
