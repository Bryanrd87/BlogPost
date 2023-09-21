using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Login.Queries;
public record GetLoginDetailsQuery([Required] string UserName, [Required, Display(Name = "Password"), DataType(DataType.Password)] string Password) : IRequest<LoginResponseDTO>;