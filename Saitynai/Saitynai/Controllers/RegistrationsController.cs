using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Saitynai.Auth.Model;
using Saitynai.Data.Dtos.Competitions;
using Saitynai.Data.Dtos.Events;
using Saitynai.Data.Dtos.Registrations;
using Saitynai.Data.Entities;
using Saitynai.Data.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Saitynai.Controllers
{
    [ApiController]
    [Route("api/events/{eventId}/competitions/{competitionId}/registrations")]
    public class RegistrationsController : ControllerBase
    {
        private readonly ICompetitionsRepository _competitionsRepository;
        private readonly IEventsRepository _eventsRepository;
        private readonly IRegistrationsRepository _registrationsRepository;
        private readonly IAuthorizationService _authorizationService;
        public RegistrationsController(IRegistrationsRepository registrationsRepository, ICompetitionsRepository competitionsRepository, IEventsRepository eventsRepository, IAuthorizationService authorizationService)
        {
            _registrationsRepository = registrationsRepository;
            _competitionsRepository = competitionsRepository;
            _eventsRepository = eventsRepository;
            _authorizationService = authorizationService;
        }

        //[HttpGet]
        //[Authorize(Roles = IsmRoles.Admin)]
        //public async Task<IEnumerable<RegistrationDto>> GetMany(int competitionId)
        //{
        //    var registrations = await _registrationsRepository.GetManyAsync(competitionId);

        //    return registrations.Select(o => new RegistrationDto(o.Id, o.CarNo, o.Manufacturer, o.Model));
        //}

        [HttpGet]
        [Authorize(Roles = IsmRoles.IsmUser)]
        public async Task<ActionResult<IEnumerable<RegistrationDto>>> GetMany(int eventId, int competitionId)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var competitions = await _eventsRepository.GetAsync(competitionId);

            if (competitions == null)
                return NotFound();

            var registrations = await _registrationsRepository.GetManyAsync(competitionId);

            var result = new List<RegistrationDto>();
            var i = 0;
            foreach(var registration in registrations)
            {
                var authorizationResult = await _authorizationService.AuthorizeAsync(User, registration, PolicyNames.ResourceOwner);
                if (authorizationResult.Succeeded)
                {
                    i++;
                    result.Add(new RegistrationDto(registration.Id, registration.CarNo, registration.Manufacturer, registration.Model));
                }
            }

            return Ok(result);
        }



        [HttpGet]
        [Route("{registrationId}", Name = "GetRegistration")]
        [Authorize(Roles = IsmRoles.IsmUser)]
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

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, registration, PolicyNames.ResourceOwner);
            if (!authorizationResult.Succeeded)
            {
                // 404
                return Forbid();
            }

            return new RegistrationDto(registration.Id, registration.CarNo, registration.Manufacturer, registration.Model);
        }

        [HttpPost]
        [Authorize(Roles = IsmRoles.IsmUser)]
        public async Task<ActionResult<RegistrationDto>> Create(int eventId, int competitionId, CreateRegistrationDto createRegistrationDto)
        {
            var eventmodel = await _eventsRepository.GetAsync(eventId);

            if (eventmodel == null)
                return NotFound();

            var competition = await _competitionsRepository.GetAsync(eventId, competitionId);

            if (competition == null)
                return NotFound();

            var registration = new Registration { CarNo = createRegistrationDto.CarNo, Manufacturer = createRegistrationDto.Manufacturer, Model = createRegistrationDto.Model, Competition = competition, UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)};

            await _registrationsRepository.CreateAsync(registration);

            return Created("", new RegistrationDto(registration.Id,registration.CarNo, registration.Manufacturer, registration.Model));
        }

        [HttpPut]
        [Route("{registrationId}")]
        [Authorize(Roles = IsmRoles.IsmUser)]
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

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, registration, PolicyNames.ResourceOwner);
            if (!authorizationResult.Succeeded)
            {
                // 404
                return Forbid();
            }

            registration.CarNo = updateRegistrationDto.CarNo;
            registration.Manufacturer = updateRegistrationDto.Manufacturer;
            registration.Model = updateRegistrationDto.Model;

            await _registrationsRepository.UpdateAsync(registration);

            return Ok(new RegistrationDto(registration.Id, registration.CarNo, registration.Manufacturer, registration.Model));

        }

        [HttpDelete]
        [Route("{registrationId}")]
        [Authorize(Roles = IsmRoles.IsmUser)]
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

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, registration, PolicyNames.ResourceOwner);
            if (!authorizationResult.Succeeded)
            {
                // 404
                return Forbid();
            }

            await _registrationsRepository.DeleteAsync(registration);

            return NoContent();
        }
    }
}
