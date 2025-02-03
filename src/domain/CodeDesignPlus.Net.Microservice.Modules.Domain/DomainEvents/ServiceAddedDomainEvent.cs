namespace CodeDesignPlus.Net.Microservice.Modules.Domain.DomainEvents;

[EventKey<ModuleAggregate>(1, "ServiceAddedDomainEvent")]
public class ServiceAddedDomainEvent(
    Guid aggregateId,
    Guid idService,
    string name,
    string controller,
    string action,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public Guid IdService { get; private set; } = idService;

    public string Name { get; private set; } = name;

    public string Controller { get; private set; } = controller;

    public string Action { get; private set; } = action;

    public static ServiceAddedDomainEvent Create(Guid aggregateId, Guid idService, string name, string controller, string action)
    {
        return new ServiceAddedDomainEvent(aggregateId, idService, name, controller, action);
    }
}
