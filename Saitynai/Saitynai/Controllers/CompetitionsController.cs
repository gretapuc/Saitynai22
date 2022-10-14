using Microsoft.AspNetCore.Mvc;
using Saitynai.Data.Dtos.Competitions;
using Saitynai.Data.Dtos.Events;
using Saitynai.Data.Entities;
using Saitynai.Data.Repositories;

namespace Saitynai.Controllers
{
    [ApiController]
    [Route("api/events/{eventId}/competitions")]
    public class CompetitionsController : ControllerBase
    {
        private readonly ICompetitionsRepository _competitionsRepository;
        private readonly IEventsRepository _eventsRepository;
        public CompetitionsController(ICompetitionsRepository competitionsRepository, IEventsRepository eventsRepository)
        {
            _competitionsRepository = competitionsRepository;
            _eventsRepository = eventsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<CompetitionDto>> GetMany(int eventId)
        {
            var events = await _competitionsRepository.GetManyAsync(eventId);

            return events.Select(o => new CompetitionDto(o.Id, o.Name, o.Description, o.Rules));
        }

        [HttpGet]
        [Route("{competitionId}", Name = "GetCompetition")]
        public async Task<ActionResult<CompetitionDto>> Get(int eventId,int competitionId)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var competition = await _competitionsRepository.GetAsync(eventId, competitionId);

            if (competition == null)
                return NotFound();

            return new CompetitionDto(competition.Id, competition.Name, competition.Description, competition.Rules);
        }

        [HttpPost]
        public async Task<ActionResult<CompetitionDto>> Create(int eventId, CreateCompetitionDto createCompetitionDto)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var competition = new Competition { Name = createCompetitionDto.Name, Description = createCompetitionDto.Description, Rules = createCompetitionDto.Rules, Event = eventmodel };

            await _competitionsRepository.CreateAsync(competition);

            return Created("", new CompetitionDto(competition.Id, competition.Name, competition.Description, competition.Rules));
        }

        [HttpPut]
        [Route("{competitionId}")]
        public async Task<ActionResult<CompetitionDto>> Update(int eventId, int competitionId, UpdateCompetitionDto updateCompetitionDto)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var competition = await _competitionsRepository.GetAsync(eventId, competitionId);

            if (competition == null)
                return NotFound();

            competition.Name = updateCompetitionDto.Name;
            competition.Description = updateCompetitionDto.Description;
            competition.Rules = updateCompetitionDto.Rules;

            await _competitionsRepository.UpdateAsync(competition);

            return Ok(new CompetitionDto(competition.Id, competition.Name, competition.Description, competition.Rules));

        }

        [HttpDelete]
        [Route("{competitionId}")]
        public async Task<ActionResult> Remove(int eventId, int competitionId)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var competition = await _competitionsRepository.GetAsync(eventId, competitionId);

            if (competition == null)
                return NotFound();

            await _competitionsRepository.DeleteAsync(competition);

            return NoContent();
        }

    }
}
