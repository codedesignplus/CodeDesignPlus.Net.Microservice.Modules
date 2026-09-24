using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Modules.Infrastructure;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("300", "UnknownError");
}
