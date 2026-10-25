using CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.CreateModule;

public class CreateModuleCommandHandler(IModuleRepository repository, IPubSub pubsub, IMapper mapper) : IRequestHandler<CreateModuleCommand>
{
    public async Task Handle(CreateModuleCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        // Ya guardado por un envío anterior del mismo formulario: el reintento termina bien sin repetir nada (pendings/589).
        if (await repository.ExistsAsync<ModuleAggregate>(request.Id, cancellationToken))
            return;

        var services = mapper.Map<List<ServiceEntity>>(request.Services);

        var module = ModuleAggregate.Create(request.Id, request.Name, request.Description, services, request.ActorId);

        // Dos envíos que llegan a la vez pasan los dos la comprobación de arriba; el segundo choca contra el id y no publica.
        if (!await repository.CreateIfAbsentAsync(module, cancellationToken))
            return;

        await pubsub.PublishAsync(module.GetAndClearEvents(), cancellationToken);
    }
}
