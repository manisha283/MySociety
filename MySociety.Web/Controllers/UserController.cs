using Microsoft.AspNetCore.Mvc;

namespace MySociety.Web.Controllers;

public class UserController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    

}
