using ITSG.Excersise.Domain.Resources;
using MediatR;

namespace ITSG.Excersise.Application.Resources
{
    public class UnlockResourceCommand : IRequest<LockResult>
    {
        public UnlockResourceCommand(long resourceId, byte[] version)
        {
            ResourceId = resourceId;
            Version = version;
        }

        public long ResourceId { get; }
        public byte[] Version { get; }
    }
}
