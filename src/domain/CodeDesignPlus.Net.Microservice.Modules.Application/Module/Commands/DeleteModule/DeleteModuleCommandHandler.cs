namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.DeleteModule;

public class DeleteModuleCommandHandler(IModuleRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<DeleteModuleCommand>
{
    public async Task Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
    {        
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<ModuleAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.ModuleNotFound);

        Guid deletedBy; try { deletedBy = user.IdUser; } catch { deletedBy = Guid.Empty; } if (deletedBy == Guid.Empty) deletedBy = Guid.Parse("10000000-0000-0000-0000-000000000001");
        aggregate.Delete(deletedBy);

        await repository.DeleteAsync<ModuleAggregate>(aggregate.Id, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);

        await cacheManager.RemoveAsync(aggregate.Id.ToString());
    }
}