namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.CreateModule;

[DtoGenerator]
public record CreateModuleCommand(Guid Id, string Name, string Description, List<ServiceDto> Services) : IRequest;

public class Validator : AbstractValidator<CreateModuleCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
