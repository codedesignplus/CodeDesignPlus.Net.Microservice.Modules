namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Queries.GetModuleById;

public class GetModuleByIdQueryHandler(IModuleRepository repository, IMapper mapper, ICacheManager cacheManager) : IRequestHandler<GetModuleByIdQuery, ModuleDto>
{
    public async Task<ModuleDto> Handle(GetModuleByIdQuery request, CancellationToken cancellationToken)
    {
        
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exists = await cacheManager.ExistsAsync(request.Id.ToString());

        if (exists)
            return await cacheManager.GetAsync<ModuleDto>(request.Id.ToString());

        var module = await repository.FindAsync<ModuleAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(module, Errors.ModuleNotFound);

        await cacheManager.SetAsync(request.Id.ToString(), mapper.Map<ModuleDto>(module));

        return mapper.Map<ModuleDto>(module);
    }
}
