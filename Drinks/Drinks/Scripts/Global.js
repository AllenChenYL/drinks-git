$(function () {
    kendo.culture('zh-TW');
});
//CustomPrompt
function CustomPrompt() {
    this.render = function (dialog, fctn) {
        var winW = window.innerWidth;
        var winH = window.innerHeight;
        var dialogoverlay = document.getElementById('dialogoverlay');
        var dialogbox = document.getElementById('dialogbox');
        dialogoverlay.style.display = "block";
        dialogoverlay.style.height = winH + "px";
        dialogbox.style.left = (winW / 2) - (560 * .5) + "px";
        dialogbox.style.top = "100px";
        dialogbox.style.display = "block";
        document.getElementById('dialogboxhead').innerHTML = "飲料店目錄";
        document.getElementById('dialogboxbody').innerHTML = dialog;
        document.getElementById('dialogboxbody').innerHTML += '<br><input id="prompt_value1" value="https://i0.wp.com/img.huablog.tw/uploads/20200425124913_39.jpg"/>';
        document.getElementById('dialogboxfoot').innerHTML = '<button onclick="promot.ok(\'' + fctn + '\')">OK</button> <button onclick="promot.cancel()">Cancel</button>';
    }
    this.cancel = function () {
        document.getElementById('dialogoverlay').style.display = "none";
        document.getElementById('dialogbox').style.display = "none";
    }
    this.ok = function (fctn) {
        var prompt_value1 = document.getElementById('prompt_value1').value;
        window[fctn](prompt_value1);
        document.getElementById('dialogoverlay').style.display = "none";
        document.getElementById('dialogbox').style.display = "none";
    }
}
var promot = new CustomPrompt();


//可在Javascript中使用如同C#中的string.format
//使用方式 : var fullName = String.format('Hello. My name is {0} {1}.', 'FirstName', 'LastName');
String.format = function () {
    var s = arguments[0];
    if (s == null) return "";
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = getStringFormatPlaceHolderRegEx(i);
        s = s.replace(reg, (arguments[i + 1] == null ? "" : arguments[i + 1]));
    }
    return cleanStringFormatResult(s);
}
//可在Javascript中使用如同C#中的string.format (對jQuery String的擴充方法)
//使用方式 : var fullName = 'Hello. My name is {0} {1}.'.format('FirstName', 'LastName');
String.prototype.format = function () {
    var txt = this.toString();
    for (var i = 0; i < arguments.length; i++) {
        var exp = getStringFormatPlaceHolderRegEx(i);
        txt = txt.replace(exp, (arguments[i] == null ? "" : arguments[i]));
    }
    return cleanStringFormatResult(txt);
}
//讓輸入的字串可以包含{}
function getStringFormatPlaceHolderRegEx(placeHolderIndex) {
    return new RegExp('({)?\\{' + placeHolderIndex + '\\}(?!})', 'gm')
}
//當format格式有多餘的position時，就不會將多餘的position輸出
//ex:
// var fullName = 'Hello. My name is {0} {1} {2}.'.format('firstName', 'lastName');
// 輸出的 fullName 為 'firstName lastName', 而不會是 'firstName lastName {2}'
function cleanStringFormatResult(txt) {
    if (txt == null) return "";
    return txt.replace(getStringFormatPlaceHolderRegEx("\\d+"), "");
}

function isRequired(str) {
    //能判斷是否為 null undefined 0
    if (!str) {
        return false;
    }
    //頭尾所有空白刪除 \s為空白
    if(str.replace(/(^\s*)|(\s*$)/g, '').length == 0) {
        return false;
    }
    return true;
}
