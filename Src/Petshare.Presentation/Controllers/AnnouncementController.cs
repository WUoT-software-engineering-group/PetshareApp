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
    }
}
