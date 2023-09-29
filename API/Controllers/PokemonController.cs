using Microsoft.AspNetCore.Mvc;
using PokeApiWrapper;
using PokeApiWrapper.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly PokeWrapper pokeWrapper;

        public PokemonController(PokeWrapper pokeWrapper)
        {
            this.pokeWrapper = pokeWrapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Pokemon>> GetId(int id, CancellationToken cancellationToken)
        {
            try
            {
                var pokemon = await pokeWrapper.Pokemon.GetAsync(id, cancellationToken);

                return Ok(pokemon);
            }
            catch (PokemonNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Pokemon>> Get(string name, CancellationToken cancellationToken)
        {
            try
            {
                var pokemon = await pokeWrapper.Pokemon.GetAsync(name, cancellationToken);

                return Ok(pokemon);
            }
            catch (PokemonNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("random10")]
        public async Task<ActionResult<List<Pokemon>>> GetRnd10(CancellationToken cancellationToken)
        {
            List<Pokemon> pokemons = new();
            Random rnd = new();

            for (int i = 1; i <= 10; i++)
            {
                int pokemonId = rnd.Next(1, 151);

                pokemons.Add(await pokeWrapper.Pokemon.GetAsync(pokemonId, cancellationToken));
            }

            return Ok(pokemons);
        }
    }
}
