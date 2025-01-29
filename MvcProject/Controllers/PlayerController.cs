using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcProject.Controllers
{
    [Authorize(Roles = "Player")]
    public class PlayerController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

    }

}
