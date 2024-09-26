using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Coney.Shared.Entities;

public class Country
{
    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string Name { get; set; } = null!;

    [JsonIgnore]
    public ICollection<State>? States { get; set; }

    public int StatesCount => States == null ? 0 : States.Count;
}