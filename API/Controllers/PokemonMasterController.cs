using API.DTO;
using API.ViewModels;
using AutoMapper;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using PokeApiWrapper;
using PokeApiWrapper.Models;
using Repository;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonMasterController : ControllerBase
    {
        private readonly RepositoryWrapper repository;
        private readonly IMapper mapper;
        private readonly PokeWrapper pokeWrapper;

        public PokemonMasterController(RepositoryWrapper repository, IMapper mapper, PokeWrapper pokeWrapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.pokeWrapper = pokeWrapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<PokemonMasterViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await repository.PokemonMaster.GetAllAsync<PokemonMasterViewModel>(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}", Name = "PokemonMasterById")]
        public async Task<ActionResult<PokemonMasterViewModel>> GetById(int id, CancellationToken cancellationToken)
        {
            var entity = await repository.PokemonMaster.GetByIdAsync<PokemonMasterViewModel>(id, cancellationToken);

            if (entity is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(entity);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PokemonMasterViewModel>> Create([FromBody] PokemonMasterDTO pokemonMaster, CancellationToken cancellationToken)
        {
            if (pokemonMaster is null)
            {
                return BadRequest("Pokémon Master is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = mapper.Map<PokemonMaster>(pokemonMaster);
            repository.PokemonMaster.Create(entity);
            await repository.SaveAsync(cancellationToken);

            var result = mapper.Map<PokemonMasterViewModel>(entity);
            return CreatedAtRoute("PokemonMasterById", new { id = entity.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PokemonMasterDTO pokemonMaster, CancellationToken cancellationToken)
        {
            if (pokemonMaster is null)
            {
                return BadRequest("Pokémon Master is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await repository.PokemonMaster.GetByIdAsync(id, cancellationToken);
            if (entity is null)
            {
                return NotFound();
            }

            mapper.Map(pokemonMaster, entity);
            repository.PokemonMaster.Update(entity);
            _ = repository.SaveAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            if (!await repository.PokemonMaster.ExistsAsync(id, cancellationToken))
            {
                return NotFound();
            }

            repository.PokemonMaster.Delete(new() { Id = id});
            _ = repository.SaveAsync(cancellationToken);

            return NoContent();
        }

        [HttpPost("{id}/pokemon-caught")]
        public async Task<IActionResult> CatchPokemon(int id, PokemonCaughtDTO pokemonCaught, CancellationToken cancellationToken)
        {
            if (pokemonCaught is null)
            {
                return BadRequest("Pokémon Caught is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await repository.PokemonMaster.ExistsAsync(id, cancellationToken))
            {
                return NotFound();
            }

            try
            {
                await pokeWrapper.Pokemon.GetAsync(pokemonCaught.PokemonId!.Value, cancellationToken);
            }
            catch (PokemonNotFoundException)
            {
                return NotFound();
            }

            var entity = mapper.Map<PokemonCaught>(pokemonCaught);
            entity.PokemonMasterId = id;
            repository.PokemonCaught.Create(entity);
            _ = repository.SaveAsync(cancellationToken);

            return NoContent();
        }

        [HttpGet("{id}/pokemon-caught")]
        public async Task<ActionResult<List<Pokemon>>> PokemonCaught(int id, CancellationToken cancellationToken)
        {
            if (!await repository.PokemonMaster.ExistsAsync(id, cancellationToken))
            {
                return NotFound();
            }

            List<PokemonCaughtViewModel> pokemons = await repository.PokemonCaught.GetByMasterAsync<PokemonCaughtViewModel>(id, cancellationToken);
            foreach (var item in pokemons)
            {
                var pokemon = await pokeWrapper.Pokemon.GetBaseAsync(item.PokemonId, cancellationToken);
                item.Pokemon = pokemon;
            }

            return Ok(pokemons);
        }
    }
}
