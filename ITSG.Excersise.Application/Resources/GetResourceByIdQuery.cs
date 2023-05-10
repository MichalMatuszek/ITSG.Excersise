using ITSG.Excersise.Application.Dtos;
using ITSG.Excersise.Domain.Resources;
using MediatR;

namespace ITSG.Excersise.Application.Resources
{
    public class GetResourceByIdQuery : IRequest<ResourceDetailsDto>
    {
        public long Id { get; }

        public GetResourceByIdQuery(long id)
        {
            Id = id;
        }
    }
}
