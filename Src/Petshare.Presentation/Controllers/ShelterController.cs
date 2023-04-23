using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petshare.CrossCutting.DTO.Announcement;
using Petshare.CrossCutting.DTO.Pet;
using Petshare.CrossCutting.DTO.Shelter;
using Petshare.CrossCutting.Utils;
using Petshare.Services.Abstract;

namespace Petshare.Presentation.Controllers
{
    [ApiController]
    [Route("shelter")]
    public class ShelterController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public ShelterController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ShelterResponse>>> GetAll()
        {
            var shelters = await _serviceWrapper.ShelterService.GetAll();

            return Ok(shelters);
        }

        [HttpGet]
        [Authorize]
        [Route("{shelterId}")]
        public async Task<ActionResult<ShelterResponse>> GetById(Guid shelterId)
        {
            var shelter = await _serviceWrapper.ShelterService.GetById(shelterId);
            if (shelter == null)
                return NotFound();
            return Ok(shelter);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create([FromBody] PostShelterRequest shelter)
        {
            var createdShelterId = await _serviceWrapper.ShelterService.Create(shelter);
            return Created(createdShelterId.ToString(), null);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [Route("{shelterId}")]
        public async Task<ActionResult> Update(Guid shelterId, [FromBody] PutShelterRequest shelter)
        {
            if (!await _serviceWrapper.ShelterService.Update(shelterId, shelter))
                return BadRequest();
            return Ok();
        }

        [HttpGet("pets")]
        [Authorize(Roles = "shelter")]
        public async Task<ActionResult<IEnumerable<PetResponse>>> GetPets()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var shelterId = identity?.GetId();
            if (shelterId is null)
                return BadRequest();
            
            var pets = await _serviceWrapper.PetService.GetByShelter((Guid)shelterId);

            return Ok(pets);
        }

        [HttpGet("announcements")]
        [Authorize(Roles = "shelter")]
        public async Task<ActionResult<IEnumerable<AnnouncementResponse>>> GetAnnouncements()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var shelterId = identity?.GetId();
            if (shelterId is null)
                return BadRequest();
            
            var announcements = await _serviceWrapper.AnnouncementService.GetByShelter((Guid)shelterId);

            return Ok(announcements);
        }
    }
}