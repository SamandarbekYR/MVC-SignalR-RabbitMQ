﻿const connection = new signalR.HubConnectionBuilder()
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

    document.getElementById("sendMessageButton").style.display = "none";
    document.getElementById("loadingButton").style.display = "inline-flex";


    const message = document.getElementById("commentmessage").value;
    const selectedUsers = getSelectedUserEmails();
    let totalSuccess = 0;
    let totalFailure = 0;
    let divSuccessCount = document.getElementById('divsuccessfullMessageCount');
    divSuccessCount.textContent = `Successfully sent ${totalSuccess} ...`;
    let divFailedMessageCount = document.getElementById('divFailedMessageCount');
    divFailedMessageCount.textContent = `Failed to send: ${totalFailure}`;
    let divWhoMessageIsSending = document.getElementById('divWhoMessageisSending');
 
    let i = 0;

    function sendNextMessage() {
        if (i >= selectedUsers.length) {
            alert('Message sending process completed!');
            document.getElementById("commentmessage").value = '';

            document.getElementById("loadingButton").style.display = "none";
            document.getElementById("sendMessageButton").style.display = "inline-flex";

            setTimeout(function () {
                document.getElementById('divtoastr').style.display = 'none';
            }, 3000);

            return;
        }

        divWhoMessageIsSending.textContent = `Starting to send messages to ${selectedUsers[i]}`;

        connection.invoke("SendMessageToSelectedUsers", selectedUsers[i], message)
            .then(() => {
                totalSuccess++;
                divSuccessCount.textContent = `Successfully sent to ${totalSuccess} ...`;
                divWhoMessageIsSending.textContent = `Message sent to ${selectedUsers[i]} successfully`
            })
            .catch((err) => {
                console.error("Error sending message: ", err);
                alert('An error occurred while sending the message.');
                totalFailure++;
                divFailedMessageCount.textContent = `Failed to send: ${totalFailure}`;
            })
            .finally(() => {
                i++;
                document.getElementById('divtoastr').style.display = 'block';
                setTimeout(sendNextMessage, 3000);
            });
    }

    sendNextMessage();
});

function getSelectedUserEmails() {
    const checkboxes = document.querySelectorAll(".userCheckbox:checked");
    return Array.from(checkboxes).map(checkbox => checkbox.dataset.email);
}

document.getElementById("viewMessageButton").addEventListener("click", async function (event) {
    event.preventDefault();

    document.getElementById("messageModal").style.display = 'block';
})