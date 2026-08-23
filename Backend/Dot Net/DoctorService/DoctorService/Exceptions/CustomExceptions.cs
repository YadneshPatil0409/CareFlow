namespace DoctorService.Exceptions;

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(string message) : base(message) { }
}

public class DuplicateResourceException : Exception
{
    public DuplicateResourceException(string message) : base(message) { }
}