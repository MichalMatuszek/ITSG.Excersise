using Dapper;
using ITSG.Excersise.Domain.Resources;

namespace ITSG.Excersise.Infrastructure.Domain
{
    public class OptimisticConcurrencyResourceRepository : BaseRepository, IResourceRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public OptimisticConcurrencyResourceRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddAsync(Resource resource)
        {
            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync("INSERT INTO [Resources] ([Name]) VALUES (@Name)"
                , new { resource.Name });
        }

        public async Task<LockResult> LockAsync(long id, long userId, DateTimeOffset lockedTo, byte[] version)
        {
            using var connection = _connectionFactory.CreateConnection();

            var existingResource = await connection.QueryFirstOrDefaultAsync<Resource>(
                "SELECT * FROM [Resources] WHERE Id = @Id AND [IsDeleted] = 0", new { Id = id });

            if (existingResource == null)
                return LockResult.NotFound;

            if (!CheckRule(new UserCanLockOnlyHisResourceOrNonLockedRule(existingResource, userId)))
                return LockResult.NotAllowed;

            if(Convert.ToBase64String(existingResource.Version) != Convert.ToBase64String(version))
            {
                return LockResult.Conflicted;
            }

            var updatedRows = await connection.ExecuteAsync(
                @"UPDATE [Resources] SET [LockedBy] = @LockedBy, [LockedTo] = @LockedTo WHERE Id = @Id AND Version = @Version"
                , new
                {
                    LockedBy = userId,
                    LockedTo = lockedTo,
                    Id = id,
                    Version = version
                });

            var isSuccess = updatedRows == 1;

            return isSuccess ? LockResult.Success : LockResult.Conflicted;
        }


        public async Task<Resource?> GetByIdAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var result = await connection.QueryFirstOrDefaultAsync<Resource>(
                "SELECT * FROM [Resources] WHERE Id = @Id", new {Id = id});

            return result;
        }

        public async Task<bool> RemoveAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var rowsUpdated = 
                await connection.ExecuteAsync("UPDATE [Resources] SET [IsDeleted] = 1 WHERE Id = @Id"
                ,new {Id = id});

            return rowsUpdated == 1;
        }

        public async Task<LockResult> UnlockAsync(long id, long userId, byte[] version)
        {
            var connection = _connectionFactory.CreateConnection();

            var existingResource = await connection.QueryFirstOrDefaultAsync<Resource>(
                "SELECT * FROM [Resources] WHERE Id = @Id AND [IsDeleted] = 0", new { Id = id });

            if (existingResource == null)
                return LockResult.NotFound;

            if (!CheckRule(new UserCanUnlockOnlyHisResourceRule(existingResource, userId)))
                return LockResult.NotAllowed;

            if (Convert.ToBase64String(existingResource.Version) != Convert.ToBase64String(version))
            {
                return LockResult.Conflicted;
            }

            var updatedRows = await connection.ExecuteAsync(
                @"UPDATE [Resources] SET [LockedBy] = NULL, [LockedTo] = NULL WHERE Id = @Id AND Version = @Version"
                , new
                {
                    Id = id,
                    Version = version
                });

            var isSuccess = updatedRows == 1;

            return isSuccess ? LockResult.Success : LockResult.Conflicted;

        }
    }
}
