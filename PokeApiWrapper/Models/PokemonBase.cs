using System.Text.Json.Serialization;

namespace PokeApiWrapper.Models
{
    public class PokemonBase
    {
        [JsonPropertyOrder(-2)]
        public int Id { get; set; }

        [JsonPropertyOrder(-1)]
        public string? Name { get; set; }

        [JsonPropertyOrder(1)]
        public string? Sprite { get; set; }
    }
}
