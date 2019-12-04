using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cascade.Alexa.Core.IntentHandlers;
using Cascade.Alexa.Core.Models;

namespace Cascade.Alexa.Core
{
    public class IntentResolverService : IIntentResolverService
    {
        private readonly IEnumerable<IIntentHandler> _intentResolvers;
        public IntentResolverService(IEnumerable<IIntentHandler> intentResolvers)
        {
            _intentResolvers = intentResolvers ?? Enumerable.Empty<IIntentHandler>();
        }

        public async Task<AlexaResponse> ResolveResponseAsync(AlexaRequest request)
        {
            AlexaResponse response = null;

            switch (request.Request.Type)
            {
                case "LaunchRequest":
                    response = await LaunchRequestHandlerAsync(request);
                    break;
                case "IntentRequest":
                    response = await ResolveIntentAsync(request);
                    break;
                case "SessionEndedRequest":
                    response = new AlexaResponseBuilder("Goodbye", "Goodbye", true).Generate();
                    break;
                default:
                    break;
            }

            return response;
        }

        private async Task<AlexaResponse> LaunchRequestHandlerAsync(AlexaRequest request)
        {
            Func<object, AlexaResponse> _generate = (value) =>
            {
                return new AlexaResponseBuilder("Welsome to HR Self Service", "Welcome to HR Self Service from Cascade. How can I help", false)
                    //response.SessionAttributes.SessionId = request.Session.SessionId;
                    .SetCardContent("You can ask about pay, tasks and absence details")
                    .SetReprompt("You can ask about pay, tasks and absence details")
                    .Generate();
            };

            return await Task<AlexaResponse>.Factory.StartNew(_generate, this, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        private async Task<AlexaResponse> ResolveIntentAsync(AlexaRequest request)
        {
            return await _intentResolvers
                    .FirstOr(x => x.AppliesTo(request?.Request?.Intent?.Name), () => new UnknownIntentHandler())
                    .GenerateResponseAsync(request);
        }
    }
}
