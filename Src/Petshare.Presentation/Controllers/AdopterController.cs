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

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] PostAdopterRequest adopter)
    {
        var createdAdopterId = await _serviceWrapper.AdopterService.Create(adopter);

        return Ok(createdAdopterId);
    }
}   