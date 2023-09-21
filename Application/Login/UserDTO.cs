namespace Application.Login;
public record UserDTO
{
    public string ID { get; init; }
    public string UserName { get; init; }
    public string FullName { get; init; }
}