// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var blink = document.getElementById("blink");
var inter = setInterval(() =>{
    if(blink.textContent == "Enter Your mail")
    {
        blink.textContent = "Check your mail after submitting ⚠";
    }
    else{

        blink.textContent = "Enter Your mail";
    }
},1000);
inter();



