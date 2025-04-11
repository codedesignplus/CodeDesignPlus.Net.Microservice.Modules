using System;
using System.Collections.Generic;
using CodeDesignPlus.Net.Microservice.Modules.Domain;
using CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Modules.Domain.Test;

public class ModuleAggregateTest
{
    [Fact]
    public void Create_ValidParameters_ShouldCreateModuleAggregate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Module";
        var description = "Test Description";
        var services = new List<ServiceEntity>();
        var createdBy = Guid.NewGuid();

        // Act
        var module = ModuleAggregate.Create(id, name, description, services, createdBy);

        // Assert
        Assert.NotNull(module);
        Assert.Equal(id, module.Id);
        Assert.Equal(name, module.Name);
        Assert.Equal(description, module.Description);
        Assert.Equal(services, module.Services);
        Assert.True(module.IsActive);
    }

    [Fact]
    public void Update_ValidParameters_ShouldUpdateModuleAggregate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Module";
        var description = "Test Description";
        var services = new List<ServiceEntity>();
        var createdBy = Guid.NewGuid();
        var module = ModuleAggregate.Create(id, name, description, services, createdBy);

        var newName = "Updated Module";
        var newDescription = "Updated Description";
        var newServices = new List<ServiceEntity>();
        var updatedBy = Guid.NewGuid();

        // Act
        module.Update(newName, newDescription, newServices, true, updatedBy);

        // Assert
        Assert.Equal(newName, module.Name);
        Assert.Equal(newDescription, module.Description);
        Assert.Equal(newServices, module.Services);
    }

    [Fact]
    public void Delete_ValidParameters_ShouldDeactivateModuleAggregate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Module";
        var description = "Test Description";
        var services = new List<ServiceEntity>();
        var createdBy = Guid.NewGuid();
        var module = ModuleAggregate.Create(id, name, description, services, createdBy);

        var deletedBy = Guid.NewGuid();

        // Act
        module.Delete(deletedBy);

        // Assert
        Assert.False(module.IsActive);
    }

    [Fact]
    public void AddService_ValidParameters_ShouldAddServiceToModuleAggregate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Module";
        var description = "Test Description";
        var services = new List<ServiceEntity>();
        var createdBy = Guid.NewGuid();
        var module = ModuleAggregate.Create(id, name, description, services, createdBy);

        var serviceId = Guid.NewGuid();
        var serviceName = "Test Service";
        var controller = "TestController";
        var action = "TestAction";
        var addedBy = Guid.NewGuid();
        var httpMethod = Enums.HttpMethod.GET;

        // Act
        module.AddService(serviceId, serviceName, controller, action,  httpMethod, addedBy);

        // Assert
        Assert.Contains(module.Services, s => s.Id == serviceId && s.Name == serviceName && s.Controller == controller && s.Action == action && s.HttpMethod == httpMethod);
    }

    [Fact]
    public void RemoveService_ValidParameters_ShouldRemoveServiceFromModuleAggregate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Module";
        var description = "Test Description";
        var services = new List<ServiceEntity>();
        var createdBy = Guid.NewGuid();
        var module = ModuleAggregate.Create(id, name, description, services, createdBy);

        var serviceId = Guid.NewGuid();
        var serviceName = "Test Service";
        var controller = "TestController";
        var action = "TestAction";
        var addedBy = Guid.NewGuid();
        var httpMethod = Enums.HttpMethod.GET;
        
        module.AddService(serviceId, serviceName, controller, action, httpMethod, addedBy);

        var removedBy = Guid.NewGuid();

        // Act
        module.RemoveService(serviceId, removedBy);

        // Assert
        Assert.DoesNotContain(module.Services, s => s.Id == serviceId);
    }
}
