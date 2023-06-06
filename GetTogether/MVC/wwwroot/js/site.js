// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// regitser page get inital from name input field
function getFirstLetter() {
    var inputField = document.getElementById("nameInput");
    var inital = document.getElementById("nameInitial");

    if (inputField.value != null && inputField.value != "") {
        inital.innerHTML = inputField.value[0].toUpperCase();
    } else {
        inital.innerHTML = "?";
    }
}
