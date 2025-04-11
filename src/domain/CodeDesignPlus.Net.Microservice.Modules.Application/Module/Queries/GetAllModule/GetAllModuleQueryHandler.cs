using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Queries.GetAllModule;

public class GetAllModuleQueryHandler(IModuleRepository repository, IMapper mapper) : IRequestHandler<GetAllModuleQuery, Pagination<ModuleDto>>
{
    public async Task<Pagination<ModuleDto>> Handle(GetAllModuleQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var modules = await repository.MatchingAsync<ModuleAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<ModuleDto>>(modules);
    }
}
