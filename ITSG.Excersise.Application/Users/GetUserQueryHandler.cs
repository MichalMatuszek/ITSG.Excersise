using ITSG.Excersise.Application.Dtos;
using ITSG.Excersise.Domain.Users;
using MediatR;

namespace ITSG.Excersise.Application.Users
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDetailsDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public GetUserQueryHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDetailsDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = await _userRepository.GetByLoginAsync(request.Login);

            if (user == null)
                return null;

            if (!_passwordHasher.Check(user.PasswordHash, request.Password))
                return null;

            return new UserDetailsDto
            {
                Login = user.Login,
                UserId = user.Id,
                Roles = user.Roles
            };
        }
    }
}
