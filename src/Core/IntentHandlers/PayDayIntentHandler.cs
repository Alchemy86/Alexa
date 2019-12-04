using Cascade.Alexa.Core.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cascade.Alexa.Core.IntentHandlers
{
    public class PayDayIntentHandler : IIntentHandler
    {
        public bool AppliesTo(string intent)
        {
            return intent == "PaydayIntent";
        }

        public Task<AlexaResponse> GenerateResponseAsync(AlexaRequest request)
        {
            Func<object, AlexaResponse> _generate = (value) =>
            {
                var builder = new AlexaResponseBuilder("Pay Details", "I have just checked for you.", true);
                var date = DateTime.Now.Date;
                var slots = request.Request.Intent.GetSlots();
                var dateAskedFor = request.Request.Intent.GetSlots().FirstOrDefault(s => s.Key == "date").Value;

                if (!string.IsNullOrEmpty(dateAskedFor))
                {
                    builder.AppendSpeach($"You are indeed paid on {dateAskedFor}");
                }

                return builder.Generate();
            };

            return Task<AlexaResponse>.Factory.StartNew(_generate, this, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

    }
}
