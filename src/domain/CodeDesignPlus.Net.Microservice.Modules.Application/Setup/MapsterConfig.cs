using CodeDesignPlus.Microservice.Api.Dtos;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.CreateModule;
using CodeDesignPlus.Net.Microservice.Modules.Application.Module.Commands.UpdateModule;
using CodeDesignPlus.Net.Microservice.Modules.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Modules.Application.Setup;

public static class MapsterConfigModule
{
    public static void Configure()
    {
        //Module
        TypeAdapterConfig<CreateModuleDto, CreateModuleCommand>.NewConfig();
        TypeAdapterConfig<UpdateModuleDto, UpdateModuleCommand>.NewConfig();
        TypeAdapterConfig<ModuleAggregate, ModuleDto>.NewConfig();

        //Service
        TypeAdapterConfig<ServiceEntity, ServiceDto>.NewConfig().TwoWays();

    }
}
