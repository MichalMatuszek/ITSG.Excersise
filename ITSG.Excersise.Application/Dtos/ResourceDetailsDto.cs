namespace ITSG.Excersise.Application.Dtos
{
    public class ResourceDetailsDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LockedByUserName { get; set; }
        public DateTimeOffset? LockedTo { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] Version { get; set; }
    }
}
