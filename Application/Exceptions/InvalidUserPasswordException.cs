namespace Application.Exceptions;
public class InvalidUserPasswordException : Exception
{
    public InvalidUserPasswordException() : base("Invalid user or password.")
    {        
    }
}