using ITSG.Excersise.Domain.Resources;
using MediatR;

namespace ITSG.Excersise.Application.Resources
{
    public class LockResourceCommandHandler : IRequestHandler<LockResourceCommand, LockResult>
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly ICurrentUserProvider _currentUserProvider;

        public LockResourceCommandHandler(IResourceRepository resourceRepository, ICurrentUserProvider currentUserProvider)
        {
            _resourceRepository = resourceRepository;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<LockResult> Handle(LockResourceCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _currentUserProvider.GetCurrentUserId();

            if (!currentUser.HasValue)
                return LockResult.NotAllowed;

            var blockTo = DateTime.Now.AddMinutes(request.Minutes);

            var result = await _resourceRepository.LockAsync(request.ResourceId, currentUser.Value, blockTo, request.Version);

            return result;
        }
    }
}
