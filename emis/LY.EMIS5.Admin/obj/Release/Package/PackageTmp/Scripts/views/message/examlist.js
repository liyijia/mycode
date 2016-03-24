var LoadTable = {
    init: function (element) {
        element.dataTable({
            "bServerSide": true,
            "sAjaxSource": "/Message/ExamList",
            "fnServerParams": function (aoData) {
                aoData.push(
                    {
                        "name": "gradeId",
                        "value": $("#GradeDropDownList option:selected").attr('value')
                    },
                    {
                        "name": "classId",
                        "value": $("#ClassDropDownList option:selected").attr('value')
                    });
            },
            "aoColumns": [
                { "mData": "Name", "bSortable": false },
                { "mData": "ExamTime", "bSortable": false },
                { "mData": "Subjects", "bSortable": false },
                { "mData": "Status", "bSortable": false },
                { "mData": "Id", "bSortable": false }
            ],
            "bFilter": false,
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                $('td:eq(0)', nRow).addClass('shui');
                $('td:eq(1)', nRow).addClass('shuang');
                $('td:eq(2)', nRow).addClass('shui');
                $('td:eq(3)', nRow).addClass('shuang');
                $('td:eq(4)', nRow).html("")
                    .append("<a href='/Message/SendExam/" + aData.Id + "'>发送信息</a>");
            },
            "aaSorting": [[4, 'desc']],
        });
    }
};

$(function () {
    var page = {
        table: $('#list'),
        dropClass: $('#ClassDropDownList')
    };

    $('#btnSearch').click(function () {
        page.table.dataTable().fnAdjustColumnSizing();
    });

    LoadTable.init(page.table);


    $('#GradeDropDownList').change(function () {
        $.post('/Message/GetClass', { id: this.value }, function (data) {
            $('#ClassDropDownList>option:not(:first)').remove();
            var option = '';
            if (data.Message != 'NULL') {
                $.each(data[0].items, function (index, target) {
                    option += '<option value=\'' + target.Id + '\'>' + target.Name + '</option>';
                });
                page.dropClass.append(option);
            }
        }, 'json');
    });
});