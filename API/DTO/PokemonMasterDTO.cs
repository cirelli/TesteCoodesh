using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class PokemonMasterDTO
    {
        [Required, MaxLength(255)]
        public string? Name { get; set; }

        [Required]
        public int? Age { get; set; }

        [Required, MaxLength(14)]
        public string? CPF { get; set; }
    }
}
