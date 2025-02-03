namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.CreateModule;

public class CreateModuleCommandHandler(IModuleRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateModuleCommand>
{
    public Task Handle(CreateModuleCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}