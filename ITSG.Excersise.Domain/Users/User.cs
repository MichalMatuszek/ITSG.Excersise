namespace ITSG.Excersise.Domain.Users
{
    public class User
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public IList<string> Roles { get; set; }
    }
}
