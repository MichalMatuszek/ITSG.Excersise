namespace ITSG.Excersise.Application.Dtos
{
    public class UserDetailsDto
    {
        public long UserId { get; set; }
        public string Login { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}