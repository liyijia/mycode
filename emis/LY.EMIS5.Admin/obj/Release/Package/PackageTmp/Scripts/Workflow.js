var executorLimit = "";
var Grades = new Array();
var GradeNames = new Array();
var dialogParson = new Overlay.Dialog({
    title: '选择人员',
    width: 800,
    height: 400,
    contentId: 'SelectParson',
    success: function () {
        var nones = $.fn.zTree.getZTreeObj("treeDemo").getCheckedNodes(true);
        var Names = new Array();
        var Ids = new Array();
        for (var i in nones) {
            if (!nones[i].isDepartment) {
                Names.push(nones[i].name);
                Ids.push(nones[i].id);
            }
        }
        $('.Parsons').html(Names.join(','));
        $('input[name="SelectParson"]').val(Ids.join(','));
        this.close();
    }
});
var dialogGrade = new Overlay.Dialog({
    title: '选择年级',
    width: 800,
    height: 400,
    contentId: 'SelectGrades',
    success: function () {
        $('.Trial').html($('.WGradeTotal').html());
        $('input[name="Trial_Grades"]').val(Grades.join(','));
        $('input[name="Trial_Names"]').val(GradeNames.join(','));
        this.close();
    }
});

var setting = {
    view: {
        dblClickExpand: false,
        showLine: true,
        selectedMulti: false
    },
    check: {
        enable: true,
        chkStyle: "radio",
        radioType: "all"
    },
    data: {
        simpleData: {
            enable: true
        }
    },
    callback: {
        beforeClick: function (treeId, treeNode) {
            var tt = treeId;
            var tmpId = treeNode.id;
        }
    }
};
var options = {
    beforeSubmit: function () {
    },
    success: function (data) {
        callback(data, null);
    }, dataType: "json"
};
$(function () {
    $('input[name="Next"]').click(function () {
        if ($(this).attr('chooseExecutor') != "" && $(this).attr('chooseExecutor').toLowerCase() == "true") {
            if (executorLimit != "" && executorLimit == $(this).attr('executorLimit')) {
                dialogParson.show();
            } else {
                executorLimit = $(this).attr('executorLimit');
                $.post("/Workflow/GetParents", { executorLimit: executorLimit }, function (data) {
                    if (data.length > 0) {
                        $.fn.zTree.init($("#treeDemo"), setting, data);
                        $.fn.zTree.getZTreeObj("treeDemo").setting.check.chkboxType = { "Y": "s", "N": "s" };
                        dialogParson.show();
                    }
                });
            }
        }
    });



    $('#submit').click(function () {
        if ($('input[name="Trial_Type"]').length > 0 && $('input[name="Trial_Clazzs"]').length == 0) {
            BUI.Message.Alert('请选择试用人或班级！', 'warning');
            return;
        }
        if ($('input[name="Pay_Type"]').length > 0 && $('input[name="Pay_Clazzs"]').length == 0) {
            BUI.Message.Alert('请选择充值人或班级！', 'warning');
            return;
        }
        if ($('#SelectFree').length > 0 && $('#SelectFree').css('display')!='none' && $('input[name="Free_UserIds"]').length == 0) {
            BUI.Message.Alert('请选择免费人！', 'warning');
            return;
        }
        
        if (!$('#workForm').validate().form())
            return;

        if ($('input[name="Charge_Money"]').length > 0) {
            var money = 0;
            $('.money').each(function () {
                money += parseInt($(this).val())
            });
            if (parseInt($('input[name="Charge_Money"]').val()) != money) {
                BUI.Message.Alert('所有学校总额为' + money + '元,填写总额为' + $('input[name="Charge_Money"]').val() + '！', 'warning');
                return;
            }
        }
        if ($('input[name="Next"]:checked').length > 0 && $('input[name="Next"]:checked').attr('chooseExecutor').toLowerCase() == "true") {
            if ($('input[name="SelectParson"]').val() != '') {
                $('#workForm').ajaxSubmit(options);
            } else {
                BUI.Message.Alert('请选择处理人！', 'warning');
                return;
            }
        } else if ($('input[name="Next"]:checked').length > 0) {
            $('#workForm').ajaxSubmit(options);
        } else {
            BUI.Message.Alert('请选择下一步操作！', 'warning');
            return;
        }
    });

    $('#sreach').click(function () {
        if ($('#searchClazz').val() == '' && $('input[name="name"]').val() == '' && $('input[name="Phone"]').val() == '') {
            BUI.Message.Alert('请选择班级或姓名或电话！', 'warning');
            return;
        }
        $.post('/WorkFlow/GetParent', $('#form-Search').serialize(), function (data) {
            $('#table-Parents-list').html('');
            $.each(data, function () {
                var color = '';
                switch (this.FeeType) {
                    case 0:
                        color = 'f_red';
                        break;
                    case 1:
                        color = 'f_blue';
                        break;
                    case 2:
                        color = 'f_green';
                        break;
                    case 3:
                        color = 'f_green';
                        break;
                    default:
                        color = 'f_gray';
                        break;

                }
                if ($('.isRadio').val() == 0) {
                    $('#table-Parents-list').append('<li class="check"><input type="checkbox" name="child" value="' + this.Id + '" /></li><li class="' + this.Id + ' ' + color + '">' + this.StuName + '</li>');
                } else {
                    $('#table-Parents-list').append('<li class="check"><input type="radio" name="child" value="' + this.Id + '" /></li><li class="' + this.Id + '">' + this.StuName + '</li>');
                }

            })
        },"json")
    });

    $('#all').change(function () {
        $('input[name="child"]').prop('checked', $(this).prop('checked'));
    });

    $('#the').change(function () {
        $('input[name="child"]').each(function () {
            $(this).prop('checked', !$(this).prop('checked'))
        });
    });
    ajax("/Student/getSchool", { "schoolId": 0 }, $('#searchSchool'));
    $('#searchSchool').change(function () {
        clear($('#searchGrade'));
        if ($(this).val() == "")
            return;
        ajax("/Student/getGrade", { "schoolId": $(this).val() }, $('#searchGrade'), 0);
    });
    $('#searchGrade').change(function () {
        clear($('#searchClazz'));
        if ($(this).val() == "")
            return;
        ajax("/Student/getClazz", { "gradeId": $(this).val() }, $('#searchClazz'), 0);
    });



    $("#SelectRefund").click(function () {
        $('input[name="isFee"]').val('1');
        dialogFree.show();
    });

    $("#SelectMove").click(function () {
        $('input[name="isFee"]').val('1');
        dialogFree.show();
    });

});


function SetGrades(checkbox, name) {
    if ($(checkbox).prop('checked')) {
        Grades.push($(checkbox).val());
        GradeNames.push($('#WSchool option:checked').text() + name);
        $('.WGradeTotal').append('<li id="' + $(checkbox).val() + '"><a href="javascript:;" onclick="RemoveGrades(' + $(checkbox).val() + ')" >' + $('#WSchool option:checked').text() + name + '</a></li>');
    } else {
        RemoveGrades($(checkbox).val());
    }
}

function RemoveGrades(id) {
    $('input[name="WGrades"][value="' + id + '"]').prop('checked', false);
    $('#' + id).remove();
    for (var i = 0; i < Grades.length; i++) {
        if (Grades[i] == id) {
            Grades.splice(i, 1);
            break;
        }
    }
}

function RemoveFree(id) {
    $('.free_' + id).remove();
}

function RemoveMove(id) {
    $('.move_' + id).remove();
}

function ShowDialog(id) {
    $('.isRadio').val(id);
    dialogFree.show();
}

