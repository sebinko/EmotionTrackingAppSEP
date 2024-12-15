window.modalHelper = {
  showModal: function (modalId) {
    var modalElement = document.getElementById(modalId);
    var modal = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
    modal.show();
  },
  hideModal: function (modalId) {
    var modalElement = document.getElementById(modalId);
    var modal = bootstrap.Modal.getInstance(modalElement);
    if (modal) {
      modal.hide();
    }
  }
};