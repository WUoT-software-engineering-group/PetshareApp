using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petshare.CrossCutting.DTO.Adopter;
using Petshare.CrossCutting.Utils;
using Petshare.Services.Abstract;

namespace Petshare.Presentation.Controllers;

[ApiController]
[Route("adopter")]
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
        var result = await _serviceWrapper.AdopterService.GetAll();

        return Ok(result.Data);
    }

    [HttpGet]
    [Route("{adopterId}")]
    [Authorize(Roles = "admin,adopter")]
    public async Task<ActionResult<GetAdopterResponse>> GetById(Guid adopterId)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity?.GetRole() == "adopter" && identity.GetId() != adopterId)
            return Forbid();

        var result = await _serviceWrapper.AdopterService.GetById(adopterId);
        if (result.StatusCode.NotFound())
            return NotFound();
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Create([FromBody] PostAdopterRequest adopter)
    {
        var result = await _serviceWrapper.AdopterService.Create(adopter);

        return Created(result.Data!.ToString(), null);
    }

    [HttpPut]
    [Route("{adopterId}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult> Update(Guid adopterId, [FromBody] PutAdopterRequest adopter)
    {
        if ((await _serviceWrapper.AdopterService.UpdateStatus(adopterId, adopter)).StatusCode.NotFound())
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

        var result = await _serviceWrapper.AdopterService.VerifyAdopterForShelter(adopterId, shelterId.Value);

        if (result.StatusCode.NotFound())
            return NotFound("Adopter doesn't exist");

        if (result.StatusCode.BadRequest())
            return BadRequest("Adopter already verified");

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

        var result = await _serviceWrapper.AdopterService.CheckIfAdopterIsVerified(adopterId, shelterId.Value);
        return Ok(result.Data);
    }
}   