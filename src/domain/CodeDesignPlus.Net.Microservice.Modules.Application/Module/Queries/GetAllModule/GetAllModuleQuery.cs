namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Queries.GetAllModule;

public record GetAllModuleQuery(Guid Id) : IRequest<ModuleDto>;

