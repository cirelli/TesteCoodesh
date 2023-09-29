using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class PokemonCaught
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int PokemonMasterId { get; set; }

        [Required]
        public int PokemonId { get; set; }

        [Required]
        public DateOnly Date { get; set; }


        public PokemonMaster? PokemonMaster { get; set; }
    }
}
