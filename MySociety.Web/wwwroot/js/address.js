function getFloor() {
  $.ajax({
    url: "/Address/GetFloor",
    type: "GET",
    data: {
      blockId: $("#selectBlockId").val(),
    },
    success: function (floors) {
      var floorSelect = $("#selectFloorId");
      floorSelect.empty();
      $("#selectHouseId").empty();

      floorSelect.append('<option selected value="-1">Select Floor*</option>');
      $("#selectHouseId").append(
        '<option selected value="">Select House*</option>'
      );

      $.each(floors, function (index, floor) {
        floorSelect.append(
          $("<option/>", {
            value: floor.value,
            text: floor.text,
          })
        );
      });

      floorSelect.prop("disabled", false);
    },
    error: function () {
      alert("Error loading floors.");
    },
  });
}

function getHouse() {
  $.ajax({
    url: "/Address/GetHouse",
    type: "GET",
    data: {
      blockId: $("#selectBlockId").val(),
      floorId: $("#selectFloorId").val(),
    },
    success: function (houses) {
      var houseSelect = $("#selectHouseId");
      houseSelect.empty();

      houseSelect.append('<option selected value="-1">Select House*</option>');

      $.each(houses, function (index, house) {
        houseSelect.append(
          $("<option/>", {
            value: house.value,
            text: house.text,
          })
        );
      });

      houseSelect.prop("disabled", false);
    },
    error: function () {
      alert("Error loading houses.");
    },
  });
}

function clearAddress() {
  $("#selectBlockId").val("-1");
  $("#selectFloorId")
    .val("-1")
    .prop("disabled", true)
    .empty()
    .append('<option selected value="-1">Select Floor*</option>');
  $("#selectHouseId")
    .val("-1")
    .prop("disabled", true)
    .empty()
    .append('<option selected value="-1">Select House*</option>');
}


