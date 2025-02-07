using CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.UpdateModule;

public class UpdateModuleCommandHandler(IModuleRepository repository, IUserContext user, IPubSub pubsub, IMapper mapper) : IRequestHandler<UpdateModuleCommand>
{
    public async Task Handle(UpdateModuleCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var module = await repository.FindAsync<ModuleAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(module, Errors.ModuleNotFound);

        var services = mapper.Map<List<ServiceEntity>>(request.Services);

        module.Update(request.Name, request.Description, services, request.IsActive, user.IdUser);

        await repository.UpdateAsync(module, cancellationToken);

        await pubsub.PublishAsync(module.GetAndClearEvents(), cancellationToken);
    }
}