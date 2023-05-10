using ITSG.Excersise.Domain.Resources;
using ITSG.Excersise.Domain.Users;

namespace ITSG.Excersise.Domain
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IResourceRepository ResourceRepository { get; }
        Task CommitAsync();
    }
}
