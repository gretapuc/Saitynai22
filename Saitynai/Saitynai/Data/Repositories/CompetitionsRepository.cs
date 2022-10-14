using Microsoft.EntityFrameworkCore;
using Saitynai.Data.Entities;

namespace Saitynai.Data.Repositories
{
    public interface ICompetitionsRepository
    {
        Task<Competition?> GetAsync(int eventId, int competitionId);
        Task<IReadOnlyList<Competition>> GetManyAsync(int eventId);
        Task CreateAsync(Competition competition);
        Task UpdateAsync(Competition competition);
        Task DeleteAsync(Competition competition);

    }
    public class CompetitionsRepository : ICompetitionsRepository
    {
        private readonly IsmDbContext _ismDbContext;
        public CompetitionsRepository(IsmDbContext ismDbContext)
        {
            _ismDbContext = ismDbContext;
        }

        public async Task<Competition?> GetAsync(int eventId, int competitionId)
        {
            return await _ismDbContext.Competitions.FirstOrDefaultAsync(o => o.Event.Id == eventId && o.Id == competitionId);
        }

        public async Task<IReadOnlyList<Competition>> GetManyAsync(int eventId)
        {
            return await _ismDbContext.Competitions.Where(c => c.Event.Id == eventId).ToListAsync();
        }

        public async Task CreateAsync(Competition competition)
        {
            _ismDbContext.Competitions.Add(competition);
            await _ismDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Competition competition)
        {
            _ismDbContext.Competitions.Update(competition);
            await _ismDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Competition competition)
        {
            _ismDbContext.Competitions.Remove(competition);
            await _ismDbContext.SaveChangesAsync();
        }
    }
}
