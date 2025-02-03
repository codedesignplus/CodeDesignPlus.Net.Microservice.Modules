namespace CodeDesignPlus.Net.Microservice.Modules.Domain.DomainEvents;

[EventKey<ModuleAggregate>(1, "ModuleCreatedDomainEvent")]
public class ModuleCreatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static ModuleCreatedDomainEvent Create(Guid aggregateId)
    {
        return new ModuleCreatedDomainEvent(aggregateId);
    }
}
