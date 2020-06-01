"use strict";

function jsonPretty(jsonString) {
    //jsonString= jsonString.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var obj = JSON.parse(jsonString);
    //console.info(obj.json());//DEBUG
    console.info(obj);//DEBUG
    //var pretty = JSON.stringify(obj, null, 2);
    var pretty = JSON.stringify(obj);
    return pretty;
}



//function jsonPretty(jsonString) {
//    var obj  = {"DeviceName":"NewDevice_1","KeyExchange":"NewDevice_1 TcpIp = 50000 Addr = 1","DataAction":1,"TimeAction":2006,"IsValidAll":true,"ResponsesItems":[{"MessageDict":{"RuleName":"Data","viewRule.Id":"1","GetDataByte.Request":"[023244203134205E205E202020202020202020202010033637]Lenght = 50 Format = HEX","GetDataByte.RequestBase":"[\u00022D 14 ^ ^ 0x10\u000367]Lenght = 28 Format = Windows - 1251","GetDataByte.ByteRequest":"023244203134205E205E202020202020202020202010033637 Lenght = 25","TimeResponse":"2000"},"Status":4,"StatusStr":"End","ResponseInfo":{"RealData":{"Str":"ok","Format":"UTF - 8"},"IsOutDataValid":true},"ProcessedItemsInBatch":{"StartItemIndex":0,"BatchSize":1,"ProcessedItems":[{"InDataItem":{"ScheduleId":0,"TrnId":0,"Lang":0,"ExpectedTime":"0001 - 01 - 01T00: 00: 00","Id":0},"InseartedData":{"NumberOfTrain":" ","StationArrival":" ","StationDeparture":" ","Note":" ","TypeAlias":" ","DelayTime":" ","VagonDirection":" "}}]}}]}   

//    console.info(obj);//DEBUG
//    //var pretty = JSON.stringify(obj, null, 2);
//    var pretty = JSON.stringify(obj);
//    return pretty;
//}