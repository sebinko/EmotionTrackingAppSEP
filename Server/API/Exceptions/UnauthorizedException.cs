namespace API.Exceptions;

public class UnauthorizedException(string message) : Exception(message)
{
}