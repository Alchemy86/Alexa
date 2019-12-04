using Cascade.Alexa.Core.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cascade.Alexa.Core.IntentHandlers
{
    public class CancelOrStopIntentHandler : IIntentHandler
    {
        public bool AppliesTo(string intent)
        {
            string[] matches = { "AMAZON.StopIntent", "AMAZON.CancelIntent" };
            return matches.Contains(intent);
        }

        public Task<AlexaResponse> GenerateResponseAsync(AlexaRequest request)
        {
            Func<object, AlexaResponse> _generate = (value) => new AlexaResponseBuilder("Done", "Request has now ended. Good job chaps", true).Generate();
            return Task<AlexaResponse>.Factory.StartNew(_generate, this, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }
    }
}
