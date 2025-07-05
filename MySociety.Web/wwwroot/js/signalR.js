$(document).ready(function () {
  // Establish SignalR connection
  const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .withAutomaticReconnect() // Enable automatic reconnection
    .build();

  // Handle receiving notifications
  // connection.on("ReceiveNotification", function (senderUserId, message) {
  //   updateNotificationCount();
  //   updateNotificationDropdown();
  //   toastr.info(message);

  //   console.log(message);
  //   console.log("function 1");
  //   // const $notificationArea = $("#notificationArea");
  //   // if ($notificationArea.length) {
  //   //   $notificationArea.text(`From User ${senderUserId}: ${message}`).show();
  //   // } else {
  //   //   // Fallback: Show alert if no notification area exists
  //   //   alert(`Notification from ${senderUserId}: ${message}`);
  //   // }

  // });

  connection.on("ReceiveNotification", function (message) {
    updateNotificationCount();
    updateNotificationDropdown();
    toastr.info(message);
    paginationAjax(1);

    console.log(message);
    console.log("function 2");

  });

  // Handle connection close and reconnection
  connection.onclose(function () {
    console.log("SignalR connection closed. Reconnecting...");
  });

  // Start connection
  connection
    .start()
    .then(function () {
      console.log("SignalR connected.");
    })
    .catch(function (err) {
      console.error("SignalR connection error: ", err.toString());
    });

  // Expose a function to send notifications
  window.sendNotification = function (targetUserId, message, callback) {
    if (!targetUserId || !message) {
      alert("Please enter both user ID and message.");
      return;
    }

    connection
      .invoke("SendNotificationToUser", targetUserId, message)
      .then(function () {
        console.log(`Notification sent to user ${targetUserId}`);
        if (callback) callback(true);
      })
      .catch(function (err) {
        console.error("Error sending notification: ", err.toString());
        alert("Failed to send notification.");
        if (callback) callback(false);
      });
  };

  // Expose a function to send server-side notifications via API
  window.sendServerNotification = function (targetUserId, message, callback) {
    if (!targetUserId || !message) {
      alert("Please enter both user ID and message.");
      return;
    }

    $.ajax({
      url: "/api/Notification/send",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify({ targetUserId: targetUserId, message: message }),
      success: function () {
        console.log("Server notification sent.");
        if (callback) callback(true);
      },
      error: function (xhr) {
        console.error("Error sending server notification: ", xhr.responseText);
        alert("Failed to send server notification: " + xhr.responseText);
        if (callback) callback(false);
      },
    });
  };
});


