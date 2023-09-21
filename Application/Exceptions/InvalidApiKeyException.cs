namespace Application.Exceptions;
public class InvalidApiKeyException : Exception
{
    public InvalidApiKeyException() : base("Unauthorized. Invalid API key")
    {
    }
}