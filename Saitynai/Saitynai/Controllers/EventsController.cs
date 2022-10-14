using Microsoft.AspNetCore.Mvc;
using Saitynai.Data.Dtos.Events;
using Saitynai.Data.Entities;
using Saitynai.Data.Repositories;

namespace Saitynai.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsRepository _eventsRepository;
        public EventsController(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
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
        public async Task<ActionResult<EventDto>> Create(CreateEventDto createEventDto)
        {
            var eventmodel = new Event { Name = createEventDto.Name, Date = createEventDto.Date, Description = createEventDto.Description };

            await _eventsRepository.CreateAsync(eventmodel);

            return Created("", new EventDto(eventmodel.Id ,eventmodel.Name, eventmodel.Date, eventmodel.Description));
        }

        [HttpPut]
        [Route("{eventId}")]
        public async Task<ActionResult<EventDto>> Update(int eventId, UpdateEventDto updateEventDto)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            eventmodel.Name = updateEventDto.Name;
            eventmodel.Date = updateEventDto.Date; 
            eventmodel.Description = updateEventDto.Description;

            await _eventsRepository.UpdateAsync(eventmodel);

            return Ok(new EventDto(eventmodel.Id, eventmodel.Name, eventmodel.Date, eventmodel.Description));

        }

        [HttpDelete]
        [Route("{eventId}")]
        public async Task<ActionResult> Remove(int eventId)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            await _eventsRepository.DeleteAsync(eventmodel);

            return NoContent();
        }
    }
}
