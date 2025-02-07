using System;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.RemoveService;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Test.Module.Commands.RemoveService;

public class RemoveServiceCommandTest
{
    private readonly Validator validator;

    public RemoveServiceCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new RemoveServiceCommand(Guid.Empty, Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Provided()
    {
        var command = new RemoveServiceCommand(Guid.NewGuid(), Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
