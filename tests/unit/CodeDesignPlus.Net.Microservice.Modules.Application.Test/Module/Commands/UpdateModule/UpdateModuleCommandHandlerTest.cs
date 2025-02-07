using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.UpdateModule;
using CodeDesignPlus.Net.Microservice.Modules.Domain.DomainEvents;
using CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Test.Module.Commands.UpdateModule;

public class UpdateModuleCommandHandlerTest
{
    private readonly Mock<IModuleRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly UpdateModuleCommandHandler handler;
    private readonly ServiceEntity serviceEntity;
    private readonly ServiceDto serviceDto;

    public UpdateModuleCommandHandlerTest()
    {
        serviceEntity = new ServiceEntity
        {
            Id = Guid.NewGuid(),
            Name = "TestService",
            Controller = "TestController",
            Action = "TestAction"
        };

        serviceDto = new ServiceDto
        {
            Id = serviceEntity.Id,
            Name = serviceEntity.Name,
            Controller = serviceEntity.Controller,
            Action = serviceEntity.Action
        };

        repositoryMock = new Mock<IModuleRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        mapperMock = new Mock<IMapper>();
        handler = new UpdateModuleCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object, mapperMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        UpdateModuleCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ModuleNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = new UpdateModuleCommand(Guid.NewGuid(), "Test Module", "Test Description", [serviceDto], true);
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(r => r.FindAsync<ModuleAggregate>(It.IsAny<Guid>(), cancellationToken))
            .ReturnsAsync((ModuleAggregate)null!);

        // Act & Assert
        await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesModule()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var module = ModuleAggregate.Create(Guid.NewGuid(), "Test Module", "Test Description", [], Guid.NewGuid());
        var request = new UpdateModuleCommand(module.Id, "Test Module", "Test Description", [serviceDto], true);

        repositoryMock
            .Setup(r => r.FindAsync<ModuleAggregate>(It.IsAny<Guid>(), cancellationToken))
            .ReturnsAsync(module);

        mapperMock
            .Setup(m => m.Map<List<ServiceEntity>>(request.Services))
            .Returns([]);
        
        userContextMock
            .SetupGet(u => u.IdUser)
            .Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.UpdateAsync(module, cancellationToken), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<ModuleUpdatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
