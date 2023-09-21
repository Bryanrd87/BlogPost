using Application.Blog.Commands.Create;
using Application.Blog.Commands.Delete;
using Application.Blog.Commands.Update;
using Application.Blog.Queries.GetBlogPostDetails;
using Application.Blog.Queries.GetBlogPosts;
using Carter;
using MediatR;
using System.Security.Claims;

namespace BlogPostAPI.Endpoints
{
    public sealed class BlogPost : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapGet("blogpost/{blogPostId}", async (Guid blogPostId, ISender sender) =>
            {
                var query = new GetBlogPostDetailsQuery(blogPostId);

                return Results.Ok(await sender.Send(query));

            }).RequireAuthorization();

            app.MapGet("blogpost", async (ISender sender) =>
            {
                var query = new GetBlogPostsQuery();

                return Results.Ok(await sender.Send(query));

            }).RequireAuthorization();

            app.MapPost("blogpost", async (CreateBlogPostRequest request, ISender sender, HttpContext context) =>
            {
                var userIdClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid);

                if (userIdClaim is null)
                {
                    return Results.Unauthorized();
                }

                var userId = userIdClaim.Value;

                await sender.Send(new CreateBlogPostCommand(request.Title, request.Content, userId));

                return Results.Ok();

            }).RequireAuthorization();

            app.MapPut("blogpost", async (UpdateBlogPostRequest request, ISender sender) =>
            {
                await sender.Send(new UpdateBlogPostCommand(request.BlogPostId, request.Title, request.Content));

                return Results.NoContent();

            }).RequireAuthorization();

            app.MapDelete("blogpost/{blogPostId}", async (Guid blogPostId, ISender sender) =>
            {
                await sender.Send(new DeleteBlogPostCommand(blogPostId));

                return Results.NoContent();

            }).RequireAuthorization();
        }
    }
}
