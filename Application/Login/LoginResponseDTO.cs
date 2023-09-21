namespace Application.Login;
public record class LoginResponseDTO(UserDTO User, string Token);