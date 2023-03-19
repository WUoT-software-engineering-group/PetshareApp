using Microsoft.AspNetCore.Mvc;
using Petshare.CrossCutting.DTO;
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
        public async Task<ActionResult<IEnumerable<ShelterDTO>>> GetAll()
        {
            var shelters = await _serviceWrapper.ShelterService.GetAll();

            return Ok(shelters);
        }

        [HttpGet]
        [Route("{shelterId}")]
        public async Task<ActionResult<ShelterDTO>> GetById(string shelterId)
        {
            var shelter = await _serviceWrapper.ShelterService.GetById(shelterId);
            if (shelter == null)
                return NotFound();
            return Ok(shelter);
        }

        [HttpPost]
        public async Task<ActionResult<ShelterDTO?>> Create([FromBody] ShelterDTO shelter)
        {
            var createdShelter = await _serviceWrapper.ShelterService.Create(shelter);
            return Ok(createdShelter);
        }

        [HttpPut]
        [Route("{shelterId}")]
        public async Task<ActionResult> Update(string shelterId, [FromBody] ShelterDTO shelter)
        {
            if (!await _serviceWrapper.ShelterService.Update(shelterId, shelter))
                return BadRequest();
            return Ok();
        }
    }
}