using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class City
{
    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string Name { get; set; } = null!;

    public State? State { get; set; }

    public int? StateId { get; set; }
}