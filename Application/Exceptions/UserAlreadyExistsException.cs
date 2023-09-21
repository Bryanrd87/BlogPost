namespace Application.Exceptions;
public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string name, object key) : base($"{name} ({key}) already exists.")
    {        
    }
}
