function saveDataInLocalStorage(reminders) {
    let hasBeenCalled = localStorage.getItem("called");
    if (hasBeenCalled != "true") {
        if (reminders.length === 0) {
            localStorage.setItem("length", "0");
            localStorage.setItem("called", "true");
        } else {
            for (let i = 0; i < reminders.length; i++) {
                localStorage.setItem(`${i}`, `${reminders[i].DaysBeforeEvent},${reminders[i].ReminderTime}`);
            }
            localStorage.setItem("length", `${reminders.length}`);
            localStorage.setItem("called", "true");
        }
    }
    
}
saveDataInLocalStorage(reminders);
