using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class PokemonCaughtDTO
    {
        [Required]
        public int? PokemonId { get; set; }

        public DateOnly? Date { get; set; }
    }
}
