using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySociety.Entity.Models;
using MySociety.Service.Interfaces;

namespace MySociety.Web.Controllers;

public class HouseController : Controller
{
    private readonly IHouseService _houseService;

    public HouseController(IHouseService houseService)
    {
        _houseService = houseService;
    }

    [HttpGet]
    public async Task<IActionResult> List(int blockId, int floorId)
    {
        IEnumerable<House> houses = await _houseService.Get(blockId, floorId);
        return Json(new SelectList(houses, "Id", "Name"));
    }

}
