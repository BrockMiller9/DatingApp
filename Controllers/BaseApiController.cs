
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))] // this will add the log user activity service to our application
    [ApiController]
    [Route("api/[controller]")]  //will acces the controller by GET /api/users
    public class BaseApiController : ControllerBase
    {

    }
}