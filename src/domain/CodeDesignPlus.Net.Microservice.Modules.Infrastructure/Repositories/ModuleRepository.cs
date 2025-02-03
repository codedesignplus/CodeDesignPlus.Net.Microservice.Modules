namespace CodeDesignPlus.Net.Microservice.Modules.Infrastructure.Repositories;

public class ModuleRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<ModuleRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), IModuleRepository
{
   
}