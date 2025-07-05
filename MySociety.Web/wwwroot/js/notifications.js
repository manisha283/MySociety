$(document).ready(function () {
  paginationAjax(1); // Load users on page load
});

//Sorting Column
let sortingColumn = "block";
let sortingOrder = "asc";

function paginationAjax(pageNumber) {
  let filter = {
    PageSize: $("#itemsPerPage").val(),
    PageNumber: pageNumber,
    Column: sortingColumn,
    Sort: sortingOrder,
    DateRange: $("#dateRange").val(),
    FromDate: $("#fromDate").val(),
    ToDate: $("#toDate").val(),
    Search: $("#searchQuery").val(),
    ReadStatus: $("#selectReadStatus").val(),
    Category: $("#selectNotificationCategory").val(),
  };

  console.log(filter);

  $.ajax({
    url: "/Notifications/List",
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

function resetFilters() {
  $("#searchQuery").val("");
  $("#selectReadStatus").val("Unread");
  $("#dateRange").val("All");
  $("#fromDate").val("");
  $("#toDate").val("");
  $(".customDate").addClass("d-none");
  paginationAjax(1);
}
