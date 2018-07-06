layer.config({
    skin: 'my-skin'
});

var popBox = new WarningBox();
$(document).ready(function () {
    //弹窗
    //增加
    document.getElementById("addition").onclick = function () {
        popBox.show("warningBox01");
    }
    document.getElementById("Close01").onclick = function () {
        popBox.hide("warningBox01");
    }
    document.getElementById("Submit01").onclick = function () {
        popBox.hide("warningBox01");
    }
    //关闭
    document.getElementById("closeServer").onclick = function () {
        var idstr = getAllId();
        if (idstr === "") {
            showMsg("请选择要关闭的服务器");
            return;
        }
        popBox.show("warningBox02");
    }
    document.getElementById("Close02").onclick = function () {
        popBox.hide("warningBox02");
    }
    document.getElementById("Submit02").onclick = function () {
        popBox.hide("warningBox02");
    }
    document.getElementById("Close03").onclick = function () {
        popBox.hide("warningBox03");
    }
    document.getElementById("Submit03").onclick = function () {
        popBox.hide("warningBox03");
    }
    //开启服务器
    document.getElementById("openServer").onclick = function () {
        var idstr = getAllId();
        if (idstr === "") {
            showMsg("请选择要开启的服务器");
            return;
        }
        popBox.show("warningBox04");
    }
    document.getElementById("Close04").onclick = function () {
        popBox.hide("warningBox04");
    }
    document.getElementById("Submit04").onclick = function () {
        popBox.hide("warningBox04");
    }

    document.getElementById("openJobPanel").onclick = function () {
        layer.open({
            type: 2,
            //area: ['1070px', '550px'],
            area: ['100%', '100%'],
            fixed: false, //不固定
            maxmin: true,
            title: '站点检测JOB管理',
            content: '/Quartz/Index'
        });
    }
});

var pageCount = 0;//总页数
var articleCount = 0;//文章总数
var currentPages = 0;
var parentId = 0;//父id

function getServerList(currentPage) {
    parentId = window.parent.pId;
    $.ajax({
        url: '/Home/GetServerListByPId?' + Math.random(),
        type: 'get',
        data: { parentId: parentId, pageSize: 10, currentPage: currentPage },
        dataType: 'json',
        async: true,
        timeout: 10000,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(textStatus + "：" + errorThrown);
        },
        success: function (res) {
            articleCount = res.ArticleCount;
            pageCount = res.PageCount;
            currentPages = res.CurrentPage;

            var data = eval("(" + res.Data + ")");
            var html = GetSiteHtml(data, parentId);

            $("#modifyList").empty();
            $("#modifyList").append(html);

            $("#pagenumBox").empty();
            if (data.length > 0) {
                $("#pagenumBox").append(GetPageBar(currentPages));
            }
        }
    });
}

function GetSiteHtml(data, parentId) {
    var html = "";
    for (var i = 0; i < data.length; i++) {
        var date = toDate(data[i].createDate);
        //console.log(date);

        var d = new Date(date);
        var createDate = d.getFullYear() + '-' + d.getMonth() + '-' + d.getDate();

        var isuse = "";
        if (data[i].isuse === 1) {
            isuse = "已启用";
        }
        else {
            isuse = "已关闭";
        }

        var status = "";
        if (data[i].status != null)
        {
            status = data[i].status;
        }

        var type = "";
        if (data[i].type === 1) {
            type = "RESTFUL";
        }
        if (data[i].type === 2) {
            type = "WebService";
        }

        html += "<li>";
        html += "<p><input type=\"checkbox\" /><i style=\"display:none\" hidden=\"hidden\">" + data[i].id + "</i><i title=\"" + data[i].name + "\"><a href='javascript:void(0);' onclick=\"GetServerInfo(" + data[i].id + ")\">" + data[i].name + "</a></i></p>";
        html += "<em><i>URL：" + data[i].url + "</i><i>URL类型：" + type + "</i><i>轮询间隔：" + data[i].interval + "s</i><i>" + isuse + "</i><i>上次检测状态：" + status + "</i></em>";
        html += "</li>";
    }
    return html;
}

function GetPageBar(currentPage) {
    var html = "";
    html += "<ul class=\"pagenum\">";

    var pageCountHtml = "<li class=\"unavailable\"><p>" + currentPage + "/" + pageCount + "</p></li>";

    if (currentPage == 1) {
        html += "<li class=\"unavailable\"><p>首页</p></li>";
        html += "<li class=\"prev unavailable\"><p>上一页</p></li>";
        html += pageCountHtml;
        if (currentPage == pageCount || pageCount == 1) {
            html += "<li class=\"next unavailable\"><p>下一页</p></li>";
        }
        else {
            html += "<li class=\"next\"><a href='javascript:void(0);' onclick='getServerList(" + (currentPage + 1) + ")'><p>下一页</p></a></li>";
        }
    }
    else {
        html += "<li><a href='javascript:void(0);' onclick='getServerList(1)'><p>首页</p></a></li>";
        html += "<li class=\"prev\"><a href='javascript:void(0);' onclick='getServerList(" + (currentPage - 1) + ")'><p>上一页</p></a></li>";
        html += pageCountHtml;
    }

    if (currentPage == pageCount) {
        html += "<li class=\"unavailable\"><p>末页</p></li>";
    }
    else {
        html += "<li><a href='javascript:void(0);' onclick='getServerList(" + pageCount + ")'><p>末页</p></a></li>";
    }

    html += "</ul>";

    return html;
}

$(document).ready(function () {
    //全选
    $("#checkAll").click(function () {
        var checked = false;
        if ($(this).prop("checked")) {
            $(this).checked = true;
            checked = true;
        }
        else {
            $(this).checked = false;
        }

        $("#modifyList").each(function () {
            $(this).find('li').each(function () {
                $(this).find('input').each(function () {
                    $(this).prop("checked", checked);
                });
            });
        });
    });

    //添加服务器
    $("#Submit011").click(function () {
        var name = $.trim($("#name").val());
        var desc = $.trim($("#desc").val());
        var type = $("#type").val();
        var url = $.trim($("#url").val());
        var paras = $.trim($("#paras").val());
        var mails = $.trim($("#mails").val());
        var interval = $.trim($("#interval").val());
        var isuse = $("#isuse").val();

        if (name === ""){
            showMsg("服务器名称不能为空");
        }
        else if (url === "")
        {
            showMsg("URL不能为空");
        }
        else if (url.substr(0,4) != "http") {
            showMsg("URL格式不正确");
        }
        else if (interval === "") {
            showMsg("轮询时间不能为空");
        }
        else {
            if (mails != "")
            {
                var isVaild = true;
                var ary = mails.split(',');
                for (var i = 0; i < ary.length; i++)
                {
                    isVaild = checkEmail(ary[i]);
                    if (!isVaild)
                    {
                        break;
                    }
                }
                if (!isVaild) {
                    showMsg("邮箱格式不正确，请检查");
                    return;
                }
            }

            $.ajax({
                url: "/Home/Add",
                data: {
                    pId: parentId,
                    name: name,
                    desc: desc,
                    type: type,
                    url: url,
                    paras: encodeURI(paras),
                    mails: mails,
                    interval: interval,
                    isuse: isuse
                },
                type: 'post',
                dataType: 'json',
                async: true,
                timeout: 10000,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layer.msg(textStatus + "：" + errorThrown);
                },
                success: function (res) {
                    if (res.Msg == null)
                    {
                        res.Msg = "新增成功";
                    }
                    layer.msg(res.Msg, { time: 500 }, function () {
                        if (res.Result) {
                            popBox.hide("warningBox01");
                            getServerList(currentPages);
                        }
                    });
                }
            });
        }
    });

    //查询服务器
    $("#modify").click(function () {
        var Id = getTopId();
        console.log(Id);
        if (Id === undefined)
        {
            showMsg("请选择要修改的服务器");
            return;
        }
        GetServerInfo(Id);
    });

    //更新服务器
    $("#Submit033").click(function () {
        var id = $.trim($("#u_id").val());
        var name = $.trim($("#u_name").val());
        var desc = $.trim($("#u_desc").val());
        var type = $("#u_type").val();
        var url = $.trim($("#u_url").val());
        var paras = $.trim($("#u_paras").val());
        var mails = $.trim($("#u_mails").val());
        var interval = $.trim($("#u_interval").val());
        var isuse = $("#u_isuse").val();

        if (name === "") {
            showMsg("服务器名称不能为空");
        }
        else if (url === "") {
            showMsg("URL不能为空");
        }
        else if (url.substr(0, 4) != "http") {
            showMsg("URL格式不正确");
        }
        else if (interval === "") {
            showMsg("轮询时间不能为空");
        }
        else {
            if (mails != "") {
                var isVaild = true;
                var ary = mails.split(',');
                for (var i = 0; i < ary.length; i++) {
                    isVaild = checkEmail(ary[i]);
                    if (!isVaild) {
                        break;
                    }
                }
                if (!isVaild) {
                    showMsg("邮箱格式不正确，请检查");
                    return;
                }
            }

            $.ajax({
                url: "/Home/Update",
                data: {
                    Id: id,
                    pId: parentId,
                    name: name,
                    desc: desc,
                    type: type,
                    url: url,
                    paras: encodeURI(paras),
                    mails: mails,
                    interval: interval,
                    isuse: isuse
                },
                type: 'post',
                dataType: 'json',
                async: true,
                timeout: 10000,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layer.msg(textStatus + "：" + errorThrown);
                },
                success: function (res) {
                    if (res.Msg == null) {
                        res.Msg = "修改成功";
                    }
                    layer.msg(res.Msg, { time: 500 }, function () {
                        if (res.Result) {
                            popBox.hide("warningBox03");
                            getServerList(currentPages);
                        }
                    });
                }
            });
        }
    });

    //关闭服务器
    $("#Submit022").click(function () {
        updateUseFlag(0, '关闭', 'warningBox02');
    });

    //开启服务器
    $("#Submit044").click(function () {
        updateUseFlag(1, '开启', 'warningBox04');
    });
});

function GetServerInfo(Id)
{
    $.ajax({
        url: "/Home/getServerById",
        data: {
            Id: Id
        },
        type: 'get',
        dataType: 'json',
        async: true,
        timeout: 10000,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            layer.msg(textStatus + "：" + errorThrown);
        },
        success: function (res) {
            //console.log(res);
            var data = eval("(" + res.Data + ")");
            $("#u_id").val(Id);
            $("#u_name").val(data.name);
            $("#u_desc").val(data.description);
            $("#u_type").val(data.type);
            $("#u_url").val(data.url);
            $("#u_paras").val(data.paras);
            $("#u_mails").val(data.mailgroup);
            $("#u_interval").val(data.interval);
            $("#u_isuse").val(data.isuse);
            popBox.show("warningBox03");
        }
    });
}

function updateUseFlag(flag, type, boxName)
{
    var idstr = getAllId();
    $.ajax({
        url: "/Home/UpdateUseFlag",
        data: {
            idstr: idstr,
            flag: flag
        },
        type: 'post',
        dataType: 'json',
        async: true,
        timeout: 10000,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            layer.msg(textStatus + "：" + errorThrown);
        },
        success: function (res) {
            if (res.Msg == null) {
                res.Msg = type + "成功";
            }
            layer.msg(res.Msg, { time: 500 }, function () {
                if (res.Result) {
                    popBox.hide(boxName);
                    getServerList(currentPages);
                }
            });
        }
    });
}

function getTopId()
{
    var Id;
    $("#modifyList").each(function () {
        $(this).find('li').each(function () {
            var isget = false;
            $(this).find('input').each(function () {
                if ($(this).prop("checked")) {
                    Id = $(this).next().text();
                    isget = true;
                    return false;
                }
            });
            if (isget) {
                return false;
            }
        });
    });
    return Id;
}

function getAllId()
{
    var idstr = "";
    $("#modifyList").each(function () {
        $(this).find('li').each(function () {
            $(this).find('input').each(function () {
                if ($(this).prop("checked")) {
                    idstr += $(this).next().text() + ",";
                }
            });
        });
    });
    return idstr;
}

function showMsg(info)
{
    layer.open({
        title: '友情提示',
        content: info,
        btn: ['确定']
    });
}

$(document).ready(function () {
    getServerList(1);
});