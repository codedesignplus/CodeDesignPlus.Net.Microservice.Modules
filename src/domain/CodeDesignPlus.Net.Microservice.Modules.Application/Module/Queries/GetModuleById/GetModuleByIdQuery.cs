namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Queries.GetModuleById;

public record GetModuleByIdQuery(Guid Id) : IRequest<ModuleDto>;

