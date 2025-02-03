namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.DataTransferObjects;

public class ServiceDto : IDtoBase
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Controller { get; set; }

    public required string Action { get; set; }
}
