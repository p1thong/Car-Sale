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
});

// connection.on("OrderStatusChanged", function () {
//     location.href = "/CustomerService/MyTestDrives";
// });

// Nếu đang ở trang Dealer
if (window.location.pathname.startsWith("/DealerOrder")) {
    connection.on("ShowOrdersForDealer", function () {
        location.href = "/DealerOrder/AllOrders";
    });
    connection.on("CustomerPayOrder", function () {
        location.href = "/DealerOrder/AllOrders";
    });
    connection.on("CustomerConfirmReceived", function () {
        const pathParts = window.location.pathname.split("/");
        const orderId = pathParts[pathParts.length - 1];
        location.href = `/DealerOrder/OrderDetail/${orderId}`;
    });
}

// Nếu đang ở trang Customer
if (window.location.pathname.startsWith("/Customer")) {
    connection.on("CustomerCreateOrder", function () {
        location.href = "/Customer/MyOrders";
    });
    connection.on("DealerConfirmOrRejectOrder", function () {
        if (window.location.pathname.startsWith("/Customer/OrderDetail")) {
            const orderId = pathParts[pathParts.length - 1];
            location.href = `/Customer/OrderDetail/${orderId}`;
        } else if (window.location.pathname === "/Customer/MyOrders") {
            location.href = "/Customer/MyOrders";
        }
    });
    connection.on("DealerUpdateDeliveryStatus", function () {
        const pathParts = window.location.pathname.split("/");
        if (window.location.pathname.startsWith("/Customer/OrderDetail")) {
            const orderId = pathParts[pathParts.length - 1];
            location.href = `/Customer/OrderDetail/${orderId}`;
        } else if (window.location.pathname === "/Customer/MyOrders") {
            location.href = "/Customer/MyOrders";
        }
    });
}

connection.on("SubmitFeedBack", function () {
    location.href = "/CustomerService/Feedbacks";
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});
