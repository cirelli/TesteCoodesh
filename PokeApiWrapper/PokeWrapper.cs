using PokeApiNet;

namespace PokeApiWrapper
{
    public class PokeWrapper
    {
        private readonly PokeApiClient pokeApiClient;
        private PokemonWrapper? pokemon;

        public PokeWrapper(PokeApiClient pokeApiClient)
        {
            this.pokeApiClient = pokeApiClient;
        }

        public PokemonWrapper Pokemon
        {
            get
            {
                pokemon ??= new(pokeApiClient);
                return pokemon;
            }
        }
    }
}
