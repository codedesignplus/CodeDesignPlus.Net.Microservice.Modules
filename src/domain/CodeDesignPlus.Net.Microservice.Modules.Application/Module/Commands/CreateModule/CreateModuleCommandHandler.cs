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

        var module = ModuleAggregate.Create(request.Id, request.Name, request.Description, services, user.IdUser);

        await repository.CreateAsync(module, cancellationToken);

        await pubsub.PublishAsync(module.GetAndClearEvents(), cancellationToken);
    }
}