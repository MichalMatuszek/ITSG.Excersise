using ITSG.Excersise.Domain.Resources;
using MediatR;

namespace ITSG.Excersise.Application.Resources
{
    public class LockResourcePermanentCommand : IRequest<LockResult>
    {
        public LockResourcePermanentCommand(long resourceId, byte[] version)
        {
            ResourceId = resourceId;
            Version = version;
        }

        public long ResourceId { get; }
        public byte[] Version { get; }
    }
}
