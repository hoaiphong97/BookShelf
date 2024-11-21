namespace Domains.Entities
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public short ReadingStatus { get; set; }
    }
}
