namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Queries.GetAllModule;

public class GetAllModuleQueryHandler(IModuleRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<GetAllModuleQuery, ModuleDto>
{
    public Task<ModuleDto> Handle(GetAllModuleQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<ModuleDto>(default!);
    }
}
