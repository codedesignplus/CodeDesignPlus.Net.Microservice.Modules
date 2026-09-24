using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Modules.Application;

public class Errors : IErrorCodes
{
    public static readonly Error UnknownError = new("200");
    public static readonly Error InvalidRequest = new("201");
    public static readonly Error ModuleAlreadyExists = new("202");
    public static readonly Error ModuleNotFound = new("203");

}
