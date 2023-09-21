using Application.Exceptions;
using AutoMapper;
using Domain.User;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Login.Queries;
public class GetLoginDetailsRequestHandler : IRequestHandler<GetLoginDetailsQuery, LoginResponseDTO>
{
    private readonly IUserRepository _userRepository; 
    private readonly IMapper _mapper;
    private readonly string _secretKey;   
    private readonly IConfiguration _config;
    public GetLoginDetailsRequestHandler(IConfiguration configuration,IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;       
        _mapper = mapper;
        _config = configuration;
    }
    public async Task<LoginResponseDTO> Handle(GetLoginDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByNameAsync(request.UserName);

        bool isValid = await _userRepository.CheckPasswordAsync(user, request.Password);

        if (user is null || !isValid)
        {
            throw new InvalidUserPasswordException();
        }

        var roles = await _userRepository.GetRolesByUserAsync(user);       

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.GivenName, user.FullName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(720),
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return new(_mapper.Map<UserDTO>(user), jwt); ;
    }
}
