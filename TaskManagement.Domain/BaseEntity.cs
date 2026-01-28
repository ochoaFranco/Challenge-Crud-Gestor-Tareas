using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Domain
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;
    }
}
