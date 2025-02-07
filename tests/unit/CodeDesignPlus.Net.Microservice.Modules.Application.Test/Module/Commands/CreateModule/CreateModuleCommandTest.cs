using System;
using System.Collections.Generic;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.CreateModule;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Test.Module.Commands.CreateModule;

public class CreateModuleCommandTest
{
    private readonly Validator validator;

    public CreateModuleCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new CreateModuleCommand(Guid.Empty, "ValidName", "ValidDescription", []);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateModuleCommand(Guid.NewGuid(), string.Empty, "ValidDescription", []);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        var command = new CreateModuleCommand(Guid.NewGuid(), new string('a', 129), "ValidDescription", []);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var command = new CreateModuleCommand(Guid.NewGuid(), "ValidName", string.Empty, []);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_Max_Length()
    {
        var command = new CreateModuleCommand(Guid.NewGuid(), "ValidName", new string('a', 513), []);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateModuleCommand(Guid.NewGuid(), "ValidName", "ValidDescription", []);
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
