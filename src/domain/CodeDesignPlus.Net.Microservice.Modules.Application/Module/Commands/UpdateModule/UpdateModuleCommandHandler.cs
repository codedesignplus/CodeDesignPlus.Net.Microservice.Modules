namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.UpdateModule;

public class UpdateModuleCommandHandler(IModuleRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateModuleCommand>
{
    public Task Handle(UpdateModuleCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}