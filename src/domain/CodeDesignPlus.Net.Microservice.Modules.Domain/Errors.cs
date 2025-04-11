namespace CodeDesignPlus.Net.Microservice.Modules.Domain;

public class Errors : IErrorCodes
{
    public const string UnknownError = "100 : UnknownError";

    public const string IdModuleIsInvalid = "101 : The id of the module is invalid.";
    public const string NameModuleIsInvalid = "102 : The name of the module is invalid.";
    public const string DescriptionModuleIsInvalid = "103 : The description of the module is invalid.";
    public const string IdUserIsInvalid = "104 : The id of the user is invalid.";
    public const string NameServiceIsInvalid = "105 : The name of the service is invalid.";
    public const string ControllerServiceIsInvalid = "106 : The controller of the service is invalid.";
    public const string ActionServiceIsInvalid = "107 : The action of the service is invalid.";
    public const string ServiceNotFound = "108 : The service was not found.";
    public const string IdServiceIsInvalid = "109 : The id of the service is invalid.";
    public const string HttpMethodServiceIsInvalid = "110 : The http method of the service is invalid.";
}
