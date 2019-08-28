"use strict";

let hubUrl = 'http://localhost:53439/providerHub';
var connection = new signalR.HubConnectionBuilder().withUrl(hubUrl).build();

//Если в течение этого периода сервер не присылает никакого сообщения, то клиент считает, что подключение к серверу разорвано. 
//вызывается событие onclose()
connection.serverTimeoutInMilliseconds = 1000 * 60 * 10; //1000 * 60 * 10

//событие закрытия соединения
connection.onclose(function () {
    console.info("$.onclose Event: " + connection.state);
    //рестарт после 5 сек
    //setTimeout(function () {
    //    connection.start();
    //}, 5000); 
});



connection.on("ReceiveMessage", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = `says ${msg}`;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start()
    .catch(function(err) {
        return console.error(err.toString());
    })
    .then(function () {
        return console.info("CONNECTION IsDone");
    });



document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message)
        .catch(function (err){
            return console.error(err.toString());
        })
        .then(function (res){
            return console.info(res.toString());
        });
    event.preventDefault();
});


document.getElementById("disconnectButton").addEventListener("click", function (event) {
    console.info("$.connection.state: " + connection.state);
    connection.stop();
});

document.getElementById("reconnectButton").addEventListener("click", function (event) {
    if (connection.state === 0) 
    {
        connection.start();
    }  
    else
    {
        connection.stop();
        connection.start();
    }
    console.info("$.connection.hub.state: " + connection.state);
});



document.getElementById("StartStream").addEventListener("click", function (event) {
    connection.stream("Counter", 10, 500)
        .subscribe({
            next: (item) => {
                var li = document.createElement("li");
                li.textContent = item;
                document.getElementById("messagesList").appendChild(li);
            },
            complete: () => {
                var li = document.createElement("li");
                li.textContent = "Stream completed";
                document.getElementById("messagesList").appendChild(li);
            },
            error: (err) => {
                var li = document.createElement("li");
                li.textContent = err;
                document.getElementById("messagesList").appendChild(li);
            },
        });
    event.preventDefault();
});


connection.stream("Counter", 10, 500)
    .subscribe({
        next: (item) => {
            var li = document.createElement("li");
            li.textContent = item;
            document.getElementById("messagesList").appendChild(li);
        },
        complete: () => {
            var li = document.createElement("li");
            li.textContent = "Stream completed";
            document.getElementById("messagesList").appendChild(li);
        },
        error: (err) => {
            var li = document.createElement("li");
            li.textContent = err;
            document.getElementById("messagesList").appendChild(li);
        },
    });