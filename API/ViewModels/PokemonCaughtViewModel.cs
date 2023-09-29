using System.Text.Json.Serialization;
using PokeApiWrapper.Models;

namespace API.ViewModels
{
    public class PokemonCaughtViewModel
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        [JsonIgnore]
        public int PokemonId { get; set; }

        public PokemonBase Pokemon { get; set; } = new();
    }
}
