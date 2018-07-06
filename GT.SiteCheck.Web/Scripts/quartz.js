var num = 10;
var isget = false;

function GetSchedulerInfo() {
    $.ajax({
        url: "/Quartz/GetSchedulerInfo",
        type: 'get',
        dataType: 'json',
        async: true,
        timeout: 100000,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus + "：" + errorThrown);
        },
        success: function (res) {
            //console.log(res.Data);
            var data = eval("(" + res.Data + ")");
            var html = GetJobInfo(data);
            $("#info").empty();
            $("#info").append(html);
        }
    });
}

function GetJobInfo(data) {
    var html = "";
    html += "<span><i>实例名称：</i>" + data.SchedulerName + "</span>";
    html += "<span><i>启动时间：</i>" + GetNewDate(data.RunningSince) + "</span>";
    html += "<span><i>线程数量：</i>" + data.ThreadPoolSize + "</span>";
    html += "<span><i>执行总数：</i>" + data.NumberOfJobsExecuted + "</span>";
    html += "<span><i>自动刷新：</i><span id='time'>"+ num +"秒</span></span>";
    return html;
}

$(document).ready(function () {
    getAllJobs();
});

$(document).ready(function () {
    var html = $("#jobs").html();
    //console.log(html);
    if (html.indexOf('积极拒绝') === -1) {
        setInterval('getAllJobs()', 10000);
    }
});

function setTime()
{
    var tim = setInterval(function () {
        num--;
        //函数每调用一次num减一
        var obj = document.getElementById("time");
        if (obj != undefined) {
            obj.innerHTML = num + "秒";                              
            if (num <= 1 || isget) {                                                             
                clearInterval(tim);
            }
        }
        else {
            clearInterval(tim);
        }
    }, 1000);
}

function getAllJobs() {
    isget = true;
    GetSchedulerInfo();
    $.ajax({
        url: "/Quartz/GetAllJobs",
        type: 'get',
        dataType: 'json',
        async: true,
        timeout: 100000,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(textStatus + "：" + errorThrown);
        },
        success: function (res) {
            //console.log(res.Data);
            if (res.Msg === "") {
                var data = eval("(" + res.Data + ")");
                var html = GetJobs(data);
                $("#jobs").empty();
                $("#jobs").append(html);
                isget = false;
                num = 10;
                setTime();
            }
            else {
                $("#jobs").empty();
                $("#jobs").append(res.Msg);
            }
        }
    });
}

function GetJobs(data) {
    var html = "";
    html += "<table>";
    html += "<tr>";
    html += "<th>Job名称</th>";
    html += "<th>Job组名</th>";
    html += "<th>JOB状态</th>";
    html += "<th>开始时间</th>";
    html += "<th>运行间隔(S)</th>";
    html += "<th>上次运行时间</th>";
    html += "<th>下次运行时间</th>";
    html += "<th>暂停</th>";
    html += "<th>启动</th>";
    html += "<th>移除</th>";
    html += "</tr>";
    for (var i = 0; i < data.length; i++) {
        html += "<tr>";
        html += "<td>" + data[i].jobName + "</td>";
        html += "<td>" + data[i].jobGroup + "</td>";
        html += "<td>" + GetJobState(data[i].trigerState) + "</td>";
        html += "<td>" + GetDate(data[i].startTime) + "</td>";
        html += "<td>" + data[i].timeDiff + "</td>";
        html += "<td>" + GetDate(data[i].preTime) + "</td>";
        html += "<td>" + GetDate(data[i].nextTime) + "</td>";

        html += "<td><a href='javascript:void(0);' onclick=\"handleJob('" + data[i].jobName + "','" + data[i].jobGroup + "','pause')\">暂停</td>";
        html += "<td><a href='javascript:void(0);' onclick=\"handleJob('" + data[i].jobName + "','" + data[i].jobGroup + "','start')\">启动</td>";
        html += "<td><a href='javascript:void(0);' onclick=\"handleJob('" + data[i].jobName + "','" + data[i].jobGroup + "','delete')\">移除</td>";

        html += "</tr>";
    }
    html += "</table>";
    return html;
}

function GetJobState(state) {
    var stateText = "";
    if (state === 0) {
        stateText = "正常";
    }
    if (state === 1) {
        stateText = "暂停";
    }
    if (state === 2) {
        stateText = "完成";
    }
    if (state === 3) {
        stateText = "错误";
    }
    if (state === 4) {
        stateText = "阻塞";
    }
    if (state === 5) {
        stateText = "不存在";
    }
    return stateText;
}
function handleJob(name, group, type) {
    $.ajax({
        url: "/Quartz/handleJob",
        type: 'get',
        data: { name: name, group: group, type: type },
        dataType: 'json',
        async: true,
        timeout: 100000,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus + "：" + errorThrown);
        },
        success: function (res) {
            if (res.Msg != "") {
                alert(res.Msg);
            }
            else {
                layer.alert("成功");
                getAllJobs();
            }
        }
    });
}

function HandleScheduler(type) {
    $.ajax({
        url: "/Quartz/HandleScheduler",
        type: 'get',
        data: { type: type },
        dataType: 'text',
        async: true,
        timeout: 100000,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus + "：" + errorThrown);
        },
        success: function (res) {
            if (res != "") {
                alert(res);
            }
            else {
                layer.alert("成功");
                getAllJobs();
            }
        }
    });
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

function GetDate(dateString) {
    var date = toDate(dateString);
    if (date != null) {
        var d = new Date(date);
        var ptime = d.getFullYear() + '-' + d.getMonth() + '-' + d.getDate() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();

        return ptime;
    }
    else {
        return "";
    }
}

function GetNewDate(dateString) {
    var date = toDate(dateString);
    if (date != null) {
        var d = new Date();
        d.setTime(date.getTime() + 1000 * 60 * 60 * 8);
        var ptime = d.getFullYear() + '-' + d.getMonth() + '-' + d.getDate() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();

        return ptime;
    }
    else {
        return "";
    }
}
