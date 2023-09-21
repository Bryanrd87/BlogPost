using Application.Login;
using Application.Login.Commands;
using Application.Login.Queries;
using Carter;
using MediatR;

namespace BlogPostAPI.Endpoints;
public sealed class Login : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapPost("login/login", async (GetLoginDetailsQuery request, ISender sender) =>
        {
            var query = new GetLoginDetailsQuery(request.UserName, request.Password);

            return Results.Ok(await sender.Send(query));

        }).AllowAnonymous();

        app.MapPost("login/register", async (CreateUserCommand request, ISender sender) =>
        {
            var command =  await sender.Send(new CreateUserCommand(request.UserName, request.FullName, request.Password, request.Role));

            return Results.Ok(command);

        }).AllowAnonymous();      
    }
}

