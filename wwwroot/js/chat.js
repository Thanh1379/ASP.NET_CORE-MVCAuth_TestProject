"use strict"
//import { signalR } from "./signalr/dist/browser/signalr";
var connection = new signalR.HubConnectionBuilder().withUrl("/Chat").build();
console.log(connection);
//Disable send button until connection is established
document.getElementById("send-button").disabled = true;

connection.on("ReceiveMessage", function(user, message){
    var div = document.createElement("div");
    div.className = "message";
    document.getElementById("message-display").appendChild(div);
    div.textContent = `${user}: ${message}`;
})

connection.start().then(function() {
    document.getElementById("send-button").disabled = false;
}).catch(function(error) {
    console.log(error);
});

document.getElementById("send-button").addEventListener("click", function(event){
    var user = document.getElementById("user-name").textContent;
    var message = document.getElementById("message-input").value;
    connection.invoke("SendMessage", user, message).catch(function(error){
        return console.log(error);
    });
    event.preventDefault();
})