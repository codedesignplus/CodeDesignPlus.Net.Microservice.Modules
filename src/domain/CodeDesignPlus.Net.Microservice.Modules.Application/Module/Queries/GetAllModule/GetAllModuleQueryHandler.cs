namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Queries.GetAllModule;

public class GetAllModuleQueryHandler(IModuleRepository repository, IMapper mapper) : IRequestHandler<GetAllModuleQuery, List<ModuleDto>>
{
    public async Task<List<ModuleDto>> Handle(GetAllModuleQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var modules = await repository.MatchingAsync<ModuleAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<List<ModuleDto>>(modules);
    }
}
