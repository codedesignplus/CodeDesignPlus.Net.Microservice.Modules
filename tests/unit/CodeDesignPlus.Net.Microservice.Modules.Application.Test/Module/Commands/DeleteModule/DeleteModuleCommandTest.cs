using System;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.DeleteModule;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Test.Module.Commands.DeleteModule;

public class DeleteModuleCommandTest
{
    private readonly Validator validator;

    public DeleteModuleCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeleteModuleCommand(Guid.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var command = new DeleteModuleCommand(Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
