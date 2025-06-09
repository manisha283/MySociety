using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MySociety.Web.Controllers;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult UserList()
    {
        return PartialView("_ApprovalListPartial");
    }
}
