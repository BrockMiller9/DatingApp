

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  //will acces the controller by GET /api/users
    public class BaseApiController : ControllerBase
    {

    }
}