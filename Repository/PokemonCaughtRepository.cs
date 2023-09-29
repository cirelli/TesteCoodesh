using AutoMapper;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class PokemonCaughtRepository : RepositoryBase<PokemonCaught>
    {
        public PokemonCaughtRepository(DataContext repositoryContext, IMapper mapper)
            : base(repositoryContext, mapper)
        {
        }

        private IQueryable<PokemonCaught> GetByMasterQuery(int masterId)
            => FindByCondition(q => q.PokemonMasterId == masterId);

        public async Task<List<RModel>> GetByMasterAsync<RModel>(int masterId, CancellationToken cancellationToken)
            => await ToListAsync<RModel>(GetByMasterQuery(masterId), cancellationToken);
    }
}
