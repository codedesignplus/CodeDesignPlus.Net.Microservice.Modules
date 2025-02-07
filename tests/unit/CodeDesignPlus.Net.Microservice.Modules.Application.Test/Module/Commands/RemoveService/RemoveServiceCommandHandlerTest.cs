using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.RemoveService;
using CodeDesignPlus.Net.Microservice.Modules.Domain.DomainEvents;
using CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Test.Module.Commands.RemoveService;

public class RemoveServiceCommandHandlerTest
{
    private readonly Mock<IModuleRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly RemoveServiceCommandHandler handler;
    private readonly ServiceEntity service = new ()
    {
        Id = Guid.NewGuid(),
        Name = "TestService",
        Controller = "TestController",
        Action = "TestAction"
    };
    

    public RemoveServiceCommandHandlerTest()
    {
        repositoryMock = new Mock<IModuleRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new RemoveServiceCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        RemoveServiceCommand request = null!;
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
        var request = new RemoveServiceCommand(Guid.NewGuid(), Guid.NewGuid());
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(r => r.FindAsync<ModuleAggregate>(request.Id, userContextMock.Object.Tenant, cancellationToken))
            .ReturnsAsync((ModuleAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.ModuleNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.ModuleNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_RemovesServiceAndPublishesEvents()
    {
        // Arrange
        var request = new RemoveServiceCommand(Guid.NewGuid(), service.Id);
        var cancellationToken = CancellationToken.None;
        var module = ModuleAggregate.Create(request.Id, "TestModule", "TestDescription", [service], Guid.NewGuid());

        repositoryMock
            .Setup(r => r.FindAsync<ModuleAggregate>(request.Id, userContextMock.Object.Tenant, cancellationToken))
            .ReturnsAsync(module);

        userContextMock
            .SetupGet(u => u.IdUser)
            .Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.UpdateAsync(module, cancellationToken), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<ServiceRemovedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
