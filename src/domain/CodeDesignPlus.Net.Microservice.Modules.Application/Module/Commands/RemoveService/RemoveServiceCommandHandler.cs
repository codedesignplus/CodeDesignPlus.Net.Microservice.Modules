namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.RemoveService;

public class RemoveServiceCommandHandler(IModuleRepository repository, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<RemoveServiceCommand>
{
    public async Task Handle(RemoveServiceCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var module = await repository.FindAsync<ModuleAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(module, Errors.ModuleNotFound);

        module!.RemoveService(request.IdService, request.ActorId);

        await repository.UpdateAsync(module, cancellationToken);

        await pubsub.PublishAsync(module.GetAndClearEvents(), cancellationToken);

        await cacheManager.SetAsync(module.Id.ToString(), module);
    }
}
