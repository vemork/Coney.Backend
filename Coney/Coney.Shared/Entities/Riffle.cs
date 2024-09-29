using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class Riffle
{
    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string Name { get; set; } = null!;

    [MaxLength(255)]
    public string? Description { get; set; }

    [Required]
    public DateTimeOffset InitDate { get; set; }

    [Required]
    public DateTimeOffset EndtDate { get; set; }
}