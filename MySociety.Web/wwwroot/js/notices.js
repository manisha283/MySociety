$(document).ready(function () {
  paginationAjax(1); // Load users on page load
});

//Deafult Sorting Column and Order
let sortingColumn = "createdat";
let sortingOrder = "desc";

function paginationAjax(pageNumber) {
  let filter = {
    PageSize: $("#itemsPerPage").val(),
    PageNumber: pageNumber,
    Column: sortingColumn,
    Sort: sortingOrder,
    Search: $("#searchQuery").val(),
    DateRange: $("#dateRange").val(),
    FromDate: $("#fromDate").val(),
    ToDate: $("#toDate").val(),
    CategoryId: $("#selectCategoryId").val()
  };

  $.ajax({
    url: "/Notices/List",
    data: { filter },
    type: "POST",
    dataType: "html",
    success: function (response) {
      $("#tableContent").html(response);
      resetSortingIcon();
    },
    error: function () {
      $("#tableContent").html("An error has occurred");
    },
  });
}

function resetFilters() {
  $("#searchQuery").val("");
  $("#dateRange").val("All");
  $("#selectCategoryId").val(-1);
  $("#fromDate").val("");
  $("#toDate").val("");
  $(".customDate").addClass("d-none");
  paginationAjax(1);
}

//Delete Vehicle
let deleteNoticeId = 0;

$(document).on("click", ".btn-delete", function () {
  deleteNoticeId = $(this).data("id");
});

$(document).on("click", "#confirmDelete", function () {
  deleteNotice(deleteNoticeId);
});

function deleteNotice(id) {
  $.ajax({
    url: "/Notices/Delete",
    type: "POST",
    data: { id: id },
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