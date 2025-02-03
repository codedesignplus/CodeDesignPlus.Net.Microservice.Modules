namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.DeleteModule;

[DtoGenerator]
public record DeleteModuleCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<DeleteModuleCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
