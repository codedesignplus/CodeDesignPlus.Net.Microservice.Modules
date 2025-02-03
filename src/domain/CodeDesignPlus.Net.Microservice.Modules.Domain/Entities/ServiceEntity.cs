namespace CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;

public class ServiceEntity : IEntityBase
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Controller { get; set; }

    public required string Action { get; set; }
}
