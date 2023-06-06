"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveLog", function (userName, log) {
    var li = document.createElement("li");
    document.getElementById("chat").appendChild(li);
    li.textContent = userName + " " + log;
});

connection.on("ReceiveMessage", function (user, email, message, dateTime) {
    document.getElementById("message").value = null;

    var li = document.createElement("li");
    document.getElementById("chat").appendChild(li);

    var date = document.createElement("p");
    date.textContent = dateTime;
    date.className = "dateTime";
    li.appendChild(date);

    var userInforDiv = document.createElement("div");
    userInforDiv.className = "chatUserInfo";
    li.appendChild(userInforDiv);

    var chatMessage = document.createElement("p");
    li.appendChild(chatMessage);
    chatMessage.textContent = message;

    var userName = document.createElement("h3");
    userInforDiv.appendChild(userName);
    userName.textContent = user;

    var userMail = document.createElement("p");
    userInforDiv.appendChild(userMail);
    userMail.textContent = email;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

function sendMessage(event) {
    var message = document.getElementById("message").value;
    if (message != null && message != "") {
        var dateTime = new Date().toLocaleDateString("nl") + " " + new Date().toLocaleTimeString("nl", {
            hour12: false,
            hour: "numeric",
            minute: "numeric"
        });
        connection.invoke("SendMessageToGroup", message, dateTime).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
}

document.getElementById("sendButton").addEventListener("click", function (event) {
    sendMessage(event);
});

document.getElementById("message").addEventListener('keypress', function (event) {
    if (event.key === 'Enter') {
        sendMessage(event);
    }
});

var url = window.location.href.split("/");
var group = url[url.length - 1];
connection.invoke("JoinGroup", group).catch(function (err) {
    return console.error(err.toString());
});