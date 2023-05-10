using ITSG.Excersise.Domain.Resources;
using MediatR;

namespace ITSG.Excersise.Application.Resources
{
    public class LockResourceCommand : IRequest<LockResult>
    {
        public LockResourceCommand(long resourceId, int minutes, byte[] version)
        {
            ResourceId = resourceId;
            Minutes = minutes;
            Version = version;
        }

        public long ResourceId { get; }
        public int Minutes { get; }
        public byte[] Version { get; }
    }
}
