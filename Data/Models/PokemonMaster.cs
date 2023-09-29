using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class PokemonMaster
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Age { get; set; }

        [Required, MaxLength(14)]
        public string CPF { get; set; } = string.Empty;


        public List<PokemonCaught> PokemonCaughts { get; set; } = new();
    }
}
