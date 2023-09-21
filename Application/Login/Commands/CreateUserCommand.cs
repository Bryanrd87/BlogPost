using MediatR;

namespace Application.Login.Commands;
public record CreateUserCommand(string UserName, string FullName, string Password, string Role): IRequest<UserDTO>;
public record CreateUserRequest(string UserName, string FullName, string Password, string Role);

