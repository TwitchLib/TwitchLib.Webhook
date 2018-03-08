using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebHooks.Filters;
using Microsoft.AspNetCore.WebHooks.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Treyza.AspNetCore.WebHooks.Receivers.Twitch.Filters
{
    public class TwitchGetHeadRequestFilter : WebHookGetHeadRequestFilter, IAsyncResourceFilter
    {

        private readonly IReadOnlyList<IWebHookGetHeadRequestMetadata> _getHeadRequestMetadata;

        public TwitchGetHeadRequestFilter(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory, IEnumerable<IWebHookMetadata> metadata) : base(configuration, hostingEnvironment, loggerFactory, metadata)
        {

            _getHeadRequestMetadata = metadata.OfType<IWebHookGetHeadRequestMetadata>().ToArray();
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)

        {
            var routeData = context.RouteData;
            var request = context.HttpContext.Request;

            if (routeData.TryGetWebHookReceiverName(out var receiverName) && HttpMethods.IsGet(context.HttpContext.Request.Method))
            {
                var getHeadRequestMetadata = _getHeadRequestMetadata
                    .FirstOrDefault(metadata => metadata.IsApplicable(receiverName));
                context.Result = GetChallengeResponse(getHeadRequestMetadata, receiverName, request, routeData);

                return;
            }
            await next();
        }


        private IActionResult GetChallengeResponse(
            IWebHookGetHeadRequestMetadata getHeadRequestMetadata,
            string receiverName,
            HttpRequest request,
            RouteData routeData)
        {
            // Get the 'challenge' parameter from the request URI.
            var challenge = request.Query[getHeadRequestMetadata.ChallengeQueryParameterName];
   

            Logger.LogInformation(
                403,
                "Received a GET request for the '{ReceiverName}' WebHook receiver -- returning challenge response.",
                receiverName);

            // Echo the challenge back to the caller.
            return new ContentResult
            {
                Content = challenge,
            };
        }
    }
}
