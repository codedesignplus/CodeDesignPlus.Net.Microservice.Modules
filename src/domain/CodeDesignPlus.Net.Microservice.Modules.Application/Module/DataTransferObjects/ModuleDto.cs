namespace CodeDesignPlus.Net.Microservice.Modules.Application.Module.DataTransferObjects;

public class ModuleDto : IDtoBase
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<ServiceDto> Services { get; set; } = [];
    public bool IsActive { get; set; }
}