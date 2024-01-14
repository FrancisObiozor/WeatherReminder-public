let save = document.getElementById("saveLocationOption");
let cityInput = document.getElementById("cityInput");
let stateInput = document.getElementById("stateInput");
let countryInput = document.getElementById("countryInput");
let label = document.getElementsByClassName("radioButtonLabel");
let radioButton = document.getElementsByClassName("radioButton"); 
let noOptionsExist = document.getElementById("noOptionsExist");

if (isLocationMatch === "False" && optionsExist === "True") {
    $('#myModal').modal('show');
}

if (optionsExist === "False") {
    noOptionsExist.innerHTML = "Please enter a valid location.";
} else {
    noOptionsExist.innerHTML = "";
}

function saveLocation(event) {
    let choiceNumber = 0;
    for (let i = 0; i < radioButton.length; i++) {
        if (radioButton[i].checked) {
            choiceNumber = i;
        }
    }

    let userChoice = label[choiceNumber].innerText.split(", ");
    cityInput.value = userChoice[0];
    stateInput.value = userChoice[1];
    countryInput.value = userChoice[2];

    $('#myModal').modal('hide');
}


save.addEventListener("click", saveLocation);
