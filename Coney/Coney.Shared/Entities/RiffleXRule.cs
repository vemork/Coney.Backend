using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class RiffleXRule
{
    public int Id { get; set; }

    public Riffle? Riffle { get; set; }

    public int? RiffleId { get; set; }

    public Rule? Rule { get; set; }

    public int? RuleId { get; set; }
}