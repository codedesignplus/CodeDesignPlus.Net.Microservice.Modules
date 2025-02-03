namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.RemoveService;

public class RemoveServiceCommandHandler(IModuleRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<RemoveServiceCommand>
{
    public async Task Handle(RemoveServiceCommand request, CancellationToken cancellationToken)
    {        
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var module = await repository.FindAsync<ModuleAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsNull(module, Errors.ModuleNotFound);

        module.RemoveService(request.IdService, user.IdUser);

        await repository.UpdateAsync(module, cancellationToken);

        await pubsub.PublishAsync(module.GetAndClearEvents(), cancellationToken);
    }
}