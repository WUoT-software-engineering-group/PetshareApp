using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petshare.CrossCutting.DTO.Announcement;
using Petshare.CrossCutting.Utils;
using Petshare.Services.Abstract;

namespace Petshare.Presentation.Controllers
{
    [ApiController]
    [Route("announcements")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public AnnouncementController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpGet("{announcementId}")]
        [Authorize]
        public async Task<ActionResult<AnnouncementResponse>> GetById(Guid announcementId)
        {
            var result = await _serviceWrapper.AnnouncementService.GetById(announcementId);

            if (result.StatusCode.NotFound())
            {
                return NotFound();
            }

            return Ok(result.Data);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PagedAnnouncementResponse>> GetByFilters([FromQuery] GetAnnouncementsRequest filters)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var adopterId = identity?.GetRole() == "adopter"
                ? identity.GetId()
                : null;

            var result = await _serviceWrapper.AnnouncementService.GetByFilters(filters, adopterId);

            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = "shelter")]
        public async Task<ActionResult<AnnouncementResponse>> Create([FromBody] PostAnnouncementRequest announcement)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var shelterId = identity?.GetId();
            if (shelterId is null)
                return BadRequest();

            var result = await _serviceWrapper.AnnouncementService.Create((Guid)shelterId, announcement);

            if (result.StatusCode.BadRequest())
                return BadRequest();
            return Created(result.Data!.ToString(), null);
        }

        [HttpPut]
        [Route("{announcementId}")]
        [Authorize(Roles = "admin,shelter")]
        public async Task<ActionResult> Update(Guid announcementId, [FromBody] PutAnnouncementRequest announcement)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity?.GetId();
            if (userId is null)
                return BadRequest();

            if ((await _serviceWrapper.AnnouncementService.Update((Guid)userId, identity?.GetRole() ,announcementId, announcement)).StatusCode.BadRequest())
                return BadRequest();
            
            return Ok();
        }

        [HttpPut]
        [Route("{announcementId}/like")]
        [Authorize(Roles = "adopter")]
        public async Task<ActionResult> Like(Guid announcementId, [FromQuery] bool isLiked)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var adopterId = identity?.GetId();
            if (adopterId is null)
                return BadRequest();

            var response =
                await _serviceWrapper.AnnouncementService.UpdateLikedStatus(adopterId.Value, announcementId, isLiked);

            if (response.StatusCode.BadRequest())
                return BadRequest();

            if (response.StatusCode.NotFound())
                return NotFound();

            return Ok();
        }
    }
}
