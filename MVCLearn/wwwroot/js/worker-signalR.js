// SignalR Hub bog'lanishini o'rnatish
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

// SignalR bog'lanishini boshlash
connection.start()
    .then(() => {
        console.log("SignalR bog'lanishi boshlandi.");
    })
    .catch(err => {
        console.error(`Bog'lanishni boshlashda xato: ${err}`);
    });

// Serverdan xabar kelganda chaqiriladigan funksiya
connection.on("ReceiveMessage", function (message) {
    addNotificationToModal(message);
    playNotificationSound();
});

// Modalga yangi xabar qo'shish va bildirishnoma sonini yangilash funksiyasi
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

    //// Modalni avtomatik ravishda ochish
    //$('#notificationsModal').modal('show');
}

// Modalni ochish uchun bildirishnoma qo'ng'irog'ini bosish
document.getElementById("notificationBell").addEventListener("click", function () {
    $('#notificationsModal').modal('show');

    // Modal ochilganda countni 0 ga tushirish
    resetNotificationCount();
});

// Modalni yopish uchun 'x' yoki 'Close' tugmasi bosilganda
$('.close, .btn-secondary').click(function () {
    $('#notificationsModal').modal('hide');
});

// Bildirishnoma sonini 0 ga tushirish funksiyasi
function resetNotificationCount() {
    const countElement = document.getElementById("notificationCount");
    countElement.textContent = "0";
}

// Yangi xabar kelganda ovozli signal chiqishi uchun funksiyani qo'shamiz
function playNotificationSound() {
    const audio = new Audio('/assets/audios/RamBellSound.mp3'); // Ovoz fayli yo'lini to'g'rilash
    audio.play();
}
