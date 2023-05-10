namespace ITSG.Excersise.Domain.Users
{
    public interface IUserRepository
    {
        Task<User> GetByLoginAsync(string login);
        Task<string> GetUserLoginByIdAsync(long id);
    }
}
