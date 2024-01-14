let add = document.getElementById("add");
let isReminderInput = document.getElementById("isReminderInput");


function addReminder(){
    var url = `/Reminder/AddUmbrella?isReminder=${isReminderInput.checked}`;
    window.location.href = url;
}

add.addEventListener("click", addReminder);
