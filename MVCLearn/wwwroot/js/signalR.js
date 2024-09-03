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


connection.on("UpdateUserStatus", (email, isOnline) => {
    const userElement = document.querySelector(`[data-email='${email}']`);
    if (userElement) {
        const statusElement = userElement.closest('tr').querySelector('.status-text');
        if (statusElement) {
            if (isOnline) {
                statusElement.classList.remove('text-red-500');
                statusElement.classList.add('text-green-500');
                statusElement.textContent = 'Online';
            } else {
                statusElement.classList.remove('text-green-500');
                statusElement.classList.add('text-red-500');
                statusElement.textContent = 'Offline';
            }
        }
    }
});


document.getElementById("sendMessageButton").addEventListener("click", async function (event) {
    event.preventDefault();

    const message = document.getElementById("commentmessage").value;
    const selectedUsers = getSelectedUserEmails();
    try {
        await connection.invoke("SendMessageToSelectedUsers", selectedUsers, message);
        console.log("Message sent successfully.");
        alert('Message sent successfully!');
        document.getElementById("commentmessage").value = '';
    } catch (err) {
        console.error("Error sending message: ", err);
        alert('An error occurred while sending the message.');
    }
});

function getSelectedUserEmails() {
    const checkboxes = document.querySelectorAll(".userCheckbox:checked");
    return Array.from(checkboxes).map(checkbox => checkbox.dataset.email);
}
