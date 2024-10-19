using System.ComponentModel.DataAnnotations;

namespace Coney.Shared.Entities;

public class Ticket
{
    public int Id { get; set; }

    public string TicketNumber { get; set; } = null!;

    public Riffle? Riffle { get; set; }

    public int? RiffleId { get; set; }

    public User? User { get; set; }

    public int? UserId { get; set; }
}