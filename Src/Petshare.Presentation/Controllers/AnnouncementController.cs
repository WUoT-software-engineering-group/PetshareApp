using Microsoft.AspNetCore.Mvc;
using Petshare.CrossCutting.DTO.Announcement;
using Petshare.Services.Abstract;

namespace Petshare.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public AnnouncementController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AnnouncementResponse>>> GetByFilters([FromQuery] GetAnnouncementsRequest filters)
        {
            var announcements = await _serviceWrapper.AnnouncementService.GetByFilters(filters);

            return Ok(announcements);
        }

        [HttpPost]
        public async Task<ActionResult<AnnouncementResponse>> Create([FromBody] PostAnnouncementRequest announcement)
        {
            // TODO: Wyciągać shelterId z tokena, jak ogarniemy autoryzację
            return Ok();
            //var createdAnnouncement = await _serviceWrapper.AnnouncementService.Create(id, announcement);
            //if (createdAnnouncement == null)
            //    return BadRequest();
            //return Ok(createdAnnouncement);
        }

        [HttpPut]
        [Route("{announcementId}")]
        public async Task<ActionResult> Update(Guid announcementId, [FromBody] PutAnnouncementRequest announcement)
        {
            // TODO: Wyciągać userId z tokena, jak ogarniemy autoryzację
            //return Ok();
            var userId = Guid.NewGuid();
            if (!await _serviceWrapper.AnnouncementService.Update(userId, announcementId, announcement))
                return BadRequest();
            return Ok();
        }
    }
}
