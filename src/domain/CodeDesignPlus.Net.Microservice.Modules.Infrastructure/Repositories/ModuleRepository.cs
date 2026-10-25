namespace CodeDesignPlus.Net.Microservice.Modules.Infrastructure.Repositories;

public class ModuleRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<ModuleRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), IModuleRepository
{
    /// <inheritdoc />
    public async Task<bool> CreateIfAbsentAsync(ModuleAggregate module, CancellationToken cancellationToken)
    {
        try
        {
            await CreateAsync(module, cancellationToken);

            return true;
        }
        catch (MongoWriteException exception) when (exception.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }
}
