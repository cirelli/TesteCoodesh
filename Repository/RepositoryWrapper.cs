using AutoMapper;
using Data;

namespace Repository
{
    public class RepositoryWrapper
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public RepositoryWrapper(DataContext repositoryContext, IMapper mapper)
        {
            dataContext = repositoryContext;
            this.mapper = mapper;
        }

        private PokemonMasterRepository? pokemonMaster;
        public PokemonMasterRepository PokemonMaster
        {
            get
            {
                pokemonMaster ??= new PokemonMasterRepository(dataContext, mapper);
                return pokemonMaster;
            }
        }

        private PokemonCaughtRepository? pokemonCaught;
        public PokemonCaughtRepository PokemonCaught
        {
            get
            {
                pokemonCaught ??= new PokemonCaughtRepository(dataContext,mapper);
                return pokemonCaught;
            }
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
