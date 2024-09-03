// SignalR Hub bog'lanishini o'rnatish
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start()
    .then(() => {
        console.log("SignalR bog'lanishi boshlandi.");
    })
    .catch(err => {
        console.error(`Bog'lanishni boshlashda xato: ${err}`);
    });

connection.on("ReceiveMessage", function (message) {
    addNotificationToModal(message);
    playNotificationSound();
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
}

document.getElementById("notificationBell").addEventListener("click", function () {
    $('#notificationsModal').modal('show');

    resetNotificationCount();
});

$('.close, .btn-secondary').click(function () {
    $('#notificationsModal').modal('hide');
});

function resetNotificationCount() {
    const countElement = document.getElementById("notificationCount");
    countElement.textContent = "0";
}

function playNotificationSound() {
    const audio = new Audio('/assets/audios/RamBellSound.mp3'); 
    audio.play();
}
