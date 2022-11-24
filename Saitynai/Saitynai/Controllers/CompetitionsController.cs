using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saitynai.Auth.Model;
using Saitynai.Data.Dtos.Competitions;
using Saitynai.Data.Dtos.Events;
using Saitynai.Data.Entities;
using Saitynai.Data.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Saitynai.Controllers
{
    [ApiController]
    [Route("api/events/{eventId}/competitions")]
    public class CompetitionsController : ControllerBase
    {
        private readonly ICompetitionsRepository _competitionsRepository;
        private readonly IEventsRepository _eventsRepository;
        private readonly IAuthorizationService _authorizationService;
        public CompetitionsController(ICompetitionsRepository competitionsRepository, IEventsRepository eventsRepository, IAuthorizationService authorizationService)
        {
            _competitionsRepository = competitionsRepository;
            _eventsRepository = eventsRepository;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IEnumerable<CompetitionDto>> GetMany(int eventId)
        {
            var competitions = await _competitionsRepository.GetManyAsync(eventId);

            return competitions.Select(o => new CompetitionDto(o.Id, o.Name, o.Description, o.Rules));
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
        [Authorize(Roles = IsmRoles.Admin)]
        public async Task<ActionResult<CompetitionDto>> Create(int eventId, CreateCompetitionDto createCompetitionDto)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var competition = new Competition { Name = createCompetitionDto.Name, Description = createCompetitionDto.Description, Rules = createCompetitionDto.Rules, Event = eventmodel, UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)};

            await _competitionsRepository.CreateAsync(competition);

            return Created("", new CompetitionDto(competition.Id, competition.Name, competition.Description, competition.Rules));
        }

        [HttpPut]
        [Route("{competitionId}")]
        [Authorize(Roles = IsmRoles.Admin)]
        public async Task<ActionResult<CompetitionDto>> Update(int eventId, int competitionId, UpdateCompetitionDto updateCompetitionDto)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var competition = await _competitionsRepository.GetAsync(eventId, competitionId);

            if (competition == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, competition, PolicyNames.ResourceOwner);
            if (!authorizationResult.Succeeded)
            {
                // 404
                return Forbid();
            }

            competition.Name = updateCompetitionDto.Name;
            competition.Description = updateCompetitionDto.Description;
            competition.Rules = updateCompetitionDto.Rules;

            await _competitionsRepository.UpdateAsync(competition);

            return Ok(new CompetitionDto(competition.Id, competition.Name, competition.Description, competition.Rules));

        }

        [HttpDelete]
        [Route("{competitionId}")]
        [Authorize(Roles = IsmRoles.Admin)]
        public async Task<ActionResult> Remove(int eventId, int competitionId)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var competition = await _competitionsRepository.GetAsync(eventId, competitionId);

            if (competition == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, competition, PolicyNames.ResourceOwner);
            if (!authorizationResult.Succeeded)
            {
                // 404
                return Forbid();
            }

            await _competitionsRepository.DeleteAsync(competition);

            return NoContent();
        }

    }
}
