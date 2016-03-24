$(function () {
    var $table = $('#list');
    $('#buttonSearch').on('click', function () {
        $table.dataTable().fnAdjustColumnSizing();
    });

    LoadTable.init($table);

    $('a[name=\'deleteRows\']').live('click', function () {
        if (!confirm('确定要删除？')) {
            return false;
        }

        var url = '/Gift/Delete';
        var data = {
            id: $(this).attr('id')
        };
        $.getJSON(url, data, function (json) {
            if (json.Id == '1') {
                alert(json.Message);
                window.location = json.Path;
            }
            if (json.Id == "0")
                alert(json.Message);
        });
        return false;
    });
});

var LoadTable = {
    init: function (element) {
        element.dataTable({
            "bServerSide": true,
            "sAjaxSource": "/Gift/Index",
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "content", "value": $("#content").val() });
            },
            "aoColumns": [
                { "mData": "Name", "bSortable": false },
                { "mData": "Number", "bSortable": false },
                { "mData": "TypeName", "bSortable": false },
                { "mData": "Exchange", "bSortable": false },
                { "mData": "ShelvesTime", "bSortable": false },
                { "mData": "StartTime", "bSortable": false },
                { "mData": "EndTime", "bSortable": false },
                { "mData": "Credit", "bSortable": false },
                { "mData": "Id", "bSortable": false }
            ],
            "bFilter": false,
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                $('td:eq(0)', nRow).addClass('shui');
                $('td:eq(1)', nRow).addClass('shuang');
                $('td:eq(2)', nRow).addClass('shui');
                $('td:eq(3)', nRow).addClass('shuang');
                $('td:eq(4)', nRow).addClass('shui');
                $('td:eq(5)', nRow).addClass('shuang');
                $('td:eq(6)', nRow).addClass('shui');
                $('td:eq(7)', nRow).addClass('shuang');
                $('td:eq(8)', nRow).html("")
                    .append("<span class='xhx'><a href='/Gift/Edit/" + aData.Id + "'>编辑</a></span> &nbsp;&nbsp;")
                    .append("<span class='xhx'><a id='" + aData.Id + "' name='deleteRows' href='#'>删除</a></span> &nbsp;&nbsp;")
                    .append("<span class='xhx'><a href='/Gift/ExchangeCount/" + aData.Id + "'>兑换统计</a></span>");
            },
            "aaSorting": [[6, 'desc']],
        });
    }
};