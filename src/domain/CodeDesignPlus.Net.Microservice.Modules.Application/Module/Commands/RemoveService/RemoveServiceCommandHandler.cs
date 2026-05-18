namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.RemoveService;

public class RemoveServiceCommandHandler(IModuleRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<RemoveServiceCommand>
{
    public async Task Handle(RemoveServiceCommand request, CancellationToken cancellationToken)
    {        
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var module = await repository.FindAsync<ModuleAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsNull(module, Errors.ModuleNotFound);

        Guid removedBy; try { removedBy = user.IdUser; } catch { removedBy = Guid.Empty; } if (removedBy == Guid.Empty) removedBy = Guid.Parse("10000000-0000-0000-0000-000000000001");
        module.RemoveService(request.IdService, removedBy);

        await repository.UpdateAsync(module, cancellationToken);

        await pubsub.PublishAsync(module.GetAndClearEvents(), cancellationToken);

        await cacheManager.SetAsync(module.Id.ToString(), module);
    }
}