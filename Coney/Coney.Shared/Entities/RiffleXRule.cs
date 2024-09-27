using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class RiffleXRule
{
    public int Id { get; set; }

    public Riffle? Riffle { get; set; }

    [Required]
    public int RiffleId { get; set; }

    public Rule? Rule { get; set; }

    [Required]
    public int RuleId { get; set; }
}