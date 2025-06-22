$(document).ready(function () {
  paginationAjax(1); // Load users on page load
});

//Sorting Column
let sortingColumn = "name";
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
    url: "/AdminPanel/UserList",
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
    url: "/AdminPanel/ChangeUserStatus",
    type: "POST",
    data: {
      userId: userId,
      isApprove: isApprove,
    },
    success: function (response) {
      if (response.success) {
        toastr.success(response.message);
        paginationAjax(1); //to show latest change in list
      } else {
        toastr.error(response.message);
      }
    },
    error: function () {
      console.error("Error occurred while changing user status!");
    },
  });
}
