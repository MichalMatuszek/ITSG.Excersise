using ITSG.Excersise.Domain.Resources;
using MediatR;

namespace ITSG.Excersise.Application.Resources
{
    public class UnlockResourceCommandHandler : IRequestHandler<UnlockResourceCommand, LockResult>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IResourceRepository _resourceRepository;

        public UnlockResourceCommandHandler(ICurrentUserProvider currentUserProvider, IResourceRepository resourceRepository)
        {
            _currentUserProvider = currentUserProvider;
            _resourceRepository = resourceRepository;
        }

        public async Task<LockResult> Handle(UnlockResourceCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _currentUserProvider.GetCurrentUserId();

            if (!currentUser.HasValue)
                return LockResult.NotAllowed;

            var result = await _resourceRepository.UnlockAsync(request.ResourceId, currentUser.Value, request.Version);

            return result;
        }
    }
}
