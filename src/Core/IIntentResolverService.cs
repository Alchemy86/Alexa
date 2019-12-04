using Cascade.Alexa.Core.Models;
using System.Threading.Tasks;

namespace Cascade.Alexa.Core
{
    public interface IIntentResolverService
    {
        Task<AlexaResponse> ResolveResponseAsync(AlexaRequest request);
    }
}
