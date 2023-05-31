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
            var result = await _serviceWrapper.ShelterService.GetAll();

            return Ok(result.Data);
        }

        [HttpGet]
        [Authorize]
        [Route("{shelterId}")]
        public async Task<ActionResult<ShelterResponse>> GetById(Guid shelterId)
        {
            var result = await _serviceWrapper.ShelterService.GetById(shelterId);
            if (result.StatusCode.NotFound())
                return NotFound();
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create([FromBody] PostShelterRequest shelter)
        {
            var result = await _serviceWrapper.ShelterService.Create(shelter);
            return Created(result.Data!.ToString(), null);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [Route("{shelterId}")]
        public async Task<ActionResult> Update(Guid shelterId, [FromBody] PutShelterRequest shelter)
        {
            if ((await _serviceWrapper.ShelterService.Update(shelterId, shelter)).StatusCode.BadRequest())
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
            
            var result = await _serviceWrapper.PetService.GetByShelter((Guid)shelterId);

            return Ok(result.Data);
        }

        [HttpGet("announcements")]
        [Authorize(Roles = "shelter")]
        public async Task<ActionResult<PagedAnnouncementResponse>> GetAnnouncements([FromQuery] int pageNumber, [FromQuery] int pageCount)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var shelterId = identity?.GetId();
            if (shelterId is null)
                return BadRequest();
            
            var result = await _serviceWrapper.AnnouncementService.GetByShelter((Guid)shelterId, pageNumber, pageCount);

            return Ok(result.Data);
        }
    }
}