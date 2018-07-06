function falseInfo(a, b) {
    this.show = function () {
        document.getElementById(a).innerText = b;
        document.getElementById(a).style.display = "";
    };
    this.hide = function () {
        document.getElementById(a).style.display = "none";
    }
}

$(document).ready(function () {
    var _falseInfo = new falseInfo("false", '');
    _falseInfo.hide();
});

function login() {
    var username = $.trim($("#userName").val());
    var password = $("#Password").val();

    if (username == "") {
        var _falseInfo = new falseInfo("false", '用户名不能为空');
        _falseInfo.show();
    }
    else if (password == "") {
        var _falseInfo = new falseInfo("false", '密码不能为空');
        _falseInfo.show();
    }
    else {
        $('#login').attr("disabled", "true");
        var Id = layer.msg("正在登录，请稍候...", {
            icon: 16,
            shade: 0.7,
            time: false //取消自动关闭
        });
        $.ajax({
            url: '/User/Login',
            type: 'get',
            data: { userName: username, Password: password },
            dataType: 'json',
            async: true,
            timeout: 10000,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus + "：" + errorThrown);
            },
            success: function (data) {
                var result = data.Result;
                if (result == false) {
                    var _falseInfo = new falseInfo("false", data.Msg);
                    _falseInfo.show();
                    $('#login').removeAttr("disabled");
                }
                else {
                    var _falseInfo = new falseInfo("false", data.Msg);
                    _falseInfo.hide();
                    window.location.href = "/Home/Index";
                }
                layer.close(Id);
            }
        });
    }
}