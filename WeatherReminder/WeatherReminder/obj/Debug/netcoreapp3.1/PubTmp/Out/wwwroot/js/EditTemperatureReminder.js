let edit = document.getElementsByClassName("edit");
let isReminderInput = document.getElementById("isReminderInput");
let temperatureCutOffInput = document.getElementById("temperatureCutOffInput");

function saveListIndex(event) {
    let li = event.target.closest('li');
    let nodes = Array.from(li.closest('ul').children);
    let index = nodes.indexOf(li);
    var url = `/Reminder/EditTemperature?id=${index}&isReminder=${isReminderInput.checked}&temperatureCutoff=${temperatureCutOffInput.value}`;
    window.location.href = url;
}

for (let i = 0; i < edit.length; i++) {
    edit[i].addEventListener("click", saveListIndex);
}







