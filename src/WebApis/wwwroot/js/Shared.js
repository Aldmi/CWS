"use strict";

function jsonPretty(obj) {
    console.info(obj);
    var pretty = JSON.stringify(obj);
    return pretty;
}



//function jsonPretty(jsonString) {
//    //jsonString= jsonString.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
//    var obj = JSON.parse(jsonString);
//    //console.info(obj.json());//DEBUG
//    console.info(obj);//DEBUG
//    //var pretty = JSON.stringify(obj, null, 2);
//    var pretty = JSON.stringify(obj);
//    return pretty;
//}