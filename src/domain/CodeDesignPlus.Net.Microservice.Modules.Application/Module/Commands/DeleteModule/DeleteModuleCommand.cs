namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.DeleteModule;

[DtoGenerator]
public record DeleteModuleCommand(Guid Id, Guid ActorId) : IRequest;

public class Validator : AbstractValidator<DeleteModuleCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.ActorId).NotEmpty().NotNull();
    }
}
