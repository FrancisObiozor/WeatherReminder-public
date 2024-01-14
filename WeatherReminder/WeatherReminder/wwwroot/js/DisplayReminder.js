import pullRemindersFromLocalStorage from './PullReminder.js';

//let ul = document.getElementsByTagName("ul");
//let li = document.getElementsByTagName("li");

function displayReminders() {
    let reminders = pullRemindersFromLocalStorage();
    let dayString = "days";

    let ulContainer = document.getElementById("ulContainer");
    let newUL = document.createElement("ul");
    newUL.className = "list-group";
    //newUL.style.left = "-20px";
    //newUL.style.position = "relative";
    ulContainer.appendChild(newUL);


    if (reminders.length === 0) {
        let ul = document.getElementsByTagName("ul");
        let newLI = document.createElement("li");
        newLI.className = "list-group-item";
        newLI.innerHTML = `<div class="inline text-danger">You have no reminders. Click "Edit Reminders" to manager your reminders.</div>`;
        ul[2].appendChild(newLI);
    } else {
        for (let i = 0; i < reminders.length; i++) {
            if (reminders[i].day == 1) {
                dayString = "day";
            } else {
                dayString = "days";
            }

            let ul = document.getElementsByTagName("ul");
            let newLI = document.createElement("li");
            newLI.className = "list-group-item";
            newLI.innerHTML =
                `<div class ="container">
				<div class = "row">
					<div class ="col-md-8">
						<div class="inline text-info">${reminders[i].DaysBeforeEvent} ${dayString} before Event</div> at <div class="inline text-info">${reminders[i].ReminderTime}</div>
					</div>
					
				</div>
			</div>`;
            ul[2].appendChild(newLI);
        }
    }
}

displayReminders();