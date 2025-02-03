namespace CodeDesignPlus.Net.Microservice.Modules.Domain.DomainEvents;

[EventKey<ModuleAggregate>(1, "ModuleDeletedDomainEvent")]
public class ModuleDeletedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static ModuleDeletedDomainEvent Create(Guid aggregateId)
    {
        return new ModuleDeletedDomainEvent(aggregateId);
    }
}
