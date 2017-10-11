$(function()
{
    myFunction();
})

function myFunction() {
    var w = window.innerWidth;
    var h = window.innerHeight;
    document.getElementById("WWW").innerHTML = "Width: " + w;
    document.getElementById("HHH").innerHTML = " Height: " + h;
}