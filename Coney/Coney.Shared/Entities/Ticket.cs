using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class Ticket
{
    public int Id { get; set; }

    [Required]
    public string TicketNumber { get; set; } = null!;

    public Riffle? Riffle { get; set; }

    [Required]
    public int RiffleId { get; set; }

    public User? User { get; set; }

    public int? UserId { get; set; }
}