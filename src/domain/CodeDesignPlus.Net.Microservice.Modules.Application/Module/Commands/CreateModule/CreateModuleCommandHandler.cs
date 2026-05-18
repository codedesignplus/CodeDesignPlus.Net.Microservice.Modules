using CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.CreateModule;

public class CreateModuleCommandHandler(IModuleRepository repository, IUserContext user, IPubSub pubsub, IMapper mapper) : IRequestHandler<CreateModuleCommand>
{
    public async Task Handle(CreateModuleCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);
        
        var exist = await repository.ExistsAsync<ModuleAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.ModuleAlreadyExists);

        var services = mapper.Map<List<ServiceEntity>>(request.Services);

        Guid createdBy;
        try { createdBy = user.IdUser; }
        catch { createdBy = Guid.Parse("10000000-0000-0000-0000-000000000001"); }
        if (createdBy == Guid.Empty) createdBy = Guid.Parse("10000000-0000-0000-0000-000000000001");

        var module = ModuleAggregate.Create(request.Id, request.Name, request.Description, services, createdBy);

        await repository.CreateAsync(module, cancellationToken);

        await pubsub.PublishAsync(module.GetAndClearEvents(), cancellationToken);
    }
}