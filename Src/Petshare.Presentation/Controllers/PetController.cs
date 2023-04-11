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

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<PetResponse>>> GetAll()
        {
            //var shelterId = // retrieved from roles
            //var pets = await _serviceWrapper.PetService.GetByShelter(shelterId);

            // TODO: remove when auth is added
            var pets = await _serviceWrapper.PetService.GetByShelter();

            return Ok(pets);
        }

        [HttpGet("{petId}")]
        public async Task<ActionResult<PetResponse>> GetById(Guid petId)
        {
            var pet = await _serviceWrapper.PetService.GetById(petId);

            return pet is null
                ? NotFound()
                : Ok(pet);
        }

        [HttpPost()]
        public async Task<ActionResult<PetResponse>> Create([FromBody] PostPetRequest pet)
        {
            //var shelterId = // retrieved from roles
            //var createdPet = await _serviceWrapper.PetService.Create(shelterId, pet);

            // TODO: remove when auth is added
            var shelters = await _serviceWrapper.ShelterService.GetAll();
            var shelterId = shelters.First().ID;
            var createdPet = await _serviceWrapper.PetService.Create(shelterId, pet);

            return createdPet is null
                ? BadRequest()
                : Ok(createdPet);
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
