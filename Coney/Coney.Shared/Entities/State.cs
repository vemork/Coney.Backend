using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class State
{
    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string Name { get; set; } = null!;

    public Country? Country { get; set; }

    public int? CountryId { get; set; }
}