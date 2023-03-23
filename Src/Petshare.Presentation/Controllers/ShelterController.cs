using Microsoft.AspNetCore.Mvc;
using Petshare.CrossCutting.DTO.Shelter;
using Petshare.Services.Abstract;

namespace Petshare.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShelterController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public ShelterController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShelterResponse>>> GetAll()
        {
            var shelters = await _serviceWrapper.ShelterService.GetAll();

            return Ok(shelters);
        }

        [HttpGet]
        [Route("{shelterId}")]
        public async Task<ActionResult<ShelterResponse>> GetById(Guid shelterId)
        {
            var shelter = await _serviceWrapper.ShelterService.GetById(shelterId);
            if (shelter == null)
                return NotFound();
            return Ok(shelter);
        }

        [HttpPost]
        public async Task<ActionResult<ShelterResponse?>> Create([FromBody] PostShelterRequest shelter)
        {
            var createdShelter = await _serviceWrapper.ShelterService.Create(shelter);
            return Ok(createdShelter);
        }

        [HttpPut]
        [Route("{shelterId}")]
        public async Task<ActionResult> Update(Guid shelterId, [FromBody] PutShelterRequest shelter)
        {
            if (!await _serviceWrapper.ShelterService.Update(shelterId, shelter))
                return BadRequest();

            return Ok();
        }
    }
}