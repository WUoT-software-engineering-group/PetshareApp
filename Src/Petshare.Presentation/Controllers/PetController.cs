using Microsoft.AspNetCore.Mvc;
using Petshare.CrossCutting.DTO;
using Petshare.Services.Abstract;

namespace Petshare.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public PetController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpGet("{petId}")]
        public async Task<ActionResult<PetDTO>> GetById(Guid petId)
        {
            var pet = await _serviceWrapper.PetService.GetById(petId);

            return pet is null
                ? BadRequest()
                : Ok(pet);
        }

        [HttpPost()]
        public async Task<ActionResult<PetDTO>> Create([FromBody] PetDTO pet)
        {
            var createdPet = await _serviceWrapper.PetService.Create(pet);

            return createdPet is null
                ? BadRequest()
                : Ok(createdPet);
        }

        [HttpPut("{petId}")]
        public async Task<ActionResult<PetDTO>> Update(Guid petId, [FromBody] PetDTO pet)
        {
            pet.ID = petId;

            var updateSuccessul = await _serviceWrapper.PetService.Update(pet);

            return !updateSuccessul
                ? BadRequest()
                : Ok(updateSuccessul);
        }
    }
}
