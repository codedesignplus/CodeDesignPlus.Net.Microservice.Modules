namespace CodeDesignPlus.Net.Microservice.Modules.Application;

public class Errors : IErrorCodes
{
    public const string UnknownError = "200 : UnknownError";
    public const string InvalidRequest = "201 : The request is invalid";
    public const string ModuleAlreadyExists = "202 : The module already exists";
    public const string ModuleNotFound = "203 : The module not found";

}
