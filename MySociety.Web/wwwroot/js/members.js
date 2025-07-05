$(document).ready(function () {
  paginationAjax(1); // Load users on page load
});

//Sorting Column
let sortingColumn = "block";
let sortingOrder = "asc";

let userStatus = "pending";

function paginationAjax(pageNumber) {
  let filter = {
    PageSize: $("#itemsPerPage").val(),
    PageNumber: pageNumber,
    Column: sortingColumn,
    Sort: sortingOrder,
    Search: $("#searchQuery").val(),
    Status: userStatus,
    RoleId: $("#selectRole").val(),
    BlockId: $("#selectBlock").val(),
    FloorId: $("#selectFloor").val(),
    HouseId: $("#selectHouse").val(),
    DateRange: $("#dateRange").val(),
    FromDate: $("#fromDate").val(),
    ToDate: $("#toDate").val(),
  };

  $.ajax({
    url: "/Members/List",
    data: { filter },
    type: "POST",
    dataType: "html",
    success: function (data) {
      $("#tableContent").html(data);
      resetSortingIcon();
    },
    error: function () {
      $("#tableContent").html("An error has occurred");
    },
  });
}

//Approve user
let approveUserId = 0;

$(document).on("click", ".btn-approve", function () {
  approveUserId = $(this).data("id");
});

$(document).on("click", "#confirmApprove", function () {
  changeUserStatus(approveUserId, true);
});

//Reject user
let rejectUserId = 0;

$(document).on("click", ".btn-reject", function () {
  rejectUserId = $(this).data("id");
});

$(document).on("click", "#confirmReject", function () {
  changeUserStatus(rejectUserId, false);
});

//Change user status from pending to approve or reject
function changeUserStatus(userId, isApprove) {
  $.ajax({
    url: "/Members/ChangeUserStatus",
    type: "POST",
    data: {
      userId: userId,
      isApprove: isApprove,
    },
    success: function (response) {
      if (response.success) {
        paginationAjax(1);
        toastr.success(response.message);
        if (redirectToList) {
          // Redirect to list after short delay
          setTimeout(function () {
            window.location.href = "/Members/Index";
          }, 1500); // Delay to allow toast to show
        } else {
          console.log("status changed");
          paginationAjax(1); // Refresh list
        }
      } else {
        toastr.error(response.message);
      }
    },
    error: function () {
      console.error("Error occurred while changing user status!");
    },
  });
}

function filterByStatus(status) {
  userStatus = status;
  paginationAjax(1);
}

$(document).on("change", "#selectRole", function () {
  if ($(this).val() == 4) {
    $("#selectBlock").prop("disabled", true);
    $("#selectFloor").prop("disabled", true);
    $("#selectHouse").prop("disabled", true);
  } else {
    $("#selectBlock").prop("disabled", false);
    $("#selectFloor").prop("disabled", false);
    $("#selectHouse").prop("disabled", false);
  }
  paginationAjax(1);
});

function resetFilters() {
  $("#searchQuery").val("");
  $("#status").val("Pending");
  $("#dateRange").val("All");
  $("#selectRole").val(-1);
  $("#selectBlock").val(-1);
  $("#selectFloor").val(-1);
  $("#selectHouse").val(-1);
  $("#fromDate").val("");
  $("#toDate").val("");
  $(".customDate").addClass("d-none");
  paginationAjax(1);
}