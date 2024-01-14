let userWelcome = document.getElementById("userWelcome");

function onResize() {
    let height = window.innerHeight;
    let width = window.innerWidth;
    console.log(`width: ${width}`);
    console.log(`height:${height}`);
    if (width < 576) {
        userWelcome.style.display = "none";
    }
    else {
        userWelcome.style.display = "";
    }
}

window.addEventListener('resize', onResize);