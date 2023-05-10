using ITSG.Excersise.Domain.Resources;
using MediatR;

namespace ITSG.Excersise.Application.Resources
{
    public class DeleteResourceCommandHandler : IRequestHandler<DeleteResourceCommand, bool>
    {

        private IResourceRepository _resourceRepository;

        public DeleteResourceCommandHandler(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task<bool> Handle(DeleteResourceCommand request, CancellationToken cancellationToken)
        {
            var result = await _resourceRepository.RemoveAsync(request.Id);

            return result;
        }
    }
}
