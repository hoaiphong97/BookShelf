using System.ComponentModel.DataAnnotations;

namespace Domains.Entities
{
    public interface IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public short Status {  get; set; }
        public bool IsDeleted {  get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? LastUpdatedDate {  get; set; }
    }
}
