using Cascade.Alexa.Core;
using Cascade.Alexa.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cascade.Alexa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlexaController : ControllerBase
    {
        private readonly IIntentResolverService _intentResolverService;

        public AlexaController(IIntentResolverService intentResolverService)
        {
            _intentResolverService = intentResolverService ?? throw new ArgumentNullException(nameof(intentResolverService));
        }

        [HttpPost]
        public AlexaResponse PocAsync(AlexaRequest request)
        {
            try
            {
                var moo = _intentResolverService.ResolveResponseAsync(request);

                return moo.Result;
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
    }
}
