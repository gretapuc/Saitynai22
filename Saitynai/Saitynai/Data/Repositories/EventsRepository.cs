using Saitynai.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Saitynai.Data.Repositories
{
    public interface IEventsRepository
    {
        Task<Event?> GetAsync(int eventId);
        Task<IReadOnlyList<Event>> GetManyAsync();
        Task CreateAsync(Event eventmodel);
        Task UpdateAsync(Event eventmodel);
        Task DeleteAsync(Event eventmodel);

    }
    public class EventsRepository : IEventsRepository
    {
        private readonly IsmDbContext _ismDbContext;
        public EventsRepository(IsmDbContext ismDbContext)
        {
            _ismDbContext = ismDbContext;
        }

        public async Task<Event?> GetAsync(int eventId)
        {
            return await _ismDbContext.Events.FirstOrDefaultAsync(o => o.Id == eventId);
        }

        public async Task<IReadOnlyList<Event>> GetManyAsync()
        {
            return await _ismDbContext.Events.ToListAsync();
        }
        
        public async Task CreateAsync(Event eventmodel)
        {
            _ismDbContext.Events.Add(eventmodel);
            await _ismDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Event eventmodel)
        {
            _ismDbContext.Events.Update(eventmodel);
            await _ismDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Event eventmodel)
        {
            _ismDbContext.Events.Remove(eventmodel);
            await _ismDbContext.SaveChangesAsync();
        }
    }
}
