using PokeApiNet;

namespace PokeApiWrapper
{
    public abstract class WrapperBase
    {
        protected readonly PokeApiClient pokeApiClient;

        internal WrapperBase(PokeApiClient pokeApiClient)
        {
            this.pokeApiClient = pokeApiClient;
        }
    }
}
