let remove = document.getElementsByClassName("remove");
let isReminderInput = document.getElementById("isReminderInput");
let temperatureCutOffInput = document.getElementById("temperatureCutOffInput");

function removeReminder(event) {
    let li = event.target.closest('li');
    let nodes = Array.from(li.closest('ul').children);
    let index = nodes.indexOf(li);
    var url = `/Edit/Temperature?id=${index}&currentIsReminder=${isReminderInput.checked}&currentTempCutoff=${temperatureCutOffInput.value}`;
    window.location.href = url;
}

for (let i = 0; i < remove.length; i++) {
    remove[i].addEventListener("click", removeReminder);
}





