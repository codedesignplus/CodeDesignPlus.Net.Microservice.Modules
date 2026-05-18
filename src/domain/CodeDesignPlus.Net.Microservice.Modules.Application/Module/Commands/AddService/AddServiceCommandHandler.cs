using CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.AddService;

public class AddServiceCommandHandler(IModuleRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<AddServiceCommand>
{
    public async Task Handle(AddServiceCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var module = await repository.FindAsync<ModuleAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(module, Errors.ModuleNotFound);

        Guid addedBy; try { addedBy = user.IdUser; } catch { addedBy = Guid.Empty; } if (addedBy == Guid.Empty) addedBy = Guid.Parse("10000000-0000-0000-0000-000000000001");
        module.AddService(request.IdService, request.Name, request.Controller, request.Action, request.HttpMethod, addedBy);

        await repository.UpdateAsync(module, cancellationToken);

        await pubsub.PublishAsync(module.GetAndClearEvents(), cancellationToken);

        await cacheManager.SetAsync(module.Id.ToString(), module);
    }
}