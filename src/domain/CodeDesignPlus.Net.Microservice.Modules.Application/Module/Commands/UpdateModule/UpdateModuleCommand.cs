namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.UpdateModule;

[DtoGenerator]
public record UpdateModuleCommand(Guid Id, string Name, string Description, List<ServiceDto> Services, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateModuleCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(512);
    }
}
