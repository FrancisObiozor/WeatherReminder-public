let database = [
	{
		day: "0",
		time: "6:00AM"
	},
	{
		day: "1",
		time: "7:00AM"	
	},
	{
		day: "2",
		time: "8:00AM"
	}
]

function saveDataInLocalStorage(database){
	for(let i = 0; i < database.length; i++){
		localStorage.setItem(`${i}`, `${database[i].day},${database[i].time}`);
	}
	localStorage.setItem("length", `${database.length}`);
	console.log()
}

saveDataInLocalStorage(database);
