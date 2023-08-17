using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Restaurants_REST_API.Controllers
{
    /*
     * This controller is for customers only! 
     * Business logic for customer and owner is completely 
     * different so this is why this controller may have 
     * similar endpoints to others existing controllers
     */
    [Route("api/client")]
    [ApiController]
    public class ClientsController : ControllerBase
    {

    }
}
