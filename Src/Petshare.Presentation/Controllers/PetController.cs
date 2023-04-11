using Microsoft.AspNetCore.Http;
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
            var updateSuccessful = await _serviceWrapper.PetService.Update(petId, pet);

            return !updateSuccessful
                ? BadRequest()
                : Ok(updateSuccessful);
        }

        [HttpPost("{petId}/photo")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<PetResponse>> UploadPhoto(Guid petId, IFormFile photo)
        {
            //var shelterId = // retrieved from roles
            
            // TODO: remove when auth is added
            var shelters = await _serviceWrapper.ShelterService.GetAll();
            var shelterId = shelters.First().ID;
            //

            string photoUri = await _serviceWrapper.FileService.UploadFile(photo.OpenReadStream(), photo.FileName);

            var updateSuccessful = await _serviceWrapper.PetService.UpdatePhotoUri(petId, shelterId, photoUri);
            
            return !updateSuccessful
                ? BadRequest()
                : Ok(updateSuccessful);
        }
    }
}
