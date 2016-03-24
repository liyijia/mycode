var _Schools;
//返回
$(function () {
    //$.post('/Student/getSchool', { "schoolId": 0 }, function (data) {
    //    if (data.length > 0) {
    //        _Schools = data;
    //    }
    //});
    $('.GoBack').click(function () {
        location.href = $(this).attr('ReturnUrl');
    });
    //
    //使用方法
    //$(文本域选择器).insertContent("插入的内容");
    //$(文本域选择器).insertContent("插入的内容"，数值); //根据数值选中插入文本内容两边的边界, 数值: 0是表示插入文字全部选择，-1表示插入文字两边各少选中一个字符。
    //
    //在光标位置插入内容, 并选中
    $.fn.extend({
        insertContent: function (myValue, t) {
            var $t = $(this)[0];
            if (document.selection) { //ie
                this.focus();
                var sel = document.selection.createRange();
                sel.text = myValue;
                this.focus();
                sel.moveStart('character', -l);
                var wee = sel.text.length;
                if (arguments.length == 2) {
                    var l = $t.value.length;
                    sel.moveEnd("character", wee + t);
                    t <= 0 ? sel.moveStart("character", wee - 2 * t - myValue.length) : sel.moveStart("character", wee - t - myValue.length);
                    sel.select();
                }
            } else if ($t.selectionStart || $t.selectionStart == '0') {
                var startPos = $t.selectionStart;
                var endPos = $t.selectionEnd;
                var scrollTop = $t.scrollTop;
                $t.value = $t.value.substring(0, startPos) + myValue + $t.value.substring(endPos, $t.value.length);
                this.focus();
                $t.selectionStart = startPos + myValue.length;
                $t.selectionEnd = startPos + myValue.length;
                $t.scrollTop = scrollTop;
                if (arguments.length == 2) {
                    $t.setSelectionRange(startPos - t, $t.selectionEnd + t);
                    this.focus();
                }
            }
            else {
                this.value += myValue;
                this.focus();
            }
        }
    });
});
//AJAX 提交
function ajaxSubmit(form, action, callback) {
    form.ajaxSubmit({
        type: 'POST',
        url: action == null ? form.attr("action") : action,
        dataType: "json",
        beforeSubmit: function () {
        },
        success: callback == null ? function () { } : callback
    });
}
//列表页面可能用到 回调函数
function callback(result, table) {
    result =eval(result);
    if (result.icon == 'success') {
        BUI.Message.Alert(result.message, function () {
            if (result.location)
                location.href = result.location;
            else if (table != null) {
                table.dataTable().fnAdjustColumnSizing();
            }
        }, result.icon);
    }
    else {
        var message = result.message.split("|");
        var resultMessage = new Overlay.Dialog({
            title: result.title,
            width: 500,
            height: 166,
            bodyContent: result.message == "" ? "服务器内部发生错误" : message[0],
            success: function () {
                if (table)
                    table.dataTable().fnAdjustColumnSizing();
                else if (result.location)
                {
                    if (message.length<=1) {
                        location.href = result.location;
                    }
                }
                this.close();
            }
        });
        resultMessage.show();
        if (message.length > 1) {
            location.href = result.location;
        }
    }
}

//列表页批量删除公用方法
function deletes(table, action, message) {
    var child = $(".child:checked");
    if (child.length == 0) {
        BUI.Message.Alert('请选择操作对象！', 'warning');
        return false;
    }
    BUI.Message.Confirm(message == undefined ? '确认要禁用选中对象吗？' : message, function () {
        for (var i = 0; i < child.length; i++) {
            $(child[i]).attr("name", "obj[" + i + "].Id");
        }

        ajaxSubmit($("#form-datalist"), action, function (result) {
            callback(result, table);
        });
    }, 'question');
}
function enabled(table, action, message) {
    BUI.Message.Confirm(message == undefined ? '确认要操作该对象吗？' : message, function () {
        ajaxSubmit($("#form-datalist"), action, function (result) {
            callback(result, table);
        });
    }, 'question');
}
var Old = '';
(function ($) {
    $.fn.GetValue = function () {
        if ($(this).val() == Old) {
            return '';
        }
        return $(this).val();
    };
    $.fn.SetValue = function (value) {
        Old = value;
        $(this).val(Old).css('color', '#999').focus(function () {
            if ($(this).val() == Old) {
                $(this).val('').css('color', '#000')
            }
        }).blur(function () {
            if ($(this).val() == '' || $(this).val() == Old) {
                $(this).val(Old).css('color', '#999')
            }
        });
    };
    $.fn.BindSchool = function (isEdit) {
        $(this).empty();
        $(this).append($("<option/>").text("请选择").attr("value", ""));
        var school = $(this);
        var different = school.attr("different");
        var Grade = $("#select_grade_" + different);
        var Clazz = $("#select_clazz_" + different);
        var schoolId = 0; var gradeId = 0; var clazzId = 0;
        if (isEdit) {
            schoolId = $(".hide_school_" + different).val();
            gradeId = $(".hide_grade_" + different).val();
            clazzId = $(".hide_clazz_" + different).val();
            if (gradeId > 0)
                Grade.BindGrade(schoolId, Clazz, gradeId);
            if (clazzId > 0)
                Clazz.BindClazz(gradeId, clazzId);
        }
        //学校
        if (_Schools != undefined && _Schools.length > 0) {
            $.each(_Schools, function () {
                school.append($("<option/>").val(this.Id).text(this.Name));
            });
            if (schoolId > 0)
                school.val(schoolId);
        } else {
            $.post('/Student/getSchool', { "schoolId": 0 }, function (data) {
                if (data.length > 0) {
                    _Schools = data;
                    $.each(_Schools, function () {
                        school.append($("<option/>").val(this.Id).text(this.Name));
                    });
                    if (schoolId > 0)
                        school.val(schoolId);
                }
            });
        }
        $(this).select2({ 'dropdownAutoWidth': true });
        //年级
        if (Grade.attr('different') != undefined) {
            if (gradeId == 0) {
                Grade.empty();
                Grade.append($("<option/>").text("请选择").attr("value", ""));
            }
            $(this).change(function () {
                Grade.BindGrade($(this).val(), Clazz, 0);
            });
            Grade.select2({ 'dropdownAutoWidth': true });
        }
        //班级
        if (Clazz.attr('different') != undefined) {
            if (clazzId == 0) {
                Clazz.empty();
                Clazz.append($("<option/>").text("请选择").attr("value", ""));
            }
            $(Grade).change(function () {
                Clazz.BindClazz(Grade.val(), 0);
            });
            Clazz.select2({ 'dropdownAutoWidth': true });
        }
    };
    $.fn.BindGrade = function (SchoolId, Clazz, GradeId) {
        $(this).prev().find('.select2-chosen').text('请选择');
        $(this).empty();
        $(this).append($("<option/>").text("请选择").attr("value", ""));
        if (Clazz) {
            Clazz.prev().find('.select2-chosen').text('请选择');
            Clazz.empty();
            Clazz.append($("<option/>").text("请选择").attr("value", ""));
        }
        var grade = $(this);
        if (SchoolId != '') {
            if ($('#_Grades').data(SchoolId) == null) {
                $.post('/Student/getGrade', { "schoolId": SchoolId }, function (data) {
                    if (data.length > 0) {
                        $('#_Grades').data(SchoolId, data);
                        $.each(data, function () {
                            grade.append($("<option/>").val(this.Id).text(this.Name));
                        });
                        if (GradeId > 0)
                            grade.val(GradeId);
                    }
                });
            } else {
                $.each($('#_Grades').data(SchoolId), function () {
                    grade.append($("<option/>").val(this.Id).text(this.Name));
                });
                if (GradeId > 0)
                    grade.val(GradeId);
            }
        }
        $(this).select2({ 'dropdownAutoWidth': true });
    };
    $.fn.BindClazz = function (GradeId, ClazzId) {
        $(this).prev().find('.select2-chosen').text('请选择');
        $(this).empty();
        $(this).append($("<option/>").text("请选择").attr("value", ""));
        if (GradeId != '') {
            var clazz = $(this);
            if ($('#_Clazzs').data(GradeId) == null) {
                $.post('/Student/getClazz', { "gradeId": GradeId }, function (data) {
                    if (data.length > 0) {
                        $('#_Clazzs').data(GradeId, data);
                        $.each(data, function () {
                            clazz.append($("<option/>").val(this.Id).text(this.Name));
                        });
                        if (ClazzId > 0)
                            clazz.val(ClazzId);
                    }
                });
            } else {
                $.each($('#_Clazzs').data(GradeId), function () {
                    clazz.append($("<option/>").val(this.Id).text(this.Name));
                });
                if (ClazzId > 0)
                    clazz.val(ClazzId);
            }
        }
        $(this).select2({ 'dropdownAutoWidth': true });
    };
})($);
function getSchool(id) {
    ajax("/Student/getSchool", { "schoolId": 0 }, $(".selectSchool"), id);
}
function getGrade(data, id) {
    ajax("/Student/getGrade", { "schoolId": data.val() }, $("#grade" + data.attr("sNum")), id);
}
function getClazz(data, id) {
    ajax("/Student/getClazz", { "gradeId": data.val() }, $("#clazz" + data.attr("sNum")), id);
}
function getGrade_Main(data, gradeid, id) {
    ajax("/Student/getGrade", { "schoolId": data }, gradeid, id);
}
function getClazz_Main(data, clazzid, id) {
    ajax("/Student/getClazz", { "gradeId": data }, clazzid, id);
}
// 1 请求地址 2 请求数据 3 下拉对象 4 默认选中
function ajax(url, data, select, id) {
    $.ajax({
        url: url, data: data, type: "post", dataType: "json", success: function (result) {
            $.each(result, function (i) {
                select.append($("<option/>").val(result[i].Id).text(result[i].Name));
            });
            if (id != 0)
                select.val(id);
        }, error: function (data) {
            Manager("请求数据失败，请重试！");
        }
    });
}
function clear(obj) {
    obj.empty();
    obj.append($("<option/>").text("请选择").attr("value", ""));
}

//增加，释放弹出层
var overlist = new Array();
function pushDialog(dialog) {
    var isPush = true;
    for (var i = 0; i < overlist.length; i++) {
        if (overlist[i] == dialog) {
            isPush == false;
            break;
        }
    }
    if (isPush)
        overlist.push(dialog);
}

function SchoolDetail(id) {
    new Overlay.Dialog({
        title: '学校详情',
        width: 700,
        closeAction: 'destroy',
        buttons: [],
        loader: {
            url: "/School/Edit/" + id
        }
    }).show();
}

function GradeDetail(id) {
    new Overlay.Dialog({
        title: '年级详情',
        width: 500,
        closeAction: 'destroy',
        buttons: [],
        loader: {
            url: "/Grade/Edit/" + id
        }
    }).show();
}

function ClazzDetail(id) {
    new Overlay.Dialog({
        title: '班级详情',
        width: 500,
        closeAction: 'destroy',
        buttons: [],
        loader: {
            url: "/Clazz/Edit/" + id
        }
    }).show();
}

function StudentDetail(id) {
    new Overlay.Dialog({
        title: '学生详情',
        width: 700,
        closeAction: 'destroy',
        buttons: [],
        loader: {
            url: "/Student/Edit/" + id
        }
    }).show();
}

function IncomingList(CustomerId, SchoolId) {
    new Overlay.Dialog({
        title: '收件箱',
        width: 800,
        height: 500,
        closeAction: 'destroy',
        buttons: [],
        loader: {
            url: "/NameCard/IncomingList?CustomerId=" + CustomerId + "&SchoolId=" + SchoolId
        }
    }).show();
}

function OutgoingList(CustomerId, SchoolId) {
    new Overlay.Dialog({
        title: '发件箱',
        width: 800, height: 500,
        closeAction: 'destroy',
        buttons: [],
        loader: {
            url: "/NameCard/OutgoingList?CustomerId=" + CustomerId + "&SchoolId=" + SchoolId
        }
    }).show();
}

function OutgoingDetail(OutgoingId, SchoolId) {
    new Overlay.Dialog({
        title: '消息详情',
        width: 800, height: 500,
        closeAction: 'destroy',
        buttons: [],
        loader: {
            url: "/NameCard/OutgoingDetail?OutgoingId=" + OutgoingId + "&SchoolId=" + SchoolId
        }
    }).show();
}
function setCookie(name, value) {

}
function getCookie(name) {
    //取出cookie   
    var strCookie = document.cookie;
    //cookie的保存格式是 分号加空格 "; "  
    var arrCookie = strCookie.split("; ");
    for (var i = 0; i < arrCookie.length; i++) {
        var arr = arrCookie[i].split("=");
        if (arr[0] == name) {
            if (arr[1] == undefined)
                return "";
            return decodeURIComponent(arr[1]);
        }
    }
    return "";
}
function delCookie(name) {
    var exp = new Date(); //当前时间   
    exp.setTime(exp.getTime() - 10); //删除cookie 只需将cookie设置为过去的时间    
    var cval = getCookie(name);
    if (cval != null)
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}
function InitSchoolMenu(choose) {
    $.ajax({
        url: "/Message/GetSchoolDatatree?choose=" + choose,
        type: "get",
        error: function () {
            BUI.Message.Alert('加载数据失败，', 'error');
        },
        success: function (data) {
            var roleTree = $("#SchoolDatatree");
            if (data != null) {
                $.fn.zTree.init(roleTree, SchoolSetting, data);
            }
        }
    });
}

function RemoveParent(obj) {
    $(obj).parent().remove();
}