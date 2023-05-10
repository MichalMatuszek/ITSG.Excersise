namespace ITSG.Excersise.Domain.Resources
{
    public class Resource
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? LockedBy { get; set; }
        public DateTimeOffset? LockedTo { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] Version { get; set; }
    }
}
