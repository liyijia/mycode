$(function () {

    var page = {
        contact: $('#contact'),
        content: $('#content'),
        inputtime: $('#inputtime'),
        chars: $('#number'),
        count: $('#count'),
        maxstrlen: 198
    };

    page.content.val('');//clear textarea
    page.contact.hide();//hide contacts

    //datetime controls
    page.inputtime.datetimebox({
        showSeconds: false,
        editable: false,
        disabled: true
    });

    $('#checkTime').click(function () {
        if ($(this).is(':checked')) {
            page.inputtime.datetimebox({
                disabled: false
            });
        } else {
            page.inputtime.datetimebox({
                disabled: true
            });
        }
    });

    //select contacts
    var flag;
    //var selectUser=
    $('#btnSelect').click(function () {
        //if (page.contact.is(':visible')) {
        //    page.contact.hide('slow');
        //} else {

        //    if (flag == undefined) {

        //        $.ajax({
        //            url: '/Message/GetGade2',
        //            type: 'get',
        //            dataType: 'text',
        //            success: function (data) {
        //                $('#li_grade').append('年级：' + data + '<br />');
        //                $('#li_class').append('班级：<select id="selectClass"><option value="0">----请选择----</option></select>');
        //                flag = true;
        //                page.contact.show('slow');
        //            }
        //        });

        //    } else {
        //        page.contact.show('slow');
        //    }
        //}

    });
    //send Employee
    $('#btnEmployee').click(function () {
        var value = page.content.val().replace(/[ ]/g, '');
        if (value.length < 5) {
            $.messager.alert('提示信息', '信息过短，请重新编辑!', 'warning');
            return;
        }

        //检查定时发送时间
        if ($('#checkTime').is(':checked')) {
            var tt = page.inputtime.datetimebox('getValue');

            if (tt == '') {
                $.messager.alert('提示信息', '请选择定时发送的时间!', 'warning');
                return;
            }
            if (new Date(tt.split(' ')[0]) - new Date() > 3600 * 1000 * 24 * 7) {
                $.messager.alert('提示信息', '只能提前一周（7天）定时发送!', 'warning');
                return;
            }
        }

        //检查是否选择发送对象
        var student = $('#student input[type=\'checkbox\']:checked');

        if (student.length == 0) {
            $.messager.alert('提示信息', '请选择发送对象!', 'warning');
            return;
        }

        //获取选中的学生
        var strStudent = '';
        $.each(student, function (index, target) {
            strStudent += target.value + ',';
        });
        strStudent = strStudent.substring(0, strStudent.length - 1);
        $('#hidenStudent').attr('value', strStudent);
        //发起ajax请求
        $.ajax({
            type: 'post',
            url: '/Message/SendEmployee',
            data: $("form").serialize(),
            datatype: 'json',
            success: function (data) {
                $.messager.alert('提示信息', '信息发送成功!');
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                $.messager.alert('提示信息', '信息发送失败!', 'error');
            }
        });
        return;
    });
    //选择用户
    $("#btnManager").click(function () {
        if (page.contact.is(':visible')) {
            page.contact.hide('slow');
        } else {

            if (flag == undefined) {
                $.ajax({
                    url: '/Message/GetManager',
                    type: 'post',
                    dataType: 'text',
                    success: function (data) {
                        var $student = $('#student');
                        $student.empty();

                        $student.append(data);
                        flag = true;
                        page.contact.show('slow');
                    }
                });
            }
            else {
                page.contact.show('slow');
            }
        }
    });
    $('#li_grade').delegate('select', 'change', function () {
        var id = this.value;

        $.ajax({
            url: '/Message/GetClass2',
            data: { id: id },
            type: 'post',
            dataType: 'text',
            success: function (data) {
                $('#selectClass>option:not(:first)').remove();
                $('#student').empty();
                $('#selectClass').append(data);
            }
        });
    });

    $('#li_class').delegate('select', 'change', function () {
        var id = this.value;

        $.ajax({
            url: '/Message/GetStudent2',
            data: { id: id },
            type: 'post',
            dataType: 'text',
            success: function (data) {
                var $student = $('#student');
                $student.empty();

                $student.append(data);


            }
        });
    });

    //send messages
    $('#btnSend').click(function () {
        var value = page.content.val().replace(/[ ]/g, '');
        if (value.length < 5) {
            $.messager.alert('提示信息', '信息过短，请重新编辑!', 'warning');
            return;
        }

        //检查定时发送时间
        if ($('#checkTime').is(':checked')) {
            var tt = page.inputtime.datetimebox('getValue');

            if (tt == '') {
                $.messager.alert('提示信息', '请选择定时发送的时间!', 'warning');
                return;
            }
            if (new Date(tt.split(' ')[0]) - new Date() > 3600 * 1000 * 24 * 7) {
                $.messager.alert('提示信息', '只能提前一周（7天）定时发送!', 'warning');
                return;
            }
        }

        //检查是否选择发送对象
        var student = $('#student input[type=\'checkbox\']:checked');

        if (student.length == 0) {
            $.messager.alert('提示信息', '请选择发送对象!', 'warning');
            return;
        }

        //获取选中的学生
        var strStudent = '';
        $.each(student, function (index, target) {
            strStudent += target.value + ',';
        });
        strStudent = strStudent.substring(0, strStudent.length - 1);
        $('#hidenStudent').attr('value', strStudent);

        //发起ajax请求
        $.ajax({
            type: 'post',
            url: '/Message/SendMessage',
            data: $("form").serialize(),
            datatype: 'json',
            success: function (data) {
                $.messager.alert('提示信息', '信息发送成功!');
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                $.messager.alert('提示信息', '信息发送失败!', 'error');
            }
        });
        return;
    });

    page.content.bind('keyup focus', function () {
        var str = $(this).val().replace(/[ ]/g, ''); ///[ ]/g匹配字符串中的空格

        var myLen = globalfun.getStrleng(str, page.maxstrlen);

        if (myLen > page.maxstrlen) {
            page.chars.html(page.maxstrlen - myLen);
            $(this).css({ 'border': '2px solid red' });
        }
        else {
            page.chars.html(page.maxstrlen - myLen);
            $(this).css({ 'border': '1px solid #d4e5ea' });
        }
    });

    //textarea
    page.content.focus(function () {
        $(this).css({ 'height': '100px' });
    });

    //loses focus
    page.content.blur(function () {
        if ($(this).val().length == 0) {
            $(this).css({ 'height': '50px' });
        }
    });

    //today date
    $('#today').click(function () {
        page.content.insertContent(globalfun.currentdate());
    });

    //tomorrow date
    $('#tomorrow').click(function () {
        page.content.insertContent(globalfun.tomorrowdate());
    });

    //my signature
    $('#signature').click(function () {
        page.content.insertContent('{' + $(this).html() + '}');
    });

    //student name
    $('#studentName').click(function () {
        page.content.insertContent('{' + $(this).html() + '}');
    });

    //selected all student group
    $("#checkAll").click(function () {
        $('#studentGroup input[type=\'checkbox\']').prop("checked", this.checked);
        $('#student input[type=checkbox]').prop("checked", this.checked);
        var tt = $('#student input[type=checkbox]:checked');
        globalfun.getCount(page.count, tt.length);
    });

    $('#studentGroup').delegate('input[type=checkbox]', 'change', function () {
        $("#checkAll").prop("checked", $('#studentGroup input[type=\'checkbox\']').length == $("#studentGroup input[type='checkbox']:checked").length ? true : false);

        //var check = $('#studentGroup input[type=\'checkbox\']').filter(':checked');

        //获取选中分组的Id
        //var id = '';
        //for (var i = 0; i < check.length; i++) {
        //    id += check[i].value + ',';
        //}
        //id = id == '' ? [] : id.substring(0, id.length - 1).split(',');

        //if (id.length == 0) {
        //    $('#student input[type=checkbox]').prop('checked', false);
        //    globalfun.getCount(page.count, 0);
        //    return;
        //}

        //选中分组后选中组里面的学生
        //var stu = $('#student input[type=checkbox]');
        //var bool;
        //for (var r = 0; r < stu.length; r++) {
        //    var arr = $(stu[r]).attr('itemid').split(',');
        //    bool = false;
        //    for (var j = 0; j < id.length; j++) {
        //        if (bool == true)
        //            break;
        //        for (var t = 0; t < arr.length; t++) {
        //            if (parseInt(id[j]) == parseInt(arr[t])) {
        //                $(stu[r]).prop('checked', true);
        //                stu.splice(r, 1);
        //                r = r - 1;
        //                bool = true;
        //                break;
        //            }
        //            $(stu[r]).prop('checked', false);
        //        }
        //    }
        //}

        var tt = $('#student input[type=checkbox]:checked');
        globalfun.getCount(page.count, tt.length);
    });

    //selected all seudent
    $('#checkAllstu').click(function () {
        $('#student input[type=checkbox]').prop("checked", this.checked);

        var tt = $('#student input[type=checkbox]:checked');
        globalfun.getCount(page.count, tt.length);
    });

    $('#student').delegate('input[type=checkbox]', 'change', function () {
        $("#checkAllstu").prop("checked", $('#student input[type=checkbox]').length == $('#student input[type=checkbox]:checked').length ? true : false);

        var tt = $('#student input[type=checkbox]:checked');
        globalfun.getCount(page.count, tt.length);
    });
});

//Global Function
var globalfun = {
    currentdate: function () {
        ///<summary>获取当前时间(yyyy年mm月dd日)</summary>

        var date = new Date();
        return date.getFullYear() + '年' + (date.getMonth() + 1) + '月' + date.getDate() + '日';
    },
    tomorrowdate: function () {
        ///<summary>获取明天日期(yyyy年mm月dd日)</summary>

        var date = new Date();
        return date.getFullYear() + '年' + (date.getMonth() + 1) + '月' + (date.getDate() + 1) + '日';
    },
    getStrleng: function (str, maxstrlen) {
        ///<summary>获取字符串长度</summary>
        ///<param name="str">字符串</param>
        ///<param name="maxstrlen">最大字符数</param>

        var myLen = 0;
        var i = 0;
        for (; (i < str.length) && (myLen <= maxstrlen * 2) ; i++) {
            if (str.charCodeAt(i) > 0 && str.charCodeAt(i) < 128)
                myLen++;
            else
                myLen += 1;
        }
        return myLen;
    },
    getCount: function (element, num) {
        ///<summary>统计已选择可发短信的用户</summary>
        ///<param name="element">绑定的对象</param>
        ///<param name="num">总数</param>

        element.html(num);
    }
};