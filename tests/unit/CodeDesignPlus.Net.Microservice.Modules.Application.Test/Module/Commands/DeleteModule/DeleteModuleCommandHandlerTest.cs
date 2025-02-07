using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.DeleteModule;
using CodeDesignPlus.Net.Microservice.Modules.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Test.Module.Commands.DeleteModule;

public class DeleteModuleCommandHandlerTest
{
    private readonly Mock<IModuleRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly DeleteModuleCommandHandler handler;

    public DeleteModuleCommandHandlerTest()
    {
        repositoryMock = new Mock<IModuleRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new DeleteModuleCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        DeleteModuleCommand request = null;
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
        var request = new DeleteModuleCommand(Guid.NewGuid());
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(r => r.FindAsync<ModuleAggregate>(request.Id, cancellationToken))
            .ReturnsAsync((ModuleAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.ModuleNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.ModuleNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesModuleAndPublishesEvents()
    {
        // Arrange
        var request = new DeleteModuleCommand(Guid.NewGuid());
        var cancellationToken = CancellationToken.None;
        var moduleAggregate = ModuleAggregate.Create(Guid.NewGuid(), "Test Module", "Test Description", [], Guid.NewGuid());

        repositoryMock
            .Setup(r => r.FindAsync<ModuleAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(moduleAggregate);

        userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.DeleteAsync<ModuleAggregate>(moduleAggregate.Id, cancellationToken), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<ModuleDeletedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
