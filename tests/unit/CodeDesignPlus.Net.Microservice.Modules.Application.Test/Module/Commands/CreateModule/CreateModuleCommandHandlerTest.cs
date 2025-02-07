using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.CreateModule;
using CodeDesignPlus.Net.Microservice.Modules.Domain.DomainEvents;
using CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Test.Module.Commands.CreateModule; 

    public class CreateModuleCommandHandlerTest
    {
        private readonly Mock<IModuleRepository> repositoryMock;
        private readonly Mock<IUserContext> userContextMock;
        private readonly Mock<IPubSub> pubSubMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly CreateModuleCommandHandler handler;

        public CreateModuleCommandHandlerTest()
        {
            repositoryMock = new Mock<IModuleRepository>();
            userContextMock = new Mock<IUserContext>();
            pubSubMock = new Mock<IPubSub>();
            mapperMock = new Mock<IMapper>();

            handler = new CreateModuleCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
        {
            // Arrange
            CreateModuleCommand request = null!;
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

            Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
            Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }

        [Fact]
        public async Task Handle_ModuleAlreadyExists_ThrowsCodeDesignPlusException()
        {
            // Arrange
            var request = new CreateModuleCommand(Guid.NewGuid(), "Test Module", "Test Description", []);
            var cancellationToken = CancellationToken.None;

            repositoryMock.Setup(x => x.ExistsAsync<ModuleAggregate>(request.Id, cancellationToken)).ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

            Assert.Equal(Errors.ModuleAlreadyExists.GetMessage(), exception.Message);
            Assert.Equal(Errors.ModuleAlreadyExists.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }

        [Fact]
        public async Task Handle_ValidRequest_CreatesModuleAndPublishesEvents()
        {
            // Arrange
            var request = new CreateModuleCommand(Guid.NewGuid(), "Test Module", "Test Description", []);
            var cancellationToken = CancellationToken.None;
            var userId = Guid.NewGuid();

            repositoryMock.Setup(x => x.ExistsAsync<ModuleAggregate>(request.Id, cancellationToken)).ReturnsAsync(false);
            userContextMock.Setup(x => x.IdUser).Returns(userId);
            mapperMock.Setup(x => x.Map<List<ServiceEntity>>(request.Services)).Returns([]);

            // Act
            await handler.Handle(request, cancellationToken);

            // Assert
            repositoryMock.Verify(x => x.CreateAsync(It.IsAny<ModuleAggregate>(), cancellationToken), Times.Once);
            pubSubMock.Verify(x => x.PublishAsync(It.IsAny<List<ModuleCreatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
        }
    }
