namespace CodeDesignPlus.Net.Microservice.Modules.Domain;

public class ModuleAggregate(Guid id) : AggregateRoot(id)
{
    public static ModuleAggregate Create(Guid id, Guid tenant, Guid createBy)
    {
       return default;
    }
}
