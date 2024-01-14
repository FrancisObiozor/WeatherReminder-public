let remove = document.getElementsByClassName("remove");

function removeReminder(event) {
    let li = event.target.closest('li');
    let nodes = Array.from(li.closest('ul').children);
    let index = nodes.indexOf(li);
    var url = `/Edit/Reminders?id=${index}`;
    window.location.href = url;
}

for (let i = 0; i < remove.length; i++) {
    remove[i].addEventListener("click", removeReminder);
}

