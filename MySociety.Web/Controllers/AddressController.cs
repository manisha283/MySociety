using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySociety.Entity.Models;
using MySociety.Service.Interfaces;

namespace MySociety.Web.Controllers;

public class AddressController : Controller
{
    private readonly IFloorService _floorService;
    private readonly IHouseService _houseService;

    public AddressController(IFloorService floorService, IHouseService houseService)
    {
        _floorService = floorService;
        _houseService = houseService;

    }

    [HttpGet]
    public async Task<JsonResult> GetFloor(int blockId)
    {
        IEnumerable<Floor> houses = await _floorService.List(blockId);
        return Json(new SelectList(houses, "Id", "Name"));
    }

    [HttpGet]
    public async Task<JsonResult> GetHouse(int floorId)
    {
        IEnumerable<House> houses = await _houseService.List(floorId);
        return Json(new SelectList(houses, "Id", "Name"));
    }
}
