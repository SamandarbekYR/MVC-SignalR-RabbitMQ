const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start()
    .then(() => {
        console.log("SignalR connection started successfully.");
    })
    .catch((err) => {
        console.error(`Error starting connection: ${err}`);
    });

connection.on("ReceiveMessage", function (message) {
    // Handle receiving a message
    console.log("New message received: ", message);
});

connection.on("MessageStatusUpdated", function (message) {
    // Update the modal with new message status
    updateMessageModal(message);
});

function updateMessageModal(message) {
    const row = document.createElement("tr");
    const receiverCell = document.createElement("td");
    receiverCell.textContent = message.ReceiveName; // Adjust as needed
    const contentCell = document.createElement("td");
    contentCell.textContent = message.MessageContent;
    const sendTimeCell = document.createElement("td");
    sendTimeCell.textContent = new Date(message.SendTime).toLocaleString();
    const readTimeCell = document.createElement("td");
    readTimeCell.textContent = message.ReadTime ? new Date(message.ReadTime).toLocaleString() : "Not Read";
    const statusCell = document.createElement("td");
    statusCell.textContent = message.IsRead ? "Read" : "Not Read";

    row.appendChild(receiverCell);
    row.appendChild(contentCell);
    row.appendChild(sendTimeCell);
    row.appendChild(readTimeCell);
    row.appendChild(statusCell);

    document.getElementById("messagesTable").appendChild(row); // Assuming there's a table with id 'messagesTable'
}
