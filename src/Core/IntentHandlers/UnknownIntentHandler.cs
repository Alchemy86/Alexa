using Cascade.Alexa.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cascade.Alexa.Core.IntentHandlers
{
    public class UnknownIntentHandler : IIntentHandler
    {
        public bool AppliesTo(string intent)
        {
            return false;
        }

        public Task<AlexaResponse> GenerateResponseAsync(AlexaRequest request)
        {
            Func<object, AlexaResponse> _generate = (value) => {

                var builder = new AlexaResponseBuilder("Unnknown Request", "Sorry, no idea. Can you please repeat that", true);
                return builder.Generate();

            };
            return Task<AlexaResponse>.Factory.StartNew(_generate, this, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }
    }
}
