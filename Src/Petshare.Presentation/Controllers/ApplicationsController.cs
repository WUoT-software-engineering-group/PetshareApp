using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petshare.CrossCutting.DTO.Applications;
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
    public async Task<ActionResult> Create([FromBody] Guid announcementId)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var adopterId = identity?.GetId();
        if (adopterId is null)
            return Unauthorized();

        var createdApplicationId = await _serviceWrapper.ApplicationsService.Create(announcementId);

        return createdApplicationId is null
            ? BadRequest()
            : Created(createdApplicationId.ToString(), null);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ApplicationResponse>>> GetAll()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var role = identity?.GetRole();
        var userId = identity?.GetId();

        if (role is null || userId is null) 
        {
            return Unauthorized();
        }

        var applications = await _serviceWrapper.ApplicationsService.GetAll(role, (Guid)userId);

        return Ok(applications);
    }
}

