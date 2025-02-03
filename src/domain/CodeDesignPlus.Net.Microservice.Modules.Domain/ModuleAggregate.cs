using CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Modules.Domain;

public class ModuleAggregate(Guid id) : AggregateRootBase(id)
{
    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public List<ServiceEntity> Services { get; private set; } = [];

    private ModuleAggregate(Guid id, string name, string description, List<ServiceEntity> services, Guid createdBy) : this(id)
    {
        this.Name = name;
        this.Description = description;
        this.Services = services;
        this.IsActive = true;
        this.CreatedBy = createdBy;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(ModuleCreatedDomainEvent.Create(Id, Name, Description, Services, IsActive));
    }

    public static ModuleAggregate Create(Guid id, string name, string description, List<ServiceEntity> services, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdModuleIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameModuleIsInvalid);
        DomainGuard.IsNullOrEmpty(description, Errors.DescriptionModuleIsInvalid);
        DomainGuard.GuidIsEmpty(createdBy, Errors.IdUserIsInvalid);

        return new ModuleAggregate(id, name, description, services, createdBy);
    }

    public void Update(string name, string description, List<ServiceEntity> services, bool isActive, Guid updatedBy)
    {
        DomainGuard.IsNullOrEmpty(name, Errors.NameModuleIsInvalid);
        DomainGuard.IsNullOrEmpty(description, Errors.DescriptionModuleIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.IdUserIsInvalid);

        Name = name;
        Description = description;
        Services = services;
        UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        UpdatedBy = updatedBy;

        AddEvent(ModuleUpdatedDomainEvent.Create(Id, Name, Description, Services, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.IdUserIsInvalid);

        IsActive = false;
        UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        UpdatedBy = deletedBy;

        AddEvent(ModuleDeletedDomainEvent.Create(Id, Name, Description, Services, IsActive));
    }

    public void AddService(Guid id, string name, string controller, string action, Guid addedBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdServiceIsInvalid);
        DomainGuard.GuidIsEmpty(addedBy, Errors.IdUserIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameServiceIsInvalid);
        DomainGuard.IsNullOrEmpty(controller, Errors.ControllerServiceIsInvalid);
        DomainGuard.IsNullOrEmpty(action, Errors.ActionServiceIsInvalid);

        var service = new ServiceEntity
        {
            Id = id,
            Name = name,
            Controller = controller,
            Action = action
        };

        Services.Add(service);
        UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        UpdatedBy = addedBy;

        AddEvent(ServiceAddedDomainEvent.Create(Id, service.Id, service.Name, service.Controller, service.Action));
    }

    public void RemoveService(Guid idService, Guid removedBy)
    {
        DomainGuard.GuidIsEmpty(removedBy, Errors.IdUserIsInvalid);

        var service = Services.FirstOrDefault(x => x.Id == idService);

        DomainGuard.IsNull(service, Errors.ServiceNotFound);

        Services.Remove(service);
        UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        UpdatedBy = removedBy;

        AddEvent(ServiceRemovedDomainEvent.Create(Id, service.Id, service.Name, service.Controller, service.Action));
    }
}
