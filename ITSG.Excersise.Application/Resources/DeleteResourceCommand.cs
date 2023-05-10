using MediatR;

namespace ITSG.Excersise.Application.Resources
{
    public class DeleteResourceCommand : IRequest<bool>
    {
        public long Id { get; }

        public DeleteResourceCommand(long id)
        {
            Id = id;
        }
    }
}
