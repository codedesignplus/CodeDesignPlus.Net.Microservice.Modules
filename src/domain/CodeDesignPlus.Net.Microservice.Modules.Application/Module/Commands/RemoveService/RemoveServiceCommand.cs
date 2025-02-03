namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.RemoveService;

[DtoGenerator]
public record RemoveServiceCommand(Guid Id, Guid IdService) : IRequest;

public class Validator : AbstractValidator<RemoveServiceCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
