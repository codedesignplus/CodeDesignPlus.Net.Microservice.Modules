namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Queries.GetAllModule;

public record GetAllModuleQuery(C.Criteria Criteria) : IRequest<List<ModuleDto>>;

