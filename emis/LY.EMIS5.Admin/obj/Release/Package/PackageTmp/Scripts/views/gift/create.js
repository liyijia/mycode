var editor_info, editor_conversionrules, editor_remark;
KindEditor.ready(function (k) {
    editor_info = k.create('#editor_info');
    editor_conversionrules = k.create('#editor_conversionrules');
    editor_remark = k.create('#editor_remark');
});
$(document).ready(function () {
    //Validate form
    $('#form-gift').validate();

    //Upload file
    $('#UploadFile').submit(function () {
        var options = {
            dataType: "json",
            success: function (data) {
                if (data.Id == '0') {
                    alert(data.Message);
                }
                if (data.Id == '1') {
                    alert(data.Message);
                    $('#Image').attr('value', data.Path);
                }
                if (data.Id == '2') {
                    alert(data.Message);
                }
            }
        };
        $(this).ajaxSubmit(options);
        return false;
    });

    //Double click the pop-up dialog
    $('#Image').on('dblclick', function () {
        $("#UploadFile").dialog({
            width: 500,
            height: 340,
            modal: true,
            closeText: '关闭',
            show: {
                effect: "blind",
                duration: 1000
            },
            hide: {
                effect: "explode",
                duration: 1000
            }
        });
    });

    //Time controls
    //Submit form
    $('#form-gift').submit(function () {
        $('#_hiddenInfo').attr('value', editor_info.html());
        $('#_hiddenRemark').attr('value', editor_remark.html());
        $('#_hiddenConversionRules').attr('value', editor_conversionrules.html());
        var giftoptions = {
            dataType: "json",
            success: function (responseText) {
                if (responseText.Id == '0') {
                    alert(responseText.Message);
                }
                if (responseText.Id == '1') {
                    alert(responseText.Message);
                    //表单初始化
                    $('#form-gift').resetForm();
                    editor_info.html('');
                    editor_remark.html('');
                    editor_conversionrules.html('');
                    $('#Image').attr('value', '');
                }
            }
        };
        $(this).ajaxSubmit(giftoptions);
        return false;
    });
});