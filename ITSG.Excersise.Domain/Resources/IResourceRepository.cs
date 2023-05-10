namespace ITSG.Excersise.Domain.Resources
{
    public interface IResourceRepository
    {
        Task<Resource?> GetByIdAsync(long id);

        Task AddAsync(Resource resource);

        Task<bool> RemoveAsync(long id);

        Task<LockResult> LockAsync(long id, long userId, DateTimeOffset lockedTo, byte[] version);

        Task<LockResult> UnlockAsync(long id, long userId, byte[] version);
    }
}
