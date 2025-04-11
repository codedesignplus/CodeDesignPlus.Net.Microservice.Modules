using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Queries.GetAllModule;

public record GetAllModuleQuery(C.Criteria Criteria) : IRequest<Pagination<ModuleDto>>;

