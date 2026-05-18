using CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.AddService;

public class AddServiceCommandHandler(IModuleRepository repository, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<AddServiceCommand>
{
    public async Task Handle(AddServiceCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var module = await repository.FindAsync<ModuleAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(module, Errors.ModuleNotFound);

        module!.AddService(request.IdService, request.Name, request.Controller, request.Action, request.HttpMethod, request.ActorId);

        await repository.UpdateAsync(module, cancellationToken);

        await pubsub.PublishAsync(module.GetAndClearEvents(), cancellationToken);

        await cacheManager.SetAsync(module.Id.ToString(), module);
    }
}
