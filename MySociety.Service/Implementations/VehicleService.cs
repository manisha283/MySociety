using System.ComponentModel;
using System.Linq.Expressions;
using MySociety.Entity.HelperModels;
using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;
using MySociety.Repository.Interfaces;
using MySociety.Service.Common;
using MySociety.Service.Exceptions;
using MySociety.Service.Helper;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class VehicleService : IVehicleService
{
    private readonly IGenericRepository<Vehicle> _vehicleRepository;
    private readonly IGenericRepository<VehicleType> _vehicleTypeRepository;
    private readonly IHttpService _httpService;

    public VehicleService(IGenericRepository<Vehicle> vehicleRepository, IGenericRepository<VehicleType> vehicleTypeRepository, IUserService userService, IHttpService httpService)
    {
        _vehicleRepository = vehicleRepository;
        _vehicleTypeRepository = vehicleTypeRepository;
        _httpService = httpService;
    }

    public async Task<VehicleVM> Get(int vehicleId)
    {
        VehicleVM vehicleVM = new()
        {
            Id = vehicleId,
            Types = _vehicleTypeRepository.GetAll().ToList(),
        };

        Vehicle? vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);

        if (vehicle != null)
        {
            vehicleVM.Id = vehicle.Id;
            vehicleVM.Name = vehicle.Name;
            vehicleVM.Number = vehicle.VehicleNumber;
            vehicleVM.TypeId = vehicle.VehicleTypeId;
            vehicleVM.ParkingSlotNo = vehicle.ParkingSlotNo;
        }

        return vehicleVM;
    }

    public async Task<ResponseVM> CheckNewVehicle(string vehicleNumber)
    {
        ResponseVM response = new();

        Vehicle? vehicle = await _vehicleRepository.GetByStringAsync(v => v.VehicleNumber == vehicleNumber && v.DeletedBy == null);
        if (vehicle == null)
        {
            response.Success = true;
        }
        else
        {
            response.Success = false;
            response.Message = NotificationMessages.AlreadyExisted.Replace("{0}", "Vehicle number");
        }

        return response;
    }

    public async Task<ResponseVM> Save(VehicleVM vehicleVM)
    {
        ResponseVM response = new();

        //Get Vehicle by Id
        Vehicle vehicle = await _vehicleRepository.GetByIdAsync(vehicleVM.Id) ?? new();

        if (vehicle.Id == 0)
        {
            response = await CheckNewVehicle(vehicleVM.Number);
            if (!response.Success)
            {
                return response;
            }
            
            //Add Vehicle
            vehicle.UserId = await _httpService.LoggedInUserId();
            vehicle.CreatedBy = await _httpService.LoggedInUserId();
            vehicle.CreatedAt = DateTime.Now;
        }

        vehicle.VehicleNumber = vehicleVM.Number;
        vehicle.Name = vehicleVM.Name;
        vehicle.VehicleTypeId = vehicleVM.TypeId;
        vehicle.ParkingSlotNo = vehicleVM.ParkingSlotNo;
        vehicle.UpdatedBy = await _httpService.LoggedInUserId();
        vehicle.UpdatedAt = DateTime.Now;

        //Add/Update vehicle in database
        if (vehicle.Id == 0)
        {
            await _vehicleRepository.AddAsync(vehicle);
            response.Success = true;
            response.Message = NotificationMessages.Added.Replace("{0}", "Vehicle");
        }
        else
        {
            await _vehicleRepository.UpdateAsync(vehicle);
            response.Success = true;
            response.Message = NotificationMessages.Updated.Replace("{0}", "Vehicle");
        }

        return response;
    }

    public async Task<VehiclePagination> List(FilterVM filter)
    {
        filter.Search = string.IsNullOrEmpty(filter.Search) ? "" : filter.Search.Replace(" ", "");

        //For sorting the column according to order
        Func<IQueryable<Vehicle>, IOrderedQueryable<Vehicle>>? orderBy = q => q.OrderBy(v => v.Id);

        if (!string.IsNullOrEmpty(filter.Column))
        {
            switch (filter.Column.ToLower())
            {
                case "name":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(v => v.Name) : q => q.OrderByDescending(v => v.Name);
                    break;
                case "number":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(v => v.VehicleNumber) : q => q.OrderByDescending(v => v.VehicleNumber);
                    break;
                case "type":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(v => v.VehicleType.Name) : q => q.OrderByDescending(v => v.VehicleType.Name);
                    break;
                case "parkingSlotNo":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(v => v.ParkingSlotNo) : q => q.OrderByDescending(v => v.ParkingSlotNo);
                    break;
                default:
                    break;
            }
        }

        int userId = await _httpService.LoggedInUserId();

        DbResult<Vehicle> dbResult = await _vehicleRepository.GetRecords(
            predicate: v => v.UserId == userId && v.DeletedBy == null &&
            (string.IsNullOrEmpty(filter.Search.ToLower()) ||
                            v.Name.ToLower().Contains(filter.Search.ToLower()) ||
                            v.VehicleNumber.ToLower().Contains(filter.Search.ToLower())),
            orderBy: orderBy,
            includes: new List<Expression<Func<Vehicle, object>>>
            {
                v => v.VehicleType
            },
            pageSize: filter.PageSize,
            pageNumber: filter.PageNumber
        );

        VehiclePagination vehicles = new()
        {
            Vehicles = dbResult.Records.Select(r => new VehicleVM
            {
                Id = r.Id,
                Name = r.Name,
                Number = r.VehicleNumber,
                TypeName = r.VehicleType.Name,
                ParkingSlotNo = r.ParkingSlotNo
            }).ToList()
        };

        vehicles.Page.SetPagination(dbResult.TotalRecord, filter.PageSize, filter.PageNumber);

        return vehicles;
    }

    public async Task Delete(int vehicleId)
    {
        Vehicle vehicle = await _vehicleRepository.GetByIdAsync(vehicleId)
                    ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "vehicle"));

        vehicle.DeletedBy = await _httpService.LoggedInUserId();
        vehicle.DeletedAt = DateTime.Now;

        await _vehicleRepository.UpdateAsync(vehicle);
    }
}
