let add = document.getElementById("add");
let isReminderInput = document.getElementById("isReminderInput");
let temperatureCutOffInput = document.getElementById("temperatureCutOffInput");


function addReminder(){
    var url = `/Reminder/AddTemperature?isReminder=${isReminderInput.checked}&tempCutoff=${temperatureCutOffInput.value}`;
    window.location.href = url;
}

add.addEventListener("click", addReminder);
