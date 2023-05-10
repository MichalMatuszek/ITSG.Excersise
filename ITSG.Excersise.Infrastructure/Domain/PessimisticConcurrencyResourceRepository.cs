using Dapper;
using ITSG.Excersise.Domain.Resources;

namespace ITSG.Excersise.Infrastructure.Domain
{
    public class PessimisticConcurrencyResourceRepository : BaseRepository, IResourceRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public PessimisticConcurrencyResourceRepository(ISqlConnectionFactory connectionFactory)
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

            using(var trans = connection.BeginTransaction())
            {
                var resource = await connection.QueryFirstOrDefaultAsync<Resource>(
                    "SELECT * FROM [Resources] WITH (XLOCK) WHERE Id = @Id", new { Id = id }, trans);


                if (resource == null)
                    return LockResult.NotFound;

                if (!CheckRule(new UserCanLockOnlyHisResourceOrNonLockedRule(resource, userId)))
                    return LockResult.NotAllowed;

                await connection.ExecuteAsync(
                    "UPDATE [Resources] SET [LockedBy] = @LockedBy, [LockedTo] = @LockedTo WHERE Id = @Id",
                    new
                    {
                        LockedBy = userId,
                        LockedTo = lockedTo,
                        Id = id,
                    }, trans);

                trans.Commit();

                return LockResult.Success;

            }
        }

        public async Task<Resource?> GetByIdAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var result = await connection.QueryFirstOrDefaultAsync<Resource>(
                "SELECT * FROM [Resources] WHERE Id = @Id", new { Id = id });

            return result;
        }

        public async Task<bool> RemoveAsync(long id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var rowsUpdated =
                await connection.ExecuteAsync("UPDATE [dbo].[Resources] SET [IsDeleted] = 1 WHERE Id = @Id"
                , new { Id = id });

            return rowsUpdated == 1;
        }

        public async Task<LockResult> UnlockAsync(long id, long userId, byte[] version)
        {
            var connection = _connectionFactory.CreateConnection();

            using (var trans = connection.BeginTransaction())
            {
                var resource = await connection.QueryFirstOrDefaultAsync<Resource>(
                    "SELECT * FROM [Resources] WITH (XLOCK) WHERE Id = @Id", new { Id = id }, trans);


                if (resource == null)
                    return LockResult.NotFound;

                if (!CheckRule(new UserCanUnlockOnlyHisResourceRule(resource, userId)))
                    return LockResult.NotAllowed;

                await connection.ExecuteAsync(
                    "UPDATE [Resources] SET [LockedBy] = NULL, [LockedTo] = NULL WHERE Id = @Id",
                    new
                    {
                        LockedBy = userId,
                        Id = id,
                    }, trans);

                trans.Commit();

                return LockResult.Success;

            }
        }
    }
}
