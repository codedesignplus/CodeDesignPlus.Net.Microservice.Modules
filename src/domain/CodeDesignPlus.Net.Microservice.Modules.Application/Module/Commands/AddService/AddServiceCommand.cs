namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.AddService;

[DtoGenerator]
public record AddServiceCommand(Guid Id, Guid IdService, string Name, string Controller, string Action) : IRequest;

public class Validator : AbstractValidator<AddServiceCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
