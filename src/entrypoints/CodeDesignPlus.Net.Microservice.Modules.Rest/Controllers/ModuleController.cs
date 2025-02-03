namespace CodeDesignPlus.Net.Microservice.Modules.Rest.Controllers;

/// <summary>
/// Controller for managing the Modules.
/// </summary>
/// <param name="mediator">Mediator instance for sending commands and queries.</param>
/// <param name="mapper">Mapper instance for mapping between DTOs and commands/queries.</param>
[Route("api/[controller]")]
[ApiController]
public class ModuleController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get all Modules.
    /// </summary>
    /// <param name="criteria">Criteria for filtering and sorting the Modules.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of Modules.</returns>
    [HttpGet]
    public async Task<IActionResult> GetModules([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllModuleQuery(criteria), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get a Module by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Module.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Module.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetModuleById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetModuleByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Create a new Module.
    /// </summary>
    /// <param name="data">Data for creating the Module.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost]
    public async Task<IActionResult> CreateModule([FromBody] CreateModuleDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<CreateModuleCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing Module.
    /// </summary>
    /// <param name="id">The unique identifier of the Module.</param>
    /// <param name="data">Data for updating the Module.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateModule(Guid id, [FromBody] UpdateModuleDto data, CancellationToken cancellationToken)
    {
        data.Id = id;

        await mediator.Send(mapper.Map<UpdateModuleCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Delete an existing Module.
    /// </summary>
    /// <param name="id">The unique identifier of the Module.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteModule(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteModuleCommand(id), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Add a new Service to a Module.
    /// </summary>
    /// <param name="id">The unique identifier of the Module.</param>
    /// <param name="data">Data for adding the Service.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost("{id}/services")]
    public async Task<IActionResult> AddService(Guid id, [FromBody] AddServiceDto data, CancellationToken cancellationToken)
    {
        data.Id = id;

        await mediator.Send(mapper.Map<AddServiceCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Remove a Service from a Module.
    /// </summary>
    /// <param name="id">The unique identifier of the Module.</param>
    /// <param name="serviceId">The unique identifier of the Service.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}/services/{serviceId}")]
    public async Task<IActionResult> RemoveService(Guid id, Guid serviceId, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveServiceCommand(id, serviceId), cancellationToken);

        return NoContent();
    }
}