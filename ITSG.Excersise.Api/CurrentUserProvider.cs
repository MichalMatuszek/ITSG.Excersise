using ITSG.Excersise.Application;
using System.Security.Claims;

namespace ITSG.Excersise.Api
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public long? GetCurrentUserId()
        {
            var id = _contextAccessor?.HttpContext?.User?.FindFirstValue("Id");

            if (string.IsNullOrEmpty(id))
                return null;

            return long.Parse(id);
        }
    }
}
