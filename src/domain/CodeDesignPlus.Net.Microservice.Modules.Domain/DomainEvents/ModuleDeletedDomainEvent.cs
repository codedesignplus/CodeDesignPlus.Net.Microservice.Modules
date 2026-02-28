using CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Modules.Domain.DomainEvents;

[EventKey<ModuleAggregate>(1, "ModuleDeletedDomainEvent")]
public class ModuleDeletedDomainEvent(
     Guid aggregateId,
    string name, 
    string description, 
    List<ServiceEntity> services,
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public string Name { get; private set; } = name;

    public string Description { get; private set; } = description;

    public List<ServiceEntity> Services { get; private set; } = services;

    public bool IsActive { get; private set; } = isActive;

    public static ModuleDeletedDomainEvent Create(Guid aggregateId, string name, string description, List<ServiceEntity> services, bool isActive)
    {
        return new ModuleDeletedDomainEvent(aggregateId, name, description, services, isActive);
    }
}
