namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.CreateModule;

[DtoGenerator]
public record CreateModuleCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<CreateModuleCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
