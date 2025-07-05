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
    VisitorStatus: $("#visitorStatus").val(),
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
    success: function (response) {
      $("#tableContent").html(response);
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

let redirectToList;

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
        if (redirectToList) {
          // Redirect to list after short delay
          setTimeout(function () {
            window.location.href = "/Visitor/Index";
          }, 1500); // Delay to allow toast to show
        } else {
          paginationAjax(1); // Refresh list
        }
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

function resetFilters() {
  $("#searchQuery").val("");
  $("#visitorStatus").val("Pending");
  $("#dateRange").val("All");
  $("#checkOutStatus").val("All");
  $("#visitPurpose").val("All");
  $("#fromDate").val("");
  $("#toDate").val("");
  $(".customDate").addClass("d-none");
  paginationAjax(1);
}

$(document).on("change", "#checkOutStatus", function () {
  if ($(this).val() == "All") {
    $("#visitorStatus").val("All");
  } else {
    $("#visitorStatus").val("Approved");
  }

  paginationAjax(1);
});

$(document).on("change", "#visitorStatus", function () {
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

function VisitorStatusExpired(id) {
  $.ajax({
    url: "/Visitor/VisitorStatusExpired",
    method: "POST",
    data: {
      id: id,
    },
    success: function () {
      paginationAjax(1);
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

    var minutes = Math.floor((diffInSeconds % 3600));
    var seconds = diffInSeconds % 60;

    $element.text(`${minutes}m ${seconds}s`);

    if (minutes <= 10 && role === "Security") {
      $element
        .closest("tr")
        .find(".btn-approve, .btn-reject")
        .addClass("d-none");
    }

    debugger;

    if (minutes >= 30) {
      VisitorStatusExpired($element.closest("tr").data("id"));
    }
  
  });
}
