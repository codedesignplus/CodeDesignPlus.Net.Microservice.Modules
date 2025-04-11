using CodeDesignPlus.Net.Microservice.Modules.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.DataTransferObjects;

public class ServiceDto : IDtoBase
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Controller { get; set; }

    public required string Action { get; set; }
    
    public Domain.Enums.HttpMethod HttpMethod { get; set; } = Domain.Enums.HttpMethod.None; 
}
