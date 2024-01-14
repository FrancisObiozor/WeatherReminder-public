let edit = document.getElementsByClassName("edit");
let isReminderInput = document.getElementById("isReminderInput");

function saveListIndex(event) {
    let li = event.target.closest('li');
    let nodes = Array.from(li.closest('ul').children);
    let index = nodes.indexOf(li);
    var url = `/Reminder/EditUmbrella?id=${index}&isReminder=${isReminderInput.checked}`;
    window.location.href = url;
}

for (let i = 0; i < edit.length; i++) {
    edit[i].addEventListener("click", saveListIndex);
}







