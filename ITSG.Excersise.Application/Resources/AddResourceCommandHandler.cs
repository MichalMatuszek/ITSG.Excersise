using ITSG.Excersise.Domain.Resources;
using MediatR;

namespace ITSG.Excersise.Application.Resources
{
    public class AddResourceCommandHandler : IRequestHandler<AddResourceCommand, Unit>
    {
        private readonly IResourceRepository _resourceRepository;

        public AddResourceCommandHandler(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task<Unit> Handle(AddResourceCommand request, CancellationToken cancellationToken)
        {
            await _resourceRepository.AddAsync(new Resource
            {
                Name = request.Name,
                IsDeleted = false
            });

            return Unit.Value;
        }
    }
}
