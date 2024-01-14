let cityInput = document.getElementById("cityInput");
let stateInput = document.getElementById("stateInput");
let countryInput = document.getElementById("countryInput");
let useCurrentLocation = document.getElementById("useCurrentLocation");

function useLocation() {
    if (useCurrentLocation.checked) {
        cityInput.value = IpCity;
        stateInput.value = IpState;
        countryInput.value = IpCountry;
    }    
}

useCurrentLocation.addEventListener("click", useLocation);