$(function()
{
    ShowScreenDimensions();
})

function ShowScreenDimensions() {
    var path = window.location.pathname;
    if (path.toLowerCase() == "/home/devresources") {
        var w = window.innerWidth;
        var h = window.innerHeight;
        document.getElementById("WWW").innerHTML = "Width: " + w;
        document.getElementById("HHH").innerHTML = " Height: " + h;
    }
}