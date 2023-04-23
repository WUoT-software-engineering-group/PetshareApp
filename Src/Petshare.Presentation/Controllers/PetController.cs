using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Petshare.CrossCutting.DTO.Pet;
using Petshare.CrossCutting.Utils;
using Petshare.Services.Abstract;

namespace Petshare.Presentation.Controllers
{
    [ApiController]
    [Route("pet")]
    public class PetController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public PetController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpGet("{petId}")]
        [Authorize]
        public async Task<ActionResult<PetResponse>> GetById(Guid petId)
        {
            var pet = await _serviceWrapper.PetService.GetById(petId);

            return pet is null
                ? NotFound()
                : Ok(pet);
        }

        [HttpPost]
        [Authorize(Roles = "shelter")]
        public async Task<ActionResult> Create([FromBody] PostPetRequest pet)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var shelterId = identity?.GetId();
            if (shelterId is null)
                return BadRequest();

            var createdPetId = await _serviceWrapper.PetService.Create((Guid)shelterId, pet);

            return createdPetId is null
                ? BadRequest()
                : Created(createdPetId.ToString(), null);
        }

        [HttpPut("{petId}")]
        [Authorize(Roles = "admin,shelter")]
        public async Task<ActionResult<PetResponse>> Update(Guid petId, [FromBody] PutPetRequest pet)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity?.GetId();
            if (userId is null)
                return BadRequest();
            
            var updateSuccessful = await _serviceWrapper.PetService.Update((Guid)userId, identity?.GetRole(), petId, pet);

            return !updateSuccessful
                ? BadRequest()
                : Ok(updateSuccessful);
        }

        [HttpPost("{petId}/photo")]
        [Authorize(Roles = "shelter")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<PetResponse>> UploadPhoto(Guid petId, IFormFile file)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var shelterId = identity?.GetId();
            if (shelterId is null)
                return BadRequest();

            var photoUri = await _serviceWrapper.FileService.UploadFile(file.OpenReadStream(), file.FileName);

            var updateSuccessful = await _serviceWrapper.PetService.UpdatePhotoUri(petId, (Guid)shelterId, photoUri);
            
            return !updateSuccessful
                ? BadRequest()
                : Ok(updateSuccessful);
        }
    }
}
