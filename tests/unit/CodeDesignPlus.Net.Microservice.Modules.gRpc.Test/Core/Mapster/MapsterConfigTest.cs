namespace CodeDesignPlus.Net.Microservice.Modules.gRpc.Test.Core.Mapster;

public class MapsterConfigTest
{
    // El servicio gRPC arma sus mensajes a mano (Services/ModuleService.cs), asi que MapsterConfig no registra
    // reglas propias. Lo que se exige es que el arranque lo pueda llamar sin fallar y deje un mapper usable; la
    // prueba anterior pedia reglas registradas y solo pasaba si otra prueba habia llenado GlobalSettings antes.
    [Fact]
    public void Configure_CanBeCalledAtStartup_AndLeavesAUsableMapper()
    {
        // Act
        var exception = Record.Exception(CodeDesignPlus.Net.Microservice.Modules.gRpc.Core.Mapster.MapsterConfig.Configure);
        var mapper = new Mapper(TypeAdapterConfig.GlobalSettings);

        // Assert
        Assert.Null(exception);
        Assert.NotNull(mapper);
    }
}
