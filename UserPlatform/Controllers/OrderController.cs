using Microsoft.AspNetCore.Mvc;

namespace UserPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        [HttpGet]
        public ActionResult Test()
        {
            return Ok();
        }
    }
}
