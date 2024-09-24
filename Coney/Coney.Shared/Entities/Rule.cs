using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class Rule
{
    public int Id { get; set; }

    [MaxLength(255)]
    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public DateTimeOffset InitDate { get; set; }

    [Required]
    public DateTimeOffset EndtDate { get; set; }

    public bool Status { get; set; } = false;
}