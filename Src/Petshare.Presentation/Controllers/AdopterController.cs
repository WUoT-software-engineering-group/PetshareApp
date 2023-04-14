using Microsoft.AspNetCore.Mvc;
using Petshare.CrossCutting.DTO.Adopter;
using Petshare.Services.Abstract;

namespace Petshare.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdopterController : ControllerBase
{
    private readonly IServiceWrapper _serviceWrapper;

    public AdopterController(IServiceWrapper serviceWrapper)
    {
        _serviceWrapper = serviceWrapper;
    }

    [HttpGet]
    // TODO: dodać autoryzację dla admina
    public async Task<ActionResult<IEnumerable<GetAdopterResponse>>> GetAll()
    {
        var adopters = await _serviceWrapper.AdopterService.GetAll();

        return Ok(adopters);
    }

    [HttpGet]
    [Route("{adopterId}")]
    public async Task<ActionResult<GetAdopterResponse>> GetById(Guid adopterId)
    {
        // TODO: dodać autoryzację: admin bądź adopter z podanym id
        var adopter = await _serviceWrapper.AdopterService.GetById(adopterId);
        if (adopter == null)
            return NotFound();
        return Ok(adopter);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] PostAdopterRequest adopter)
    {
        var createdAdopterId = await _serviceWrapper.AdopterService.Create(adopter);

        return Ok(createdAdopterId);
    }
}   