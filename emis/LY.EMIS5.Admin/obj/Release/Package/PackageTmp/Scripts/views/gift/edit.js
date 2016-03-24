var editor_info, editor_conversionrules, editor_remark;
KindEditor.ready(function (k) {
    editor_info = k.create('#editor_info');
    editor_conversionrules = k.create('#editor_conversionrules');
    editor_remark = k.create('#editor_remark');
});

$(document).ready(function () {
    //Validate form
    $('#form-gift').validate();

    var options = {
        dataType: "json",
        beforeSubmit: function (formData, jqForm) {
            return true;
        },
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

    $('#UploadFile').submit(function () {
        $(this).ajaxSubmit(options);
        return false;
    });

    $('#Image').on('dblclick', function () {
        $("#UploadFile").dialog({
            width: 500,
            height: 340,
            modal: true,
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
    $('#StartTime,#EndTime').on('click', function () {
        WdatePicker();
    });

});