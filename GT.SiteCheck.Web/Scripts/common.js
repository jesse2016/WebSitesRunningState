//获取浏览器url参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function geturl(name) {
    var reg = new RegExp("[^\?&]?" + encodeURI(name) + "=[^&]+");
    console.log(window.parent.document.getElementById("frame").contentWindow.location);
    var arr = window.parent.document.getElementById("frame").contentWindow.location.search.match(reg);
    if (arr != null) {
        return decodeURI(arr[0].substring(arr[0].search("=") + 1));
    }
    return "";
}

function toDate(dateString) {
    var DATE_REGEXP = new RegExp("(\\d{4})-(\\d{2})-(\\d{2})([T\\s](\\d{2}):(\\d{2}):(\\d{2})(\\.(\\d{3}))?)?.*");
    if (DATE_REGEXP.test(dateString)) {
        var timestamp = dateString.replace(DATE_REGEXP, function ($all, $year, $month, $day, $part1, $hour, $minute, $second, $part2, $milliscond) {
            var date = new Date($year, $month, $day, $hour || "00", $minute || "00", $second || "00", $milliscond || "00");
            return date.getTime();
        });
        var date = new Date();
        date.setTime(timestamp);
        return date;
    }
    return null;
}

function checkEmail(val) {
    var reg = new RegExp("^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$"); //正则表达式
    var obj = document.getElementById("mazey"); //要验证的对象
    if (!reg.test(val.toLocaleLowerCase())) { //正则验证不通过，格式不对
        return false;
    } else {
        return true;
    }
}