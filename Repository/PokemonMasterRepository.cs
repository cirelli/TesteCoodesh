using AutoMapper;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class PokemonMasterRepository : RepositoryBase<PokemonMaster>
    {
        public PokemonMasterRepository(DataContext repositoryContext, IMapper mapper)
            : base(repositoryContext, mapper)
        {
        }


        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
            => await GetByIdQuery(id).AnyAsync(cancellationToken);


        private IQueryable<PokemonMaster> GetAllQuery()
            => FindAll().OrderBy(q => q.Name);

        public async Task<List<RModel>> GetAllAsync<RModel>(CancellationToken cancellationToken)
            => await ToListAsync<RModel>(GetAllQuery(), cancellationToken);


        private IQueryable<PokemonMaster> GetByIdQuery(int id)
            => FindByCondition(q => q.Id == id);

        public async Task<RModel?> GetByIdAsync<RModel>(int id, CancellationToken cancellationToken)
            => await FirstOrDefaultAsync<RModel>(GetByIdQuery(id), cancellationToken);

        public async Task<PokemonMaster?> GetByIdAsync(int id, CancellationToken cancellationToken)
            => await GetByIdQuery(id)
                .FirstOrDefaultAsync(cancellationToken);
    }
}
