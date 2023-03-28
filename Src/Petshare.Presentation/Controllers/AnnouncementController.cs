using Microsoft.AspNetCore.Mvc;
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
    }
}
