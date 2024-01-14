let getLocalTime = document.getElementById("GetLocalTime");
//let test = document.getElementById("test");

let now = new Date();
let month = (parseInt(now.getMonth())+1).toString();
let day = now.getDate();
let year = now.getFullYear();
let hour = now.getHours();
let minute = now.getMinutes();
let seconds = now.getSeconds();

getLocalTime.value = `${month}/${day}/${year} ${hour}:${minute}:${seconds}`;
//test.innerHTML = `${month}/${day}/${year} ${hour}:${minute}:${seconds}`;