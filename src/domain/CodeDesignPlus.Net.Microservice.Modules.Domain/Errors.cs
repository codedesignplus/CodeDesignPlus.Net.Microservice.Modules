using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Modules.Domain;

public class Errors : IErrorCodes
{
    public static readonly Error UnknownError = new("100");

    public static readonly Error IdModuleIsInvalid = new("101");
    public static readonly Error NameModuleIsInvalid = new("102");
    public static readonly Error DescriptionModuleIsInvalid = new("103");
    public static readonly Error IdUserIsInvalid = new("104");
    public static readonly Error NameServiceIsInvalid = new("105");
    public static readonly Error ControllerServiceIsInvalid = new("106");
    public static readonly Error ActionServiceIsInvalid = new("107");
    public static readonly Error ServiceNotFound = new("108");
    public static readonly Error IdServiceIsInvalid = new("109");
    public static readonly Error HttpMethodServiceIsInvalid = new("110");
}
