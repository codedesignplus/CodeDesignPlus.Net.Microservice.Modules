using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Modules.Application;

public class Errors : IErrorCodes
{
    public static readonly Error UnknownError = new("200", "UnknownError");
    public static readonly Error InvalidRequest = new("201", "The request is invalid");
    public static readonly Error ModuleAlreadyExists = new("202", "The module already exists");
    public static readonly Error ModuleNotFound = new("203", "The module not found");

}
