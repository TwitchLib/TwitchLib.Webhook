using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebHooks.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;



namespace Treyza.AspNetCore.WebHooks.Receivers.Twitch.Filters
{
    public class TwitchVerifySignatureFilter : WebHookVerifySignatureFilter, IAsyncResourceFilter
    {

        private readonly IConfiguration _config;
        public TwitchVerifySignatureFilter(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory) 
            : base(configuration, hostingEnvironment, loggerFactory)
        {

            _config = configuration;

        }

        public override string ReceiverName => TwitchConstants.ReceiverName;
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var routeData = context.RouteData;
            var request = context.HttpContext.Request;
            if (routeData.TryGetWebHookReceiverName(out var receiverName) &&
                IsApplicable(receiverName) &&
                HttpMethods.IsPost(request.Method))
            {
                // 1. Confirm a secure connection.
                var errorResult = EnsureSecureConnection(ReceiverName, context.HttpContext.Request);
                if (errorResult != null)
                {
                    context.Result = errorResult;
                    return;
                }

                // 2. Get the expected hash from the signature header.
                var headerValue = GetRequestHeader(request, TwitchConstants.SignatureHeaderName, out errorResult);
                if (errorResult == null)
                {
                    Logger.LogInformation($"{TwitchConstants.SignatureHeaderName} found, performing validation");


                    // comes across in the following format X-Hub-Signature	sha256=8199e7481c3efbf3bd7450ddb6c3599a915e893f2140bba663cb45c5347f4330 
                    // grab only the hex value
                    var fields = headerValue.Split('=');
                    if (fields.Length != 2)
                    {
                        context.Result = new BadRequestObjectResult(headerValue);
                        return;

                    }

                    var header = fields[1];
                    if (string.IsNullOrEmpty(header))
                    {
                        context.Result = new BadRequestObjectResult(fields);
                        return;
                    }

                    var expectedHash = FromHex(header, TwitchConstants.SignatureHeaderName);
                    if (expectedHash == null)
                    {
                        context.Result = CreateBadHexEncodingResult(TwitchConstants.SignatureHeaderName);
                        return;
                    }

                    // 3. Get the configured secret key from appsettings.json.
                    var secretKey = _config.GetSection("Twitch:SecretKey").Value;


                    var secret = Encoding.UTF8.GetBytes(secretKey);

                    // 4. Get the actual hash of the request body.
                    var actualHash = await ComputeRequestBodySha256HashAsync(request, secret);

                    // 5. Verify that the actual hash matches the expected hash.
                    if (!SecretEqual(expectedHash, actualHash))
                    {
                        // Log about the issue and short-circuit remainder of the pipeline.
                        errorResult = CreateBadSignatureResult(TwitchConstants.SignatureHeaderName);

                        context.Result = errorResult;
                        return;
                    }


                }
                else
                {
                    // hub.secret is optional, if X-Hub-Signature not found, signature validation will be skipped, just continue
                    Logger.LogInformation($"{TwitchConstants.SignatureHeaderName} not found, skipping validation");

                }

            }

            await next();




        }
    }
}
