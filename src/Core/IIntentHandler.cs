using Cascade.Alexa.Core.Models;
using System.Threading.Tasks;

namespace Cascade.Alexa.Core
{
    public interface IIntentHandler
    {
        bool AppliesTo(string intent);
        Task<AlexaResponse> GenerateResponseAsync(AlexaRequest request);
    }
}
