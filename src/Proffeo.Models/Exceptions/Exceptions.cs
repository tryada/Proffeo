namespace Proffeo.Models.Exceptions;

public class ValidationException : Exception
{
    private ValidationException(Type type, string field, string reason) 
        : base($"Validation error in {type.Name} - field '{field}': {reason}")
    {
    }

    public static ValidationException Create<T>(string field, string reason)
        => new(typeof(T), field, reason);
}

public class NotFoundException : Exception
{
    private NotFoundException(Type type, string key, string value) 
        : base($"Resource of type {type.Name} not found for {key} = {value}")
    {
    }

    public static NotFoundException Create<T>(string key, string value) 
        => new(typeof(T), key, value);
}