using ITSG.Excersise.Application.Dtos;
using MediatR;

namespace ITSG.Excersise.Application.Users
{
    public class GetUserQuery: IRequest<UserDetailsDto?>
    {
        public GetUserQuery(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; }
        public string Password { get;}
    }
}
