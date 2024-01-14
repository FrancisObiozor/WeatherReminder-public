function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(successCallback, errorCallback);
    } else {
        console.log("Geolocation is not supported by this browser.");
    }
}

function successCallback(position) {
    const latitude = position.coords.latitude;
    const longitude = position.coords.longitude;

    fetch('/LocationFromBrowser/GetLocation?latitude=' + latitude + '&longitude=' + longitude, {
        method: 'GET'
    })
        .then(response => {
            if (response.ok) {
                console.log("Location data sent to the server successfully.");
                // Handle the response as needed
            } else {
                console.log("Error sending location data to the server.");
                // Handle the error as needed
            }
        })
        .catch(error => {
            console.log("Error sending location data to the server: " + error.message);
            // Handle the error as needed
        });
}

function errorCallback(error) {
    console.log("Error getting the user's location: " + error.message);
    // Handle the error as needed
}

getLocation();
