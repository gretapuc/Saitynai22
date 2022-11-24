using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Saitynai.Auth.Model;
using Saitynai.Data.Dtos.Events;
using Saitynai.Data.Entities;
using Saitynai.Data.Repositories;
using System.Data;
using System.Security.Claims;

namespace Saitynai.Controllers
{
    [ApiController]

    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IAuthorizationService _authorizationService;
        public EventsController(IEventsRepository eventsRepository, IAuthorizationService authorizationService)
        {
            _eventsRepository = eventsRepository;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IEnumerable<EventDto>> GetMany()
        {
            var events = await _eventsRepository.GetManyAsync();

            return events.Select(o => new EventDto(o.Id, o.Name, o.Date, o.Description));
        }

        [HttpGet]
        [Route("{eventId}", Name = "GetEvent")]
        public async Task<ActionResult<EventDto>> Get(int eventId)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            return new EventDto(eventmodel.Id, eventmodel.Name, eventmodel.Date, eventmodel.Description);
        }

        [HttpPost]
        [Authorize(Roles = IsmRoles.Admin)]
        public async Task<ActionResult<EventDto>> Create(CreateEventDto createEventDto)
        {
            var eventmodel = new Event { Name = createEventDto.Name, Date = createEventDto.Date, Description = createEventDto.Description, UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)};

            await _eventsRepository.CreateAsync(eventmodel);

            return Created("", new EventDto(eventmodel.Id ,eventmodel.Name, eventmodel.Date, eventmodel.Description));
        }

        [HttpPut]
        [Route("{eventId}")]
        [Authorize(Roles = IsmRoles.Admin)]
        public async Task<ActionResult<EventDto>> Update(int eventId, UpdateEventDto updateEventDto)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, eventmodel, PolicyNames.ResourceOwner);
            if (!authorizationResult.Succeeded)
            {
                // 404
                return Forbid();
            }

            eventmodel.Name = updateEventDto.Name;
            eventmodel.Date = updateEventDto.Date; 
            eventmodel.Description = updateEventDto.Description;

            await _eventsRepository.UpdateAsync(eventmodel);

            return Ok(new EventDto(eventmodel.Id, eventmodel.Name, eventmodel.Date, eventmodel.Description));

        }

        [HttpDelete]
        [Route("{eventId}")]
        [Authorize(Roles = IsmRoles.Admin)]
        public async Task<ActionResult> Remove(int eventId)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, eventmodel, PolicyNames.ResourceOwner);
            if (!authorizationResult.Succeeded)
            {
                // 404
                return Forbid();
            }

            await _eventsRepository.DeleteAsync(eventmodel);

            return NoContent();
        }
    }
}
