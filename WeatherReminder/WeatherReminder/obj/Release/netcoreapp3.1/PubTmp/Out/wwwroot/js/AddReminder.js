let add = document.getElementById("add");

function addReminder(){
    var url = `/Reminder/Add`;
    window.location.href = url;
}

add.addEventListener("click", addReminder);
