using Microsoft.AspNetCore.Mvc;
using Saitynai.Data.Dtos.Competitions;
using Saitynai.Data.Dtos.Events;
using Saitynai.Data.Dtos.Registrations;
using Saitynai.Data.Entities;
using Saitynai.Data.Repositories;

namespace Saitynai.Controllers
{
    [ApiController]
    [Route("api/events/{eventId}/competitions/{competitionId}/registrations")]
    public class RegistrationsController : ControllerBase
    {
        private readonly ICompetitionsRepository _competitionsRepository;
        private readonly IEventsRepository _eventsRepository;
        private readonly IRegistrationsRepository _registrationsRepository;
        public RegistrationsController(IRegistrationsRepository registrationsRepository, ICompetitionsRepository competitionsRepository, IEventsRepository eventsRepository)
        {
            _registrationsRepository = registrationsRepository;
            _competitionsRepository = competitionsRepository;
            _eventsRepository = eventsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<RegistrationDto>> GetMany(int competitionId)
        {
            var events = await _registrationsRepository.GetManyAsync(competitionId);

            return events.Select(o => new RegistrationDto(o.Id, o.CarNo, o.Manufacturer, o.Model));
        }

        [HttpGet]
        [Route("{registrationId}", Name = "GetRegistration")]
        public async Task<ActionResult<RegistrationDto>> Get(int eventId, int competitionId, int registrationId)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var competition = await _competitionsRepository.GetAsync(eventId, competitionId);

            if (competition == null)
                return NotFound();

            var registration = await _registrationsRepository.GetAsync(competitionId, registrationId);

            if (registration == null)
                return NotFound();

            return new RegistrationDto(registration.Id, registration.CarNo, registration.Manufacturer, registration.Model);
        }

        [HttpPost]
        public async Task<ActionResult<RegistrationDto>> Create(int eventId, int competitionId, CreateRegistrationDto createRegistrationDto)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var competition = await _competitionsRepository.GetAsync(eventId, competitionId);

            if (competition == null)
                return NotFound();

            var registration = new Registration { CarNo = createRegistrationDto.CarNo, Manufacturer = createRegistrationDto.Manufacturer, Model = createRegistrationDto.Model, Competition = competition};

            await _registrationsRepository.CreateAsync(registration);

            return Created("", new RegistrationDto(registration.Id,registration.CarNo, registration.Manufacturer, registration.Model));
        }

        [HttpPut]
        [Route("{registrationId}")]
        public async Task<ActionResult<RegistrationDto>> Update(int eventId, int competitionId, int registrationId, UpdateRegistrationDto updateRegistrationDto)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var competition = await _competitionsRepository.GetAsync(eventId, competitionId);

            if (competition == null)
                return NotFound();

            var registration = await _registrationsRepository.GetAsync(competitionId, registrationId);

            if (registration == null)
                return NotFound();

            registration.CarNo = updateRegistrationDto.CarNo;
            registration.Manufacturer = updateRegistrationDto.Manufacturer;
            registration.Model = updateRegistrationDto.Model;

            await _registrationsRepository.UpdateAsync(registration);

            return Ok(new RegistrationDto(registration.Id, registration.CarNo, registration.Manufacturer, registration.Model));

        }

        [HttpDelete]
        [Route("{registrationId}")]
        public async Task<ActionResult> Remove(int eventId, int competitionId, int registrationId)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var competition = await _competitionsRepository.GetAsync(eventId, competitionId);

            if (competition == null)
                return NotFound();

            var registration = await _registrationsRepository.GetAsync(competitionId, registrationId);

            if (registration == null)
                return NotFound();

            await _registrationsRepository.DeleteAsync(registration);

            return NoContent();
        }
    }
}
