namespace ITSG.Excersise.Application.Dtos
{
    public class LockResourceDto
    {
        public long ResourceId { get; set; }
        public bool IsPermanent { get; set; }
        public byte[] Version { get; set; }
    }
}
