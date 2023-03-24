using Microsoft.AspNetCore.Mvc;
using Petshare.CrossCutting.DTO.Pet;
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
        public async Task<ActionResult<PetResponse>> GetById(Guid petId)
        {
            var pet = await _serviceWrapper.PetService.GetById(petId);

            return pet is null
                ? BadRequest()
                : Ok(pet);
        }

        [HttpPost()]
        public async Task<ActionResult<PetResponse>> Create([FromBody] PostPetRequest pet)
        {
            //var shelterId = // retrieved from roles
            //var createdPet = await _serviceWrapper.PetService.Create(shelterId, pet);

            //return createdPet is null
            //    ? BadRequest()
            //    : Ok(createdPet);

            return Ok(await Task.FromResult(pet));
        }

        [HttpPut("{petId}")]
        public async Task<ActionResult<PetResponse>> Update(Guid petId, [FromBody] PutPetRequest pet)
        {
            var updateSuccessul = await _serviceWrapper.PetService.Update(petId, pet);

            return !updateSuccessul
                ? BadRequest()
                : Ok(updateSuccessul);
        }
    }
}
