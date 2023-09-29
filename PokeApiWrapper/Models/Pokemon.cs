using System.Text.Json.Serialization;

namespace PokeApiWrapper.Models
{
    public class Pokemon : PokemonBase
    {
        public Pokemon()
        {
            
        }

        public Pokemon(PokemonBase pokemonBase)
        {
            Id = pokemonBase.Id;
            Name = pokemonBase.Name;
            Sprite = pokemonBase.Sprite;
        }

        public int Height { get; set; }

        public int Weight { get; set; }

        public List<PokemonType> Types { get; set; } = new();

        [JsonPropertyOrder(2)]
        public List<PokemonBase> Evolutions { get; set; } = new();
    }
}
