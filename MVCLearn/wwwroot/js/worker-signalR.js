const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start()
    .then(() => {
        console.log("SignalR connection started.");
    })
    .catch(err => {
        console.error(`Error starting connection: ${err}`);
    });

connection.on("ReceiveMessage", function (message) {
    addNotificationToModal(message);
});

function addNotificationToModal(message) {
    const row = document.createElement("tr");
    const dateCell = document.createElement("td");
    dateCell.textContent = new Date().toLocaleString(); 
    const messageCell = document.createElement("td");
    messageCell.textContent = message;

    row.appendChild(dateCell);
    row.appendChild(messageCell);

    document.getElementById("notificationsTable").appendChild(row);

    const countElement = document.getElementById("notificationCount");
    countElement.textContent = parseInt(countElement.textContent) + 1;

    $('#notificationsModal').modal('show');
}
document.getElementById("notificationBell").addEventListener("click", function () {
    $('#notificationsModal').modal('show');
});
