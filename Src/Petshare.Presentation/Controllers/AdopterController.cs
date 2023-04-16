using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petshare.CrossCutting.DTO.Adopter;
using Petshare.CrossCutting.Utils;
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
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<IEnumerable<GetAdopterResponse>>> GetAll()
    {
        var adopters = await _serviceWrapper.AdopterService.GetAll();

        return Ok(adopters);
    }

    [HttpGet]
    [Route("{adopterId}")]
    [Authorize(Roles = "admin,adopter")]
    public async Task<ActionResult<GetAdopterResponse>> GetById(Guid adopterId)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity?.GetRole() == "adopter" && identity.GetId() != adopterId)
            return Forbid();

        var adopter = await _serviceWrapper.AdopterService.GetById(adopterId);
        if (adopter == null)
            return NotFound();
        return Ok(adopter);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Guid>> Create([FromBody] PostAdopterRequest adopter)
    {
        var createdAdopterId = await _serviceWrapper.AdopterService.Create(adopter);

        return Ok(createdAdopterId);
    }

    [HttpPut]
    [Route("{adopterId}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult> Update(Guid adopterId, [FromBody] PutAdopterRequest adopter)
    {
        if (!await _serviceWrapper.AdopterService.UpdateStatus(adopterId, adopter))
            return NotFound();
        return Ok();
    }

    [HttpPut]
    [Route("{adopterId}/verify")]
    [Authorize(Roles = "shelter")]
    public async Task<ActionResult> VerifyAdopterForShelter(Guid adopterId)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var shelterId = identity?.GetId();
        if (shelterId == null)
            return Forbid();

        await _serviceWrapper.AdopterService.VerifyAdopterForShelter(adopterId, shelterId.Value);
        return Ok();
    }

    [HttpGet]
    [Route("{adopterId}/isVerified")]
    [Authorize(Roles = "shelter")]
    public async Task<ActionResult<bool>> CheckIfAdopterIsVerified(Guid adopterId)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var shelterId = identity?.GetId();
        if (shelterId == null)
            return Forbid();

        var isVerified = await _serviceWrapper.AdopterService.CheckIfAdopterIsVerified(adopterId, shelterId.Value);
        return Ok(isVerified);
    }
}   