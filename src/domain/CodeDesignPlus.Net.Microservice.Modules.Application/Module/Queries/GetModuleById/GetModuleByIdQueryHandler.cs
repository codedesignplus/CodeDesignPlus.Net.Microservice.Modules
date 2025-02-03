namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Queries.GetModuleById;

public class GetModuleByIdQueryHandler(IModuleRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<GetModuleByIdQuery, ModuleDto>
{
    public Task<ModuleDto> Handle(GetModuleByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<ModuleDto>(default!);
    }
}
