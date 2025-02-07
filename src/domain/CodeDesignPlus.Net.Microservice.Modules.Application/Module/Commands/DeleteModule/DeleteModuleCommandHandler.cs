namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.DeleteModule;

public class DeleteModuleCommandHandler(IModuleRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteModuleCommand>
{
    public async Task Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
    {        
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<ModuleAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.ModuleNotFound);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<ModuleAggregate>(aggregate.Id, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}