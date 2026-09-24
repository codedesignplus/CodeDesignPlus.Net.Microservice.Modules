using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Modules.Domain;

public class Errors : IErrorCodes
{
    public static readonly Error UnknownError = new("100", "UnknownError");

    public static readonly Error IdModuleIsInvalid = new("101", "The id of the module is invalid.");
    public static readonly Error NameModuleIsInvalid = new("102", "The name of the module is invalid.");
    public static readonly Error DescriptionModuleIsInvalid = new("103", "The description of the module is invalid.");
    public static readonly Error IdUserIsInvalid = new("104", "The id of the user is invalid.");
    public static readonly Error NameServiceIsInvalid = new("105", "The name of the service is invalid.");
    public static readonly Error ControllerServiceIsInvalid = new("106", "The controller of the service is invalid.");
    public static readonly Error ActionServiceIsInvalid = new("107", "The action of the service is invalid.");
    public static readonly Error ServiceNotFound = new("108", "The service was not found.");
    public static readonly Error IdServiceIsInvalid = new("109", "The id of the service is invalid.");
    public static readonly Error HttpMethodServiceIsInvalid = new("110", "The http method of the service is invalid.");
}
