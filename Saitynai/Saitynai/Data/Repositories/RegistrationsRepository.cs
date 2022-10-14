using Microsoft.EntityFrameworkCore;
using Saitynai.Data.Entities;

namespace Saitynai.Data.Repositories
{
    public interface IRegistrationsRepository
    {
        Task<Registration?> GetAsync(int competitionId, int registrationId);
        Task<IReadOnlyList<Registration>> GetManyAsync(int competitionId);
        Task CreateAsync(Registration registration);
        Task UpdateAsync(Registration registration);
        Task DeleteAsync(Registration registration);

    }
    public class RegistrationsRepository : IRegistrationsRepository
    {
        private readonly IsmDbContext _ismDbContext;
        public RegistrationsRepository(IsmDbContext ismDbContext)
        {
            _ismDbContext = ismDbContext;
        }

        public async Task<Registration?> GetAsync(int competitionId, int registrationId)
        {
            return await _ismDbContext.Registrations.FirstOrDefaultAsync(o => o.Competition.Id == competitionId && o.Id == registrationId);
        }

        public async Task<IReadOnlyList<Registration>> GetManyAsync(int competitionId)
        {
            return await _ismDbContext.Registrations.Where(c => c.Competition.Id == competitionId).ToListAsync();
        }

        public async Task CreateAsync(Registration registration)
        {
            _ismDbContext.Registrations.Add(registration);
            await _ismDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Registration registration)
        {
            _ismDbContext.Registrations.Update(registration);
            await _ismDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Registration registration)
        {
            _ismDbContext.Registrations.Remove(registration);
            await _ismDbContext.SaveChangesAsync();
        }
    }
}
