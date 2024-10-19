using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class UserXRiffle
{
    public int Id { get; set; }

    public Riffle? Riffle { get; set; }

    public int? RiffleId { get; set; }

    public User? User { get; set; }

    public int? UserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}