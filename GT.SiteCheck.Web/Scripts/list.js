var nodeId = null;

//$(document).ready(function () {
//    menuFold();
//});

function gotoModify()
{
    window.location.href = "/Home/modify?nodeId=" + nodeId;
}

function loadServer() {
    $.ajax({
        url: '/Home/LoadServerByPid',
        data: { pId: nodeId },
        type: 'get',
        dataType: 'json',
        async: true,
        timeout: 10000,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus + "：" + errorThrown);
        },
        success: function (res) {
            var data = eval("(" + res.Data + ")");
            var html = GetNode(data);
            //console.log(html);
            $("#name").text(res.Name);
            $("#serverList").empty();
            $("#serverList").append(html);
        }
    });
}

function GetNode(data) {
    var html = "";
    for (var i = 0; i < data.length; i++) {
        var status = "off";
        if (data[i].status === "200")
        {
            status = "on";
        }
        html += "<li><a href='javascipt:void(0);' style='font-color:black;' title=\"" + data[i].description + "\"><p><q class=\"" + status + "\"></q><i title=\"" + data[i].description + "\">" + data[i].name + "</i></p></a></li>";
    }
    return html;
}

$(document).ready(function () {
    nodeId = window.parent.pId;
    loadServer();
});

$(document).ready(function () {
    setInterval('loadServer()', 30000);
});
