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
