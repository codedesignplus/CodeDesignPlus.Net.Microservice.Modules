namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.DeleteModule;

public class DeleteModuleCommandHandler(IModuleRepository repository, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<DeleteModuleCommand>
{
    public async Task Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<ModuleAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.ModuleNotFound);

        aggregate!.Delete(request.ActorId);

        await repository.DeleteAsync<ModuleAggregate>(aggregate.Id, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);

        await cacheManager.RemoveAsync(aggregate.Id.ToString());
    }
}
