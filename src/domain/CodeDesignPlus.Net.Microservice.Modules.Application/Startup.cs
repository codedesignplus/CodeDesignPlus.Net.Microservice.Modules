using CodeDesignPlus.Net.Core.Abstractions;
using CodeDesignPlus.Net.Microservice.Modules.Application.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDesignPlus.Net.Microservice.Modules.Application
{
    public class Startup : IStartup
    {
        public void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            MapsterConfigModule.Configure();
        }
    }
}
