namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.AddService;

[DtoGenerator]
public record AddServiceCommand(Guid Id, Guid IdService, string Name, string Controller, string Action) : IRequest;

public class Validator : AbstractValidator<AddServiceCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
        RuleFor(x => x.Controller).NotEmpty().NotNull().MaximumLength(64);
        RuleFor(x => x.Action).NotEmpty().NotNull().MaximumLength(64);
    }
}
