using System.ComponentModel.DataAnnotations;

namespace API.ViewModels
{
    public class PokemonMasterViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Age { get; set; }

        [Required]
        public string CPF { get; set; } = string.Empty;
    }
}
