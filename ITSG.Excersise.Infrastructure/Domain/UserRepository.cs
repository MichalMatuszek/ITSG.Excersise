using Dapper;
using ITSG.Excersise.Domain.Users;

namespace ITSG.Excersise.Infrastructure.Domain
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public UserRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User> GetByLoginAsync(string login)
        {
            using var connection = _connectionFactory.CreateConnection();

            var dict = new Dictionary<long, User>();

            var user = (await connection.QueryAsync<User, string, User>(
                "SELECT u.[Id], u.[Login], u.[PasswordHash], ur.[Name] FROM Users u LEFT JOIN UserRoles ur ON ur.UserId = u.Id WHERE [Login] = @Login",
                (user, role) =>
                {
                    if (!dict.TryGetValue(user.Id, out User entity))
                    {
                        entity = user;
                        entity.Roles = new List<string>();
                        dict.Add(entity.Id, entity);
                    }

                    if (role != null) entity.Roles.Add(role);
                    entity.Roles = entity.Roles.Distinct().ToList();

                    return entity;
                },
                new {Login = login}, splitOn: "Name"
            )).FirstOrDefault();

            return user;
        }

        public async Task<string> GetUserLoginByIdAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var result = await connection.QueryFirstOrDefaultAsync<string>("SELECT [Login] FROM Users WHERE Id = @Id"
                , new { Id = id });

            return result;
        }
    }
}
