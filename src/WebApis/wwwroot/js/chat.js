"use strict";

//let hubUrl = 'http://192.168.1.35:44138/providerHub';
let hubUrl = '/providerHub';
var connection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl)
    .configureLogging(signalR.LogLevel.Information)
    .build();

//Если в течение этого периода сервер не присылает никакого сообщения, то клиент считает, что подключение к серверу разорвано. 
//вызывается событие onclose()
connection.serverTimeoutInMilliseconds = 1000 * 60 * 10; //1000 * 60 * 10

//событие закрытия соединения
connection.onclose(function () {
    document.getElementById("stateHub").innerHTML = "CONNECTION Close !!!";
    console.info("$.onclose Event: " + connection.state);
    //рестарт после 5 сек
    //setTimeout(function () {
    //    connection.start();
    //}, 5000); 
});


//событие получения данных
connection.on("ReceiveMessage", function (message) {
    var msg = jsonPretty(message);
    var encodedMsg = `CWS: ${msg}`;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});


//Функция подключения к хабу
async function start()
{
    try
    {
        if (connection.state === signalR.HubConnectionState.Disconnected)
        {
            await connection.start();
            document.getElementById("stateHub").innerHTML = "CONNECTION IsDone";
            return console.info("CONNECTION IsDone");
        }
        else
        {
            return console.info("Already Connected !!!");
        }
    } catch (err)
    {
        document.getElementById("stateHub").innerHTML = "CONNECTION Errors: " + err.toString();
        return console.error(err.toString());
    }
};

//Функция отключения от хаба
async function stop()
{
    try
    {
        if (connection.state === signalR.HubConnectionState.Connected)
        {
            await connection.stop();
        }
        else {
            console.info("Already Disconnected !!!");
        }
    } catch (err)
    {
        document.getElementById("stateHub").innerHTML = "Disconnected Errors: " + err.toString();
    }
};


//Подключение к хабу....
start();



//КНОПКА подключения к hub
document.getElementById("connectButton").addEventListener("click", function (event) {
    start();
});

//КНОПКА Разрыва коннекта с hub
document.getElementById("disconnectButton").addEventListener("click", function (event) {
    stop();
});


//КНОПКА отправить тестовой сообщение через hub
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


//using STREAM sending
//document.getElementById("StartStream").addEventListener("click", function (event) {
//    connection.stream("Counter", 10, 500)
//        .subscribe({
//            next: (item) => {
//                var li = document.createElement("li");
//                li.textContent = item;
//                document.getElementById("messagesList").appendChild(li);
//            },
//            complete: () => {
//                var li = document.createElement("li");
//                li.textContent = "Stream completed";
//                document.getElementById("messagesList").appendChild(li);
//            },
//            error: (err) => {
//                var li = document.createElement("li");
//                li.textContent = err;
//                document.getElementById("messagesList").appendChild(li);
//            },
//        });
//    event.preventDefault();
//});


//connection.stream("Counter", 10, 500)
//    .subscribe({
//        next: (item) => {
//            var li = document.createElement("li");
//            li.textContent = item;
//            document.getElementById("messagesList").appendChild(li);
//        },
//        complete: () => {
//            var li = document.createElement("li");
//            li.textContent = "Stream completed";
//            document.getElementById("messagesList").appendChild(li);
//        },
//        error: (err) => {
//            var li = document.createElement("li");
//            li.textContent = err;
//            document.getElementById("messagesList").appendChild(li);
//        },
//    });

