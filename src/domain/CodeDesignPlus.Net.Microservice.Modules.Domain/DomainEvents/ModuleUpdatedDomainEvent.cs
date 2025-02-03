namespace CodeDesignPlus.Net.Microservice.Modules.Domain.DomainEvents;

[EventKey<ModuleAggregate>(1, "ModuleUpdatedDomainEvent")]
public class ModuleUpdatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static ModuleUpdatedDomainEvent Create(Guid aggregateId)
    {
        return new ModuleUpdatedDomainEvent(aggregateId);
    }
}
