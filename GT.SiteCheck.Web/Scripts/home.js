$(document).ready(function () {
    menuFold();
});

function loadTree() {
    $.ajax({
        url: '/Home/LoadTree?' + Math.random(),
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
            $("#menu").empty();
            $("#menu").append(html);
        }
    });
}

function GetNode(data) {
    var html = "";
    html += "<ul class=\"menu\">";
    for (var i = 0; i < data.length; i++) {
        if (i == 0)
        {
            goServerList(data[i].id);
        }
        var item = "<li><p><a onclick=\"goServerList(" + data[i].id + ")\" href=\"javascript:void(0);\">" + data[i].name + "</a></p></li>";

        var childNodeList = data[i].children;
        if (childNodeList != null) {
            if (data[i].level > 0) {
                html += item;
            }
            html += GetNode(childNodeList);
        }
        else {
            if (data[i].level > 0) {
                html += item;
            }
        }
    }
    html += "</ul>";
    return html;
}

var pId = null;
function goServerList(nodeId)
{
    pId = nodeId;
    document.getElementById('frame').src = "/Home/list?nodeId=" + nodeId;
}

$(document).ready(function () {
    loadTree();
});