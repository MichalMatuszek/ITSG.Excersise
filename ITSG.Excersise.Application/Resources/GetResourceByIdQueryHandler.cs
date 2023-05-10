using ITSG.Excersise.Application.Dtos;
using ITSG.Excersise.Domain.Resources;
using ITSG.Excersise.Domain.Users;
using MediatR;

namespace ITSG.Excersise.Application.Resources
{
    public class GetResourceByIdQueryHandler : IRequestHandler<GetResourceByIdQuery, ResourceDetailsDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IResourceRepository _resourceRepository;

        public GetResourceByIdQueryHandler(IUserRepository userRepository, IResourceRepository resourceRepository)
        {
            _userRepository = userRepository;
            _resourceRepository = resourceRepository;
        }

        public async Task<ResourceDetailsDto> Handle(GetResourceByIdQuery request, CancellationToken cancellationToken)
        {
            var resource = await _resourceRepository.GetByIdAsync(request.Id);

            if (resource == null)
                return null;

            string userName = string.Empty;
            if(resource.LockedBy.HasValue)
            {
                userName = await _userRepository.GetUserLoginByIdAsync(resource.LockedBy.Value);
            }

            return new ResourceDetailsDto
            {
                Id = resource.Id,
                IsDeleted = resource.IsDeleted,
                Version = resource.Version,
                Name = resource.Name,
                LockedTo = resource.LockedTo,
                LockedByUserName = userName
            };
        }
    }
}
