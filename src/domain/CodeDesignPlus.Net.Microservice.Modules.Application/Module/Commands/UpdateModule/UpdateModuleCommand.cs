namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.UpdateModule;

[DtoGenerator]
public record UpdateModuleCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<UpdateModuleCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
