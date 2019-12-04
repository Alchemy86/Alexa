using Cascade.Alexa.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cascade.Alexa.Api.Handlers
{
    public class AlexaRequestValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AlexaRequestValidationMiddleware> _logger;

        public AlexaRequestValidationMiddleware(RequestDelegate next, ILogger<AlexaRequestValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
         
        /// <summary>
        /// Alexa requests will include both Signature and SignatureCertChainUrl headers. The request is invalid without these
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Signature") || !context.Request.Headers.ContainsKey("SignatureCertChainUrl"))
                await FailRequestOnValidationAsync(context, "Missing required header key");

            await ValidateAlexaRequestAsync(context);

            await _next(context);
        }

        private async Task ValidateAlexaRequestAsync(HttpContext context)
        {
            var signature = context.Request.Headers.First(x => x.Key == "Signature").Value;
            var signatureCertChainUrl = context.Request.Headers.First(x => x.Key == "SignatureCertChainUrl").Value;
            var req = context.Request;

            // Allows using several time the stream in ASP.Net Core
            req.EnableRewind();

            using (StreamReader reader
                  = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
            {
                var requestBody = reader.ReadToEnd();
                var validationResults = new AlexaRequestSecurity(signatureCertChainUrl, signature, requestBody).RunValidation();

                if (validationResults.Any())
                    await FailRequestOnValidationAsync(context, validationResults);
            }

            // Reset the body position for the rest of the request
            req.Body.Position = 0;
        }

        private async Task FailRequestOnValidationAsync(HttpContext context, object failureResult)
        {
            var failureData = JsonConvert.SerializeObject(failureResult);
            _logger.LogError($"Amazon alexa request rejected due to validation failures: {failureData}", failureResult);

            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsync(failureData);
        }
    }
}
