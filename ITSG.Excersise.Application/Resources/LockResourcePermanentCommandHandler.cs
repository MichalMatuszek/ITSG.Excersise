using ITSG.Excersise.Domain.Resources;
using MediatR;

namespace ITSG.Excersise.Application.Resources
{
    public class LockResourcePermanentCommandHandler : IRequestHandler<LockResourcePermanentCommand, LockResult>
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly ICurrentUserProvider _currentUserProvider;

        public LockResourcePermanentCommandHandler(IResourceRepository resourceRepository, ICurrentUserProvider currentUserProvider)
        {
            _resourceRepository = resourceRepository;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<LockResult> Handle(LockResourcePermanentCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _currentUserProvider.GetCurrentUserId();

            if (!currentUser.HasValue)
                return LockResult.NotAllowed;

            var result = await _resourceRepository.LockAsync(request.ResourceId, currentUser.Value, DateTime.MaxValue, request.Version);

            return result;
        }
    }
}
