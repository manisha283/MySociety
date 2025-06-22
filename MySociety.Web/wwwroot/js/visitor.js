$(document).ready(function () {
  paginationAjax(1); // Load users on page load
  setInterval(updateTimeDifference, 100);
  updateTimeDifference();
});

//Deafult Sorting Column and Order
let sortingColumn = "house";
let sortingOrder = "asc";

function paginationAjax(pageNumber) {
  let filter = {
    PageSize: $("#itemsPerPage").val(),
    PageNumber: pageNumber,
    Column: sortingColumn,
    Sort: sortingOrder,
    Search: $("#searchQuery").val(),
    Status: $("#approvalStatus").val(),
    DateRange: $("#dateRange").val(),
    FromDate: $("#fromDate").val(),
    ToDate: $("#toDate").val(),
    CheckOutStatus: $("#checkOutStatus").val(),
    VisitPurpose: $("#visitPurpose").val(),
  };

  $.ajax({
    url: "/Visitor/List",
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

//Approve Visitor
let approveVisitorId = 0;

$(document).on("click", ".btn-approve", function () {
  approveVisitorId = $(this).data("id");
});

$(document).on("click", "#confirmApprove", function () {
  visitorStatus(approveVisitorId, true);
});

//Reject Visitor
let rejectVisitorId = 0;

$(document).on("click", ".btn-reject", function () {
  rejectVisitorId = $(this).data("id");
});

$(document).on("click", "#confirmReject", function () {
  visitorStatus(rejectVisitorId, false);
});

// Change Visitor Status
function visitorStatus(visitorId, isApproved) {
  $.ajax({
    url: "/Visitor/VisitorStatus",
    type: "POST",
    data: {
      id: visitorId,
      isApproved: isApproved,
    },
    success: function (response) {
      if (response.success) {
        toastr.success(response.message);
        paginationAjax(1);
      } else {
        toastr.error(response.message);
      }
    },
    error: function () {
      console.log("Error occurred Vistor status not changed!");
    },
  });
}

function updateTimeDifference() {
  $(".waiting-time").each(function () {
    var $element = $(this);
    var arrivalTime = new Date($element.data("time"));
    var now = new Date();
    var diffInSeconds = Math.floor((now - arrivalTime) / 1000);

    if (diffInSeconds < 0) {
      $element.text("Just now");
      return;
    }

    var hours = Math.floor(diffInSeconds % (24 * 3600));

    // if (hours > 1) {
    //   paginationAjax(1);
    //   return;
    // }

    var minutes = Math.floor((diffInSeconds % 3600) / 60);
    var seconds = diffInSeconds % 60;

    $element.text(`${minutes}m ${seconds}s`);
  });
}

function resetFilters() {
  $("#searchQuery").val("");
  $("#approvalStatus").val("Pending");
  $("#dateRange").val("All");
  $("#checkOutStatus").val("All");
  $("#visitPurpose").val("All");
  $("#fromDate").val("");
  $("#toDate").val("");
  paginationAjax(1);
}

//Applying Filter
$(document).on("change", "#dateRange", function () {
  if ($(this).val() == "CustomDate") {
    $("#fromDate").val("");
    $("#toDate").val("");
    let today = new Date().toISOString().split("T")[0];
    $("#fromDate, #toDate").attr("max", today);
    $("#customDateRange").modal("show");
  } else {
    paginationAjax(1);
  }
});

//Selecting custom date range
$(document).on("click", "#customDateSubmit", function () {
  $("#customDateRange").modal("hide");
  paginationAjax(1);
});

//Close custom date modal
$(document).on("click", "#customDateClose, #customDateCancel", function () {
  $("#customDateRange").modal("hide");
  $("#dateRange").val($("#dateRange option:first").val());
  paginationAjax(1);
});

// Validate Date
$(document).on("change", "#fromDate", function () {
  let fromDate = $(this).val();
  $("#toDate").attr("min", fromDate); // Restrict "To Date" to not be before "From Date"
});

$(document).on("change", "#toDate", function () {
  let toDate = $(this).val();
  $("#fromDate").attr("max", toDate); // Restrict "From Date" to not be after "To Date"
});

$(document).on("change", "#checkOutStatus", function () {
  if ($(this).val() == "All") {
    $("#approvalStatus").val("All");
  } else {
    $("#approvalStatus").val("Approved");
  }

  paginationAjax(1);
});

$(document).on("change", "#approvalStatus", function () {
  if ($(this).val() != "Approved" && $(this).val() != "All") {
    $("#checkOutStatus").val("All");
  }
});

//Check out Visitor
let checkOutVisitorId = 0;

function checkOutModal(element) {
  checkOutVisitorId = $(element).data("id");
  checkOutVisitorName = $(element).data("name");
  $("#nameVisitorModal").text(checkOutVisitorName);
  $("#visitorReviewModal").modal("show");
}

// Select Star
$(document).on("click", ".star", function () {
  let number = $(this).data("number");
  $(this)
    .parent()
    .find(".star")
    .each(function () {
      $(this).addClass("fa-star-o").removeClass("fa-star");
      if ($(this).data("number") <= number) {
        $(this).removeClass("fa-star-o").addClass("fa-star");
      }
    });
});

// Customer Review and Checkout
function checkOut() {
  $.ajax({
    url: "/Visitor/CheckOut",
    method: "POST",
    data: {
      id: checkOutVisitorId,
      rating: Number($(".star-rating.fa-star").length),
      feedback: $("#visitorReview").val(),
    },
    success: function (response) {
      if (response.success) {
        $("#visitorReviewModal").modal("hide");
        toastr.success(response.message);
        paginationAjax(1);
      } else {
        toastr.error(response.message);
      }
    },
    error: function () {
      console.log("Error occurred Vistor status not changed!");
    },
  });
}

// clear all values when visitor review modal is closed
$("#visitorReviewModal").on("hidden.bs.modal", function () {
  $("#nameVisitorModal").text("");
  clearReview();
});

function clearReview() {
  $("#visitorReview").val("");
  $(".star-rating").each(function () {
    $(this).addClass("fa-star-o").removeClass("fa-star");
  });
}
