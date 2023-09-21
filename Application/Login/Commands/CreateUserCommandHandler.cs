using Application.Exceptions;
using AutoMapper;
using Domain.Role;
using Domain.User;
using MediatR;

namespace Application.Login.Commands;
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    public CreateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userRepository.GetUserByNameAsync(request.UserName);
        
        if(userExists is not null)
        {
            throw new UserAlreadyExistsException(nameof(ApplicationUser), request.UserName);
        }

        ApplicationUser user = new()
        {
            UserName = request.UserName,
            Email = request.UserName,
            NormalizedEmail = request.UserName.ToUpper(),
            FullName = request.FullName
        };

        var result = await _userRepository.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            if (!_roleRepository.RoleExistsAsync(request.Role).GetAwaiter().GetResult())
            {
                await _roleRepository.CreateAsync(request.Role);
            }
            await _userRepository.AddToRoleAsync(user, request.Role);
            var userToReturn = await _userRepository.GetUserByNameAsync(request.UserName);                 
            return _mapper.Map<UserDTO>(userToReturn);
        }
        else
        {
            var errors = result.Errors.SelectMany(x => x.Description).ToList();
            throw new ErrorRegisteringUserException(errors);
        }
    }
}
