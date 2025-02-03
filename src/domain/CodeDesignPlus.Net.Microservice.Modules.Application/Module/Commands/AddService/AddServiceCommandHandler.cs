using CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.AddService;

public class AddServiceCommandHandler(IModuleRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<AddServiceCommand>
{
    public async Task Handle(AddServiceCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var module = await repository.FindAsync<ModuleAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsNull(module, Errors.ModuleNotFound);

        module.AddService(request.IdService, request.Name, request.Controller, request.Action, user.IdUser);

        await repository.UpdateAsync(module, cancellationToken);

        await pubsub.PublishAsync(module.GetAndClearEvents(), cancellationToken);
    }
}