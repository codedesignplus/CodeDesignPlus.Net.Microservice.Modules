namespace CodeDesignPlus.Net.Microservice.Modules.Rest.Controllers;

/// <summary>
/// Controller for managing the Modules.
/// </summary>
/// <param name="mediator">Mediator instance for sending commands and queries.</param>
/// <param name="mapper">Mapper instance for mapping between DTOs and commands/queries.</param>
/// <param name="user">User context for the current request.</param>
[Route("api/[controller]")]
[ApiController]
public class ModuleController(IMediator mediator, IMapper mapper, IUserContext user) : ControllerBase
{
    /// <summary>
    /// Get all Modules.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetModules([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllModuleQuery(criteria), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get a Module by its ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetModuleById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetModuleByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new Module.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateModule([FromBody] CreateModuleDto data, CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateModuleCommand>(data) with { ActorId = user.IdUser };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Update an existing Module.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateModule(Guid id, [FromBody] UpdateModuleDto data, CancellationToken cancellationToken)
    {
        data.Id = id;
        var command = mapper.Map<UpdateModuleCommand>(data) with { ActorId = user.IdUser };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Delete an existing Module.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteModule(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteModuleCommand(id, user.IdUser), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Add a new Service to a Module.
    /// </summary>
    [HttpPost("{id}/services")]
    public async Task<IActionResult> AddService(Guid id, [FromBody] AddServiceDto data, CancellationToken cancellationToken)
    {
        data.Id = id;
        var command = mapper.Map<AddServiceCommand>(data) with { ActorId = user.IdUser };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Remove a Service from a Module.
    /// </summary>
    [HttpDelete("{id}/services/{serviceId}")]
    public async Task<IActionResult> RemoveService(Guid id, Guid serviceId, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveServiceCommand(id, serviceId, user.IdUser), cancellationToken);
        return NoContent();
    }
}
