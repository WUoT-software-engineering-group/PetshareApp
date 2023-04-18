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

        [HttpGet("{announcementId}")]
        public async Task<ActionResult<AnnouncementResponse>> GetById(Guid announcementId)
        {
            var announcement = await _serviceWrapper.AnnouncementService.GetById(announcementId);

            if (announcement is null)
            {
                return NotFound();
            }

            return Ok(announcement);
        }

        [HttpGet]
        public async Task<ActionResult<List<AnnouncementResponse>>> GetByFilters([FromQuery] GetAnnouncementsRequest filters)
        {
            var announcements = await _serviceWrapper.AnnouncementService.GetByFilters(filters);

            return Ok(announcements);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PostAnnouncementRequest announcement)
        {
            // TODO: Wyciągać shelterId z tokena, jak ogarniemy autoryzację
            // var shelterId = // retrieve from roles
            //var createdAnnouncement = await _serviceWrapper.AnnouncementService.Create(shelterId, announcement);

            var shelters = await _serviceWrapper.ShelterService.GetAll();
            var shelterId = shelters.First().ID;
            var createdAnnouncementId = await _serviceWrapper.AnnouncementService.Create(shelterId, announcement);

            if (createdAnnouncementId == null)
                return BadRequest();
            return Created(createdAnnouncementId.ToString(), null);
        }

        [HttpPut]
        [Route("{announcementId}")]
        public async Task<ActionResult> Update(Guid announcementId, [FromBody] PutAnnouncementRequest announcement)
        {
            // TODO: Wyciągać userId z tokena, jak ogarniemy autoryzację
            var userId = Guid.NewGuid();
            if (!await _serviceWrapper.AnnouncementService.Update(userId, announcementId, announcement))
                return BadRequest();
            return Ok();
        }
    }
}
