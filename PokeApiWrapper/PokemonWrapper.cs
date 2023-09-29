using PokeApiNet;

namespace PokeApiWrapper
{
    public class PokemonWrapper : WrapperBase
    {
        internal PokemonWrapper(PokeApiClient pokeApiClient)
            : base(pokeApiClient)
        {
            
        }

        public async Task<Models.Pokemon> GetAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                Pokemon pokemon = await pokeApiClient.GetResourceAsync<Pokemon>(id, cancellationToken);

                return await GetAsync(pokemon, cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new PokemonNotFoundException();
                }

                throw;
            }
        }

        public async Task<Models.Pokemon> GetAsync(string name, CancellationToken cancellationToken)
        {
            try
            {
                Pokemon pokemon = await pokeApiClient.GetResourceAsync<Pokemon>(name, cancellationToken);

                return await GetAsync(pokemon, cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new PokemonNotFoundException();
                }

                throw;
            }
        }

        public async Task<Models.PokemonBase> GetBaseAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                Pokemon pokemon = await pokeApiClient.GetResourceAsync<Pokemon>(id, cancellationToken);

                return await GetBaseAsync(pokemon, cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new PokemonNotFoundException();
                }

                throw;
            }
        }

        private async Task<Models.Pokemon> GetAsync(Pokemon pokemon, CancellationToken cancellationToken)
        {
            Models.Pokemon pokemonModel = await ConvertToModelAsync(pokemon, cancellationToken);
            pokemonModel.Evolutions = await GetEvolutionsAsync(pokemon, cancellationToken);

            return pokemonModel;
        }

        private static async Task<Models.PokemonBase> GetBaseAsync(Pokemon pokemon, CancellationToken cancellationToken)
        {
            Models.PokemonBase pokemonModel = await ConvertToModelBaseAsync(pokemon, cancellationToken);

            return pokemonModel;
        }

        private async Task<List<Models.PokemonBase>> GetEvolutionsAsync(Pokemon pokemon, CancellationToken cancellationToken)
        {
            PokemonSpecies species = await pokeApiClient.GetResourceAsync(pokemon.Species, cancellationToken);
            EvolutionChain speciesEvolutionChain = await pokeApiClient.GetResourceAsync(species.EvolutionChain, cancellationToken);

            ChainLink pokemonEvolutionChain = FindSpeciesInEvolutionChain(species, speciesEvolutionChain.Chain)!;

            List<Models.PokemonBase> evolutions = new();
            foreach (var item in pokemonEvolutionChain.EvolvesTo)
            {
                PokemonSpecies evolutionSpecies = await pokeApiClient.GetResourceAsync(item.Species, cancellationToken);
                Pokemon evolutionPokemon = await pokeApiClient.GetResourceAsync(evolutionSpecies.Varieties.First(v => v.IsDefault).Pokemon, cancellationToken);

                evolutions.Add(await ConvertToModelBaseAsync(evolutionPokemon, cancellationToken));
            }

            return evolutions;
        }

        private static ChainLink? FindSpeciesInEvolutionChain(PokemonSpecies species, ChainLink chain)
        {
            if (chain.Species.Name == species.Name)
            {
                return chain;
            }

            foreach (ChainLink evolution in chain.EvolvesTo)
            {
                var item = FindSpeciesInEvolutionChain(species, evolution);
                if (item != null)
                {
                    return item;
                }
            }

            return null;
        }

        private static async Task<Models.PokemonBase> ConvertToModelBaseAsync(Pokemon pokemon, CancellationToken cancellationToken)
        {
            var model = new Models.PokemonBase
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                Sprite = await ImageToBase64Async(pokemon.Sprites.FrontDefault, cancellationToken)
            };

            return model;
        }

        private static async Task<Models.Pokemon> ConvertToModelAsync(Pokemon pokemon, CancellationToken cancellationToken)
        {
            Models.Pokemon model = new(await ConvertToModelBaseAsync(pokemon, cancellationToken))
            {
                Height = pokemon.Height,
                Weight = pokemon.Weight,
                Types = pokemon.Types.ConvertAll(t => new Models.PokemonType { Name = t.Type.Name })
            };

            return model;
        }

        private static async Task<string> ImageToBase64Async(string url, CancellationToken cancellationToken)
        {
            byte[] bytes = await GetImageFromUrlAsync(url, cancellationToken);
            return Convert.ToBase64String(bytes);
        }

        private static async Task<byte[]> GetImageFromUrlAsync(string url, CancellationToken cancellationToken)
        {
            using HttpClient client = new();
            return await client.GetByteArrayAsync(url, cancellationToken);
        }
    }
}