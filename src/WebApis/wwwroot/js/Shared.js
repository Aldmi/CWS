"use strict";

function jsonPretty(jsonString) {
    //jsonString= jsonString.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var obj = JSON.parse(jsonString);
    console.info(obj);//DEBUG
    //var pretty = JSON.stringify(obj, null, 2);
    var pretty = JSON.stringify(obj);
    return pretty;
}