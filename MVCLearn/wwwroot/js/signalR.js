const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start()
    .then(() => {
        console.log("Connection started successfully.");
    })
    .catch((err) => {
        console.error(`Error starting connection: ${err}`);
    });

document.getElementById("sendMessageButton").addEventListener("click", async function (event) {
    event.preventDefault(); 

    const message = document.getElementById("messageInput").value;
    const selectedUsers = getSelectedUserEmails(); 
    try {
        await connection.invoke("SendMessageToSelectedUsers", selectedUsers, message);
        console.log("Message sent successfully.");
    } catch (err) {
        console.error("Error sending message: ", err);
    }
});

 function getSelectedUserEmails() {
    const checkboxes = document.querySelectorAll(".userCheckbox:checked");
    return Array.from(checkboxes).map(checkbox => checkbox.dataset.email);
}