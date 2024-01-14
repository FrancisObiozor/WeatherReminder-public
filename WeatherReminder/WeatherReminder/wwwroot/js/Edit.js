let day = document.getElementById("day");
let time = document.getElementById("time");
let save = document.getElementById("save");

function populateEditFields(){
	let index = localStorage.getItem('index');
	let entry = localStorage.getItem(`${index}`);
	let	temp = entry.split(',');
	let editDay = temp[0];
	let editTime = temp[1];
	day.setAttribute("value", editDay);
	time.setAttribute("value", editTime);
}


function editDataFromLocalStorage() {
	let index = localStorage.getItem('index');
	localStorage.setItem(`${index}`, `${day.value},${time.value}`);
}

populateEditFields();
save.addEventListener("click", editDataFromLocalStorage);