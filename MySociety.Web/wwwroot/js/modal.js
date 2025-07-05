function approveModal(element) {
  const name = $(element).data("name");
  $("#genericModalTitle").text("Approval Confirmation");
  $("#genericModalMessage").text(`Are you sure you want to approve "${name}"?`);
  $("#genericModalFooter button.btn-yes").attr("id", "confirmApprove");
  $("#genericModal").modal("show");
}

function rejectModal(element) {
  const name = $(element).data("name");
  $("#genericModalTitle").text("Rejection Confirmation");
  $("#genericModalMessage").text(`Are you sure you want to reject "${name}"?`);
  $("#genericModalFooter button.btn-yes").attr("id", "confirmReject");
  $("#genericModal").modal("show");
}

function deleteModal(element) {
  const name = $(element).data("name");
  $("#genericModalTitle").text("Delete Confirmation");
  $("#genericModalMessage").text(`Are you sure you want to delete "${name}"?`);
  $("#genericModalFooter button.btn-yes").attr("id", "confirmDelete");
  $("#genericModal").modal("show");
}

function checkOutModal(element) {
  const name = $(element).data("name");
  $("#genericModalTitle").text("Check Out Confirmation");
  $("#genericModalMessage").text(
    `Are you sure you want to check out this visitor - "${name}"?`
  );
  $("#genericModalFooter button.btn-yes").attr("id", "confirmCheckOut");
  $("#genericModal").modal("show");
}


// function approveModal(element) {
//   const name = $(element).data("name");

//   // Set title and message
//   $("#genericModalTitle").text("Approval Confirmation");
//   $("#genericModalMessage").text(`Are you sure you want to approve "${name}"?`);

//   // Optional: Change modal header background color
//   $(".modal-header").removeClass().addClass("modal-header bg-success text-white");

//   // Optional: Change modal body icon
//   // $(".modal-body img").attr("src", "/images/icons/success.svg");

//   // Optional: Change confirm button color
//   $("#genericModalFooter button.btn-yes")
//       .attr("id", "confirmApprove")
//       .removeClass()
//       .addClass("btn btn-success");

//   // Optional: Change cancel button style
//   $("#genericModalFooter button.btn-no")
//       .removeClass()
//       .addClass("btn btn-outline-secondary");

//   // Show the modal
//   $("#genericModal").modal("show");
// }