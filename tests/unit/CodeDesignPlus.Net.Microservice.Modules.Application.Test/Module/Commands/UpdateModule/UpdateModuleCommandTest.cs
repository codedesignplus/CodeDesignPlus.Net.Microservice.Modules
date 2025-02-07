using System;
using Xunit;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.UpdateModule;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Test.Module.Commands.UpdateModule
{
    public class UpdateModuleCommandTest
    {
        private readonly Validator validator;

        public UpdateModuleCommandTest()
        {
            validator = new Validator();
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty()
        {
            var command = new UpdateModuleCommand(Guid.Empty, "ValidName", "ValidDescription", [], true);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var command = new UpdateModuleCommand(Guid.NewGuid(), string.Empty, "ValidDescription", [], true);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Exceeds_MaxLength()
        {
            var command = new UpdateModuleCommand(Guid.NewGuid(), new string('a', 129), "ValidDescription", [], true);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            var command = new UpdateModuleCommand(Guid.NewGuid(), "ValidName", string.Empty, [], true);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Exceeds_MaxLength()
        {
            var command = new UpdateModuleCommand(Guid.NewGuid(), "ValidName", new string('a', 513), [], true);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Command_Is_Valid()
        {
            var command = new UpdateModuleCommand(Guid.NewGuid(), "ValidName", "ValidDescription", [], true);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
