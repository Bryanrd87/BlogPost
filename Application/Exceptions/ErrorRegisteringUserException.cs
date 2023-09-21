namespace Application.Exceptions;
public class ErrorRegisteringUserException : Exception
{
    public ErrorRegisteringUserException(List<char> error) : base("Error registering user." + string.Join("", error))
    {        
    }
}
