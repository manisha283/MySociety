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
    CategoryId: $("#selectCategoryId").val(),
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

$(".audience-filter-form").on("submit", function (e) {
  $("#selectedAudienceContainer").empty();

  $(".audience-checkbox:checked").each(function (index) {
    const groupType = $(this).data("grouptype");
    const referenceId = $(this).data("referenceid");

    $("#selectedAudienceContainer").append(`
                <input type="hidden" name="SelectedAudience[${index}].GroupTypeId" value="${groupType}" />
                <input type="hidden" name="SelectedAudience[${index}].ReferenceId" value="${referenceId}" />
            `);
  });
});

$("#formSaveNotice").on("submit", function (e) {
  // Check if Custom audience selected
  if ($("#group-type-custom").is(":checked")) {
    const selectedCount = $(".audience-checkbox:checked").length;

    if (selectedCount === 0) {
      e.preventDefault(); // Stop form submission
      alert("Please select at least one target audience.");
      return false;
    }

    // If filters selected, populate hidden fields
    $("#selectedAudienceContainer").empty();

    $(".audience-checkbox:checked").each(function (index) {
      const groupType = $(this).data("group-type");
      const referenceId = $(this).val();

      $("#selectedAudienceContainer").append(`
                    <input type="hidden" name="AudiencesVM.SelectedAudience[${index}].GroupTypeId" value="${groupType}" />
                    <input type="hidden" name="AudiencesVM.SelectedAudience[${index}].ReferenceId" value="${referenceId}" />
                `);
    });
  }

  console.log($("#selectedAudienceContainer"));

  // If All is selected, no validation required
});
