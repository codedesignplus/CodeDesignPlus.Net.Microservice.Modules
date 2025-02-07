using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.AddService;
using FluentValidation.TestHelper;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Test.Module.Commands.AddService;

public class AddServiceCommandTest
{
    private readonly Validator validator;

    public AddServiceCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new AddServiceCommand(Guid.Empty, Guid.NewGuid(), "ServiceName", "ControllerName", "ActionName");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new AddServiceCommand(Guid.NewGuid(), Guid.NewGuid(), string.Empty, "ControllerName", "ActionName");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var command = new AddServiceCommand(Guid.NewGuid(), Guid.NewGuid(), new string('a', 129), "ControllerName", "ActionName");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Controller_Is_Empty()
    {
        var command = new AddServiceCommand(Guid.NewGuid(), Guid.NewGuid(), "ServiceName", string.Empty, "ActionName");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Controller);
    }

    [Fact]
    public void Should_Have_Error_When_Controller_Exceeds_MaxLength()
    {
        var command = new AddServiceCommand(Guid.NewGuid(), Guid.NewGuid(), "ServiceName", new string('a', 65), "ActionName");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Controller);
    }

    [Fact]
    public void Should_Have_Error_When_Action_Is_Empty()
    {
        var command = new AddServiceCommand(Guid.NewGuid(), Guid.NewGuid(), "ServiceName", "ControllerName", string.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Action);
    }

    [Fact]
    public void Should_Have_Error_When_Action_Exceeds_MaxLength()
    {
        var command = new AddServiceCommand(Guid.NewGuid(), Guid.NewGuid(), "ServiceName", "ControllerName", new string('a', 65));
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Action);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new AddServiceCommand(Guid.NewGuid(), Guid.NewGuid(), "ServiceName", "ControllerName", "ActionName");
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
