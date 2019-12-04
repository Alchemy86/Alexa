using Cascade.Alexa.Api.Handlers;
using Cascade.Alexa.Core;
using Cascade.Alexa.Core.IntentHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;

namespace Cascade.Alexa.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IEnumerable<IIntentHandler> GetResolvers()
        {
            yield return new CancelOrStopIntentHandler();
            yield return new PayDayIntentHandler();
            yield return new UnknownIntentHandler();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddScoped<IIntentResolverService>(ctx =>
            {
                return new IntentResolverService(GetResolvers());
            });

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<AlexaRequestValidationMiddleware>();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
