"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .withAutomaticReconnect()
    .build();

connection.on("ScheduleTestDrives", function () {
    location.href = "/CustomerService/TestDrives";
});

connection.on("UpdateTestDriveStatusForDealer", function () {
    location.reload();
});

connection.on("UpdateTestDriveStatusForCustomer", function () {
    location.href = "/CustomerService/MyTestDrives";
})
connection.start().catch(function (err) {
    return console.error(err.toString());
});
