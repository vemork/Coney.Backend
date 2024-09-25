using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class Notification
{
    public int Id { get; set; }

    [MaxLength(255)]
    [Required]
    public string Title { get; set; } = null!;

    [MaxLength(100)]
    public string Description { get; set; } = string.Empty!;

    public DateTime CreatedDate { get; set; }
}