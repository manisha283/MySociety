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
  $(document).on("keyup change", "form input:not([type=checkbox]):not([type=radio]),form select,form textarea", function () {
    $(this).valid();
  });

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
