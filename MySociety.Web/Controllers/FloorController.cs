using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySociety.Entity.Models;
using MySociety.Service.Interfaces;

namespace MySociety.Web.Controllers;

public class FloorController : Controller
{
    private readonly IFloorService _floorService;

    public FloorController(IFloorService floorService)
    {
        _floorService = floorService;
    }

    [HttpGet]
    public async Task<IActionResult> List(int blockId)
    {
        IEnumerable<Floor> houses = await _floorService.Get(blockId);
        return Json(new SelectList(houses, "Id", "Name"));
    }
}
