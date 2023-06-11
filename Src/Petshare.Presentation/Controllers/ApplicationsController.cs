using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petshare.CrossCutting.DTO.Applications;
using Petshare.CrossCutting.Enums;
using Petshare.CrossCutting.Utils;
using Petshare.Services.Abstract;
using System.Security.Claims;

namespace Petshare.Presentation.Controllers;

[ApiController]
[Route("applications")]
public class ApplicationsController : ControllerBase
{
    private readonly IServiceWrapper _serviceWrapper;

    public ApplicationsController(IServiceWrapper serviceWrapper)
    {
        _serviceWrapper = serviceWrapper;
    }

    [HttpPost]
    [Authorize(Roles = "adopter")]
    public async Task<ActionResult> Create([FromBody] PostApplicationRequest request)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var adopterId = identity?.GetId();
        if (adopterId is null)
            return Unauthorized();

        var result = await _serviceWrapper.ApplicationsService.Create(request.AnnouncementId, (Guid)adopterId);

        return result.StatusCode.BadRequest()
            ? BadRequest()
            : Created(result.Data!.ToString(), null);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedApplicationResponse>> GetAll([FromQuery] int pageNumber, [FromQuery] int pageCount)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var role = identity?.GetRole();
        var userId = identity?.GetId();

        if (role is null || userId is null) 
        {
            return Unauthorized();
        }

        var result = await _serviceWrapper.ApplicationsService.GetAll(role, (Guid)userId, pageNumber, pageCount);

        return Ok(result.Data);
    }

    [HttpGet("{announcementId}")]
    [Authorize(Roles = "shelter")]
    public async Task<ActionResult<PagedApplicationResponse>> GetByAnnouncement(Guid announcementId, [FromQuery] int pageNumber, [FromQuery] int pageCount)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var shelterId = identity?.GetId();

        if (shelterId is null)
        {
            return Unauthorized();
        }

        var result = await _serviceWrapper.ApplicationsService.GetByAnnouncement(announcementId, (Guid)shelterId, pageNumber, pageCount);

        if (result.StatusCode.BadRequest())
        {
            return BadRequest();
        }

        return Ok(result.Data);
    }

    [HttpPut("{applicationId}/accept")]
    [Authorize(Roles = "shelter")]
    public async Task<ActionResult> Accept(Guid applicationId)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var shelterId = identity?.GetId();

        if (shelterId is null)
        {
            return Unauthorized();
        }

        var result = await _serviceWrapper.ApplicationsService.UpdateStatus(applicationId, ApplicationStatus.Accepted, (Guid)shelterId);
        return result.StatusCode.BadRequest()
            ? BadRequest()
            : Ok();
    }

    [HttpPut("{applicationId}/reject")]
    [Authorize(Roles = "shelter")]
    public async Task<ActionResult> Reject(Guid applicationId)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var shelterId = identity?.GetId();

        if (shelterId is null)
        {
            return Unauthorized();
        }

        var result = await _serviceWrapper.ApplicationsService.UpdateStatus(applicationId, ApplicationStatus.Rejected, (Guid)shelterId);
        return result.StatusCode.BadRequest()
            ? BadRequest()
            : Ok();
    }

    [HttpPut("{applicationId}/withdraw")]
    [Authorize(Roles = "shelter")]
    public async Task<ActionResult> Withdraw(Guid applicationId)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var shelterId = identity?.GetId();

        if (shelterId is null)
        {
            return Unauthorized();
        }

        var result = await _serviceWrapper.ApplicationsService.UpdateStatus(applicationId, ApplicationStatus.Withdrawn, (Guid)shelterId);
        return result.StatusCode.BadRequest()
            ? BadRequest()
            : Ok();
    }
}

