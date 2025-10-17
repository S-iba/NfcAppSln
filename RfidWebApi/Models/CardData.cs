using System.ComponentModel.DataAnnotations;

namespace RfidWebApi.Models
{
    public class CardData
    {
        [Key]
        public int Id { get; set; }
        public string Uid { get; set; } = string.Empty;

        public string? Owner { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
