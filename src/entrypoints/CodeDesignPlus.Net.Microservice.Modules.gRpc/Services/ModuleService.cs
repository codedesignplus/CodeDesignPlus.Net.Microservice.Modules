using CodeDesignPlus.Net.Exceptions.Guards;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.AddService;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.CreateModule;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.DeleteModule;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.RemoveService;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.UpdateModule;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Queries.GetAllModule;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Queries.GetModuleById;
using Google.Protobuf.WellKnownTypes;
using C = CodeDesignPlus.Net.Core.Abstractions.Models.Criteria;

namespace CodeDesignPlus.Net.Microservice.Modules.gRpc.Services;

public class ModuleService(IMediator mediator, IMapper mapper) : Module.ModuleBase
{
    private static readonly Guid SystemUserId = Guid.Parse("10000000-0000-0000-0000-000000000001");

    public override async Task<Empty> CreateModule(CreateModuleRequest request, ServerCallContext context)
    {
        DomainGuard.IsFalse(Guid.TryParse(request.Id, out var id), "Invalid module ID");

        var services = request.Services.Select(s => new ServiceDto
        {
            Id = Guid.TryParse(s.Id, out var sid) ? sid : Guid.NewGuid(),
            Name = s.Name,
            Controller = s.Controller,
            Action = s.Action,
            HttpMethod = System.Enum.TryParse<Domain.Enums.HttpMethod>(s.HttpMethod, true, out var method) ? method : Domain.Enums.HttpMethod.None
        }).ToList();

        var command = new CreateModuleCommand(id, request.Name, request.Description, services, SystemUserId);

        await mediator.Send(command, context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> UpdateModule(UpdateModuleRequest request, ServerCallContext context)
    {
        DomainGuard.IsFalse(Guid.TryParse(request.Id, out var id), "Invalid module ID");

        var services = request.Services.Select(s => new ServiceDto
        {
            Id = Guid.TryParse(s.Id, out var sid) ? sid : Guid.NewGuid(),
            Name = s.Name,
            Controller = s.Controller,
            Action = s.Action,
            HttpMethod = System.Enum.TryParse<Domain.Enums.HttpMethod>(s.HttpMethod, true, out var method) ? method : Domain.Enums.HttpMethod.None
        }).ToList();

        var command = new UpdateModuleCommand(id, request.Name, request.Description, services, request.IsActive, SystemUserId);

        await mediator.Send(command, context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> DeleteModule(DeleteModuleRequest request, ServerCallContext context)
    {
        DomainGuard.IsFalse(Guid.TryParse(request.Id, out var id), "Invalid module ID");

        await mediator.Send(new DeleteModuleCommand(id, SystemUserId), context.CancellationToken);

        return new Empty();
    }

    public override async Task<GetModuleResponse> GetModuleById(GetModuleByIdRequest request, ServerCallContext context)
    {
        DomainGuard.IsFalse(Guid.TryParse(request.Id, out var id), "Invalid module ID");

        var module = await mediator.Send(new GetModuleByIdQuery(id), context.CancellationToken);

        return MapToResponse(module);
    }

    public override async Task<GetAllModulesResponse> GetAllModules(GetAllModulesRequest request, ServerCallContext context)
    {
        var criteria = new C.Criteria
        {
            Filters = string.IsNullOrEmpty(request.Filters) ? null : request.Filters,
            Limit = request.Limit > 0 ? request.Limit : 100
        };

        var result = await mediator.Send(new GetAllModuleQuery(criteria), context.CancellationToken);

        var response = new GetAllModulesResponse { TotalCount = result.TotalCount };
        response.Modules.AddRange(result.Data.Select(MapToResponse));

        return response;
    }

    public override async Task<Empty> AddService(AddServiceRequest request, ServerCallContext context)
    {
        DomainGuard.IsFalse(Guid.TryParse(request.ModuleId, out var moduleId), "Invalid module ID");
        DomainGuard.IsFalse(Guid.TryParse(request.ServiceId, out var serviceId), "Invalid service ID");

        var httpMethod = System.Enum.TryParse<Domain.Enums.HttpMethod>(request.HttpMethod, true, out var method)
            ? method : Domain.Enums.HttpMethod.None;

        var command = new AddServiceCommand(moduleId, serviceId, request.Name, request.Controller, request.Action, httpMethod, SystemUserId);

        await mediator.Send(command, context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> RemoveService(RemoveServiceRequest request, ServerCallContext context)
    {
        DomainGuard.IsFalse(Guid.TryParse(request.ModuleId, out var moduleId), "Invalid module ID");
        DomainGuard.IsFalse(Guid.TryParse(request.ServiceId, out var serviceId), "Invalid service ID");

        await mediator.Send(new RemoveServiceCommand(moduleId, serviceId, SystemUserId), context.CancellationToken);

        return new Empty();
    }

    private static GetModuleResponse MapToResponse(ModuleDto module)
    {
        var response = new GetModuleResponse
        {
            Id = module.Id.ToString(),
            Name = module.Name,
            Description = module.Description,
            IsActive = module.IsActive
        };

        response.Services.AddRange(module.Services.Select(s => new ServiceMessage
        {
            Id = s.Id.ToString(),
            Name = s.Name,
            Controller = s.Controller,
            Action = s.Action,
            HttpMethod = s.HttpMethod.ToString()
        }));

        return response;
    }
}
