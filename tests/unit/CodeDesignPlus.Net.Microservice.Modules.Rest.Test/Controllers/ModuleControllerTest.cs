using CodeDesignPlus.Microservice.Api.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.AddService;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.CreateModule;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.UpdateModule;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Queries.GetAllModule;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Queries.GetModuleById;
using CodeDesignPlus.Net.Microservice.Modules.Rest.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.DeleteModule;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.RemoveService;

namespace CodeDesignPlus.Net.Microservice.Modules.Rest.Test.Controllers
{
    public class ModuleControllerTest
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly ModuleController controller;

        public ModuleControllerTest()
        {
            mediatorMock = new Mock<IMediator>();
            mapperMock = new Mock<IMapper>();
            controller = new ModuleController(mediatorMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task GetModules_ReturnsOkResult()
        {
            // Arrange
            var criteria = new C.Criteria();
            var cancellationToken = new CancellationToken();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllModuleQuery>(), cancellationToken))
                .ReturnsAsync([]);

            // Act
            var result = await controller.GetModules(criteria, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<ModuleDto>>(okResult.Value);
            mediatorMock.Verify(m => m.Send(It.IsAny<GetAllModuleQuery>(), cancellationToken), Times.Once);
        }

        [Fact]
        public async Task GetModuleById_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetModuleByIdQuery>(), cancellationToken))
                .ReturnsAsync(new ModuleDto(){
                    Id = id,
                    Name = "Module Test",
                    Description = "Module Test Description",
                    Services = []
                });

            // Act
            var result = await controller.GetModuleById(id, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<ModuleDto>(okResult.Value);
            mediatorMock.Verify(m => m.Send(It.IsAny<GetModuleByIdQuery>(), cancellationToken), Times.Once);
        }

        [Fact]
        public async Task CreateModule_ReturnsNoContentResult()
        {
            // Arrange
            var data = new CreateModuleDto();
            var cancellationToken = new CancellationToken();
            mapperMock
                .Setup(m => m.Map<CreateModuleCommand>(data))
                .Returns(new CreateModuleCommand(Guid.NewGuid(), data.Name, data.Description, []));

            // Act
            var result = await controller.CreateModule(data, cancellationToken);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mediatorMock.Verify(m => m.Send(It.IsAny<CreateModuleCommand>(), cancellationToken), Times.Once);
        }

        [Fact]
        public async Task UpdateModule_ReturnsNoContentResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var data = new UpdateModuleDto();
            var cancellationToken = new CancellationToken();
            mapperMock
                .Setup(m => m.Map<UpdateModuleCommand>(data))
                .Returns(new UpdateModuleCommand(Guid.NewGuid(), data.Name, data.Description, [], false));

            // Act
            var result = await controller.UpdateModule(id, data, cancellationToken);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mediatorMock.Verify(m => m.Send(It.IsAny<UpdateModuleCommand>(), cancellationToken), Times.Once);
        }

        [Fact]
        public async Task DeleteModule_ReturnsNoContentResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            // Act
            var result = await controller.DeleteModule(id, cancellationToken);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mediatorMock.Verify(m => m.Send(It.IsAny<DeleteModuleCommand>(), cancellationToken), Times.Once);
        }

        [Fact]
        public async Task AddService_ReturnsNoContentResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var data = new AddServiceDto();
            var cancellationToken = new CancellationToken();
            mapperMock
                .Setup(m => m.Map<AddServiceCommand>(data))
                .Returns(new AddServiceCommand(id, Guid.NewGuid(), data.Name, data.Controller, data.Action));

            // Act
            var result = await controller.AddService(id, data, cancellationToken);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mediatorMock.Verify(m => m.Send(It.IsAny<AddServiceCommand>(), cancellationToken), Times.Once);
        }

        [Fact]
        public async Task RemoveService_ReturnsNoContentResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var serviceId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            // Act
            var result = await controller.RemoveService(id, serviceId, cancellationToken);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mediatorMock.Verify(m => m.Send(It.IsAny<RemoveServiceCommand>(), cancellationToken), Times.Once);
        }
    }
}
