using System;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Modules.Rest.Test.Controllers;

public class ModuleControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    private readonly System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions()
    {
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
    }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

    private readonly ServiceDto service = new()
    {
        Id = Guid.NewGuid(),
        Name = "ms-test",
        Controller = "CustomController",
        Action = "CustomAction"
    };

    public ModuleControllerTest(Server<Program> server) : base(server)
    {
        server.InMemoryCollection = (x) =>
        {
            x.Add("Vault:Enable", "false");
            x.Add("Vault:Address", "http://localhost:8200");
            x.Add("Vault:Token", "root");
            x.Add("Solution", "CodeDesignPlus");
            x.Add("AppName", "my-test");
            x.Add("RabbitMQ:UserName", "guest");
            x.Add("RabbitMQ:Password", "guest");
            x.Add("Security:ValidAudiences:0", Guid.NewGuid().ToString());
        };
    }

    [Fact]
    public async Task GetModules_ReturnOk()
    {
        var module = await this.CreateModuleAsync();

        var response = await this.RequestAsync("http://localhost/api/Module", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var modules = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ModuleDto>>(json, this.options);

        Assert.NotNull(modules);
        Assert.NotEmpty(modules);
        Assert.Contains(modules, x => x.Id == module.Id);
    }

    [Fact]
    public async Task GetModuleById_ReturnOk()
    {
        var moduleCreated = await this.CreateModuleAsync();

        var response = await this.RequestAsync($"http://localhost/api/Module/{moduleCreated.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var module = System.Text.Json.JsonSerializer.Deserialize<ModuleDto>(json, this.options);

        Assert.NotNull(module);
        Assert.Equal(moduleCreated.Id, module.Id);
        Assert.Equal(moduleCreated.Name, module.Name);
        Assert.Equal(moduleCreated.Description, module.Description);
        Assert.Contains(moduleCreated.Services, x => 
            x.Id == service.Id
            && x.Name == service.Name
            && x.Controller == service.Controller
            && x.Action == service.Action
        );
    }

    [Fact]
    public async Task CreateModule_ReturnNoContent()
    {
        var data = new CreateModuleDto()
        {
            Id = Guid.NewGuid(),
            Name = "Module Test",
            Description = "Module Test Description",
            Services = [service]
        };

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync("http://localhost/api/Module", content, HttpMethod.Post);

        var module = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, module.Id);
        Assert.Equal(data.Name, module.Name);
        Assert.Equal(data.Description, module.Description);
        Assert.Contains(data.Services, x => 
            x.Id == service.Id
            && x.Name == service.Name
            && x.Controller == service.Controller
            && x.Action == service.Action
        );
    }

    [Fact]
    public async Task UpdateModule_ReturnNoContent()
    {
        var moduleCreated = await this.CreateModuleAsync();

        var data = new UpdateModuleDto()
        {
            Id = moduleCreated.Id,
            Name = "Module Test Updated",
            Description = "Module Test Description Updated",
            Services = [service]
        };

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Module/{moduleCreated.Id}", content, HttpMethod.Put);

        var module = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, module.Id);
        Assert.Equal(data.Name, module.Name);
        Assert.Equal(data.Description, module.Description);
        Assert.Contains(data.Services, x => 
            x.Id == service.Id
            && x.Name == service.Name
            && x.Controller == service.Controller
            && x.Action == service.Action
        );
    }

    [Fact]
    public async Task DeleteModule_ReturnNoContent()
    {
        var moduleCreated = await this.CreateModuleAsync();

        var response = await this.RequestAsync($"http://localhost/api/Module/{moduleCreated.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private async Task<CreateModuleDto> CreateModuleAsync()
    {
        var data = new CreateModuleDto()
        {
            Id = Guid.NewGuid(),
            Name = "Module Test",
            Description = "Module Test Description",
            Services = [service]
        };

        var json = System.Text.Json.JsonSerializer.Serialize(data, this.options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await this.RequestAsync("http://localhost/api/Module", content, HttpMethod.Post);

        return data;
    }

    private async Task<ModuleDto> GetRecordAsync(Guid id)
    {
        var response = await this.RequestAsync($"http://localhost/api/Module/{id}", null, HttpMethod.Get);

        var json = await response.Content.ReadAsStringAsync();

        return System.Text.Json.JsonSerializer.Deserialize<ModuleDto>(json, this.options)!;
    }

    private async Task<HttpResponseMessage> RequestAsync(string uri, HttpContent? content, HttpMethod method)
    {
        var httpRequestMessage = new HttpRequestMessage()
        {
            RequestUri = new Uri(uri),
            Content = content,
            Method = method
        };
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("TestAuth");

        var response = await Client.SendAsync(httpRequestMessage);

        if (!response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            throw new Exception(data);
        }

        return response;
    }

}
