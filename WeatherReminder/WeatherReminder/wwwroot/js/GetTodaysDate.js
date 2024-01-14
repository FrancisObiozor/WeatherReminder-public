let todaysDate = document.getElementById("TodaysDate");

let now = new Date();
//let month = (parseInt(now.getMonth()) + 1).toString();
let month = now.toLocaleDateString('en-US', { month: 'long', });
let day = now.getDate();
let year = now.getFullYear();
let dayOfTheWeek = now.toLocaleDateString('en-US', { weekday: 'long',});

todaysDate.innerHTML = `${dayOfTheWeek}, ${month} ${day} ${year}`;
