namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.DeleteModule;

public class DeleteModuleCommandHandler(IModuleRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteModuleCommand>
{
    public Task Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}