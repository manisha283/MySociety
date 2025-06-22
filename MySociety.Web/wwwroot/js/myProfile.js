$(document).ready(function () {
  paginationAjax(1); // Load users on page load
});

//Sorting Column
let sortingColumn = "number";
let sortingOrder = "asc";

function paginationAjax(pageNumber) {
  let filter = {
    PageSize: $("#itemsPerPage").val(),
    PageNumber: pageNumber,
    Column: sortingColumn,
    Sort: sortingOrder,
    Search: $("#searchQuery").val(),
  };

  $.ajax({
    url: "/MyProfile/VehicleList",
    data: { filter },
    type: "POST",
    dataType: "html",
    success: function (data) {
      $("#vehicleTable").html(data);
      resetSortingIcon();
    },
    error: function () {
      $("#vehicleTable").html("An error has occurred");
    },
  });
}

//Get Vehicle Modal
function getVehicle(id) {
  $.ajax({
    url: "/MyProfile/VehicleModal",
    type: "GET",
    data: { vehicleId: id },
    dataType: "html",
    success: function (data) {
      $("#vehicleContent").html(data);
      $("#vehicleModal").modal("show");
    },
    error: function () {
      console.log("There is error.Not successful");
    },
  });
}

//Add/Update Vehicle Form
$(document).on("submit", "#SaveVehicleForm", function (e) {
  e.preventDefault();

  $.ajax({
    url: $(this).attr("action"),
    type: $(this).attr("method"),
    data: $(this).serialize(),
    success: function (response) {
      if (response.success) {
        $("#vehicleModal").modal("hide");
        toastr.success(response.message);
        paginationAjax(1);
      } else {
        toastr.error(response.message);
      }
    },
    error: function () {
      console.log("There is error.Not successful");
    },
  });
});

//Delete Vehicle
let deleteVehicleId = 0;

$(document).on("click", ".btn-delete", function () {
  deleteVehicleId = $(this).data("id");
});

$(document).on("click", "#confirmDelete", function () {
  deleteVehicle(deleteVehicleId);
});

function deleteVehicle(id) {
  $.ajax({
    url: "/MyProfile/DeleteVehicle",
    type: "POST",
    data: { vehicleId: id },
    success: function (response) {
      if (response.success) {
        toastr.success(response.message);
        paginationAjax(1);
      } else {
        toastr.error(response.message);
      }
    },
    error: function () {
      console.log("Error occurred while deleting the Vehicle!");
    },
  });
}
