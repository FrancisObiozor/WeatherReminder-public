export default function pullRemindersFromLocalStorage() {
    let reminders = [];
    let length = localStorage.getItem("length");

    for (let i = 0; i < length; i++) {
        let entry = "";
        let temp = [];

        entry = localStorage.getItem(`${i}`);
        temp = entry.split(',');
        reminders[i] = {
            DaysBeforeEvent: temp[0],
            ReminderTime: temp[1]
        };
    }
    return reminders;
}
