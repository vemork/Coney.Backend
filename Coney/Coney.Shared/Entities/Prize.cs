using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class Prize
{
    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string Name { get; set; } = null!;

    [MaxLength(255)]
    [Required]
    public string Description { get; set; } = null!;

    public int Value { get; set; }

    public DateTimeOffset? DeliveredDate { get; set; }

    public bool Delivered { get; set; } = false;
}