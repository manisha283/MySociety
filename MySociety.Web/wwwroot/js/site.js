// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Function to reinitialize validation for dynamically added elements
function reinitializeValidation() {
  $("form").each(function () {
    $.validator.unobtrusive.parse($(this));
  });
}

// Call this function after any AJAX request that adds forms dynamically
$(document).ajaxComplete(function () {
  reinitializeValidation();
});

// Apply validation on input change globally
$(document).on(
  "keyup change",
  "form input:not([type=checkbox]):not([type=radio]),form select,form textarea",
  function () {
    $(this).valid();
  }
);

function togglePassword(inputId, icon) {
  var inputField = document.getElementById(inputId);
  if (inputField.type === "password") {
    inputField.type = "text";
    icon.classList.remove("fa-eye-slash");
    icon.classList.add("fa-eye");
  } else {
    inputField.type = "password";
    icon.classList.remove("fa-eye");
    icon.classList.add("fa-eye-slash");
  }
}

$(document).on("click", "#toggleSidebarBtn", function () {
  var sidebar = $("#sidebar");
  if (sidebar.css("display") == "none") {
    sidebar.css("display", "block");
  } else {
    sidebar.css("display", "none");
  }
});

// clear all values in generic modal when it is closed
$("#genericModal").on("hidden.bs.modal", function () {
  $("#genericModalTitle").text("");
  $("#genericModalMessage").text("");
  const yesBtn = $("#genericModalFooter button.btn-yes");
  yesBtn.removeAttr("id").off("click");
});

$(document).on("click", ".sortable-column", function () {
  var $clickedColumn = $(this);
  sortingColumn = $clickedColumn.data("column");
  var currentOrder = $clickedColumn.data("current-order");
  sortingOrder = currentOrder === "asc" ? "desc" : "asc";

  paginationAjax(1);
});

// After new content loads, restore sort indicators
function resetSortingIcon() {
  let $clickedColumn = $(
    '.sortable-column[data-column="' + sortingColumn + '"]'
  );

  // Reset all columns first
  $(".sortable-column")
    .not($clickedColumn)
    .each(function () {
      $(this).data("current-order", "desc");
      $(this).find('[data-order="asc"]').addClass("d-none");
      $(this).find('[data-order="desc"]').addClass("d-none");
    });

  // Apply correct sort icon and data attr
  if (sortingOrder === "asc") {
    $clickedColumn.find('[data-order="asc"]').removeClass("d-none");
    $clickedColumn.find('[data-order="desc"]').addClass("d-none");
  } else {
    $clickedColumn.find('[data-order="asc"]').addClass("d-none");
    $clickedColumn.find('[data-order="desc"]').removeClass("d-none");
  }

  $clickedColumn.data("current-order", sortingOrder);
}

function imageValidation(element, imgSize) {
  const file = element.files[0];
  var $errorSpan = $("#imageValidationSpan");
  $errorSpan.text(""); // Clear previous error

  if (!file) return;

  // Allowed extensions
  var allowedExtensions = ["jpg", "jpeg", "png", "svg", "webp"];
  var fileName = file.name;
  var fileExtension = fileName.split(".").pop().toLowerCase();

  if ($.inArray(fileExtension, allowedExtensions) === -1) {
    $errorSpan.text(
      "Only the following image types are allowed " +
        allowedExtensions.join(", ")
    );
    $(this).val(""); // Clear file input
    return;
  }

  // Max file size (e.g., 1 MB)
  var maxSizeInBytes = imgSize * 1024 * 1024;

  if (file.size > maxSizeInBytes) {
    $errorSpan.text("Image size must be less than 1MB");
    $(this).val(""); // Clear file input
    return;
  }

  // If valid
  $errorSpan.text("");

  // Image preview
  if (file) {
    var reader = new FileReader();
    reader.onload = function (e) {
      $("#img-preview").attr("src", event.target.result);
    };

    reader.readAsDataURL(file);
  }
}

function clearValidation() {
  // Clear field-level validation messages
  $(".field-validation-error")
    .empty()
    .removeClass("field-validation-error")
    .addClass("field-validation-valid");

  // Clear validation summary
  $(".validation-summary-errors")
    .empty()
    .removeClass("validation-summary-errors")
    .addClass("validation-summary-valid");
}
