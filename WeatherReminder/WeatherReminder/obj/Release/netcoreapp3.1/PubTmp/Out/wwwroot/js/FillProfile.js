let cityInput = document.getElementById("cityInput");
let stateInput = document.getElementById("stateInput");
let countryInput = document.getElementById("countryInput");
let useCurrentLocation = document.getElementById("useCurrentLocation");
let isEmail = document.getElementById("isEmail");
let isText = document.getElementById("isText");


cityInput.value = city;
stateInput.value = state;
countryInput.value = country;

if (pullLocation === "True") {
    useCurrentLocation.checked = true;
} else {
    useCurrentLocation.checked = false;
}


if (textNotifications === "True") {
    isText.checked = true;
} else {
    isText.checked = false;
}

if (emailNotifications === "True") {
    isEmail.checked = true;
} else {
    isEmail.checked = false;
}