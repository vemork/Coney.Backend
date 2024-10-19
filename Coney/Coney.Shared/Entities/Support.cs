using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities
{
    public class Support
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = null!;

        public bool IsResolved { get; set; }

        [Required]
        public string PersonInCharge { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ResolvedAt { get; set; }

        public User? User { get; set; }

        public int? UserId { get; set; }
    }
}