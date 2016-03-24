$(document).ready(function () {
    var $table = $('#dt');
    $('#buttonSearch').on('click', function () {
        $table.dataTable().fnAdjustColumnSizing();
    });

    LoadTable.init($table);
});

var LoadTable = {
    init: function (element) {
        element.dataTable({
            "bServerSide": true,
            "sAjaxSource": "/Gift/IntegralCount",
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "content", "value": $("#content").val() });
            },
            "aoColumns": [
                { "mData": "UserName", "bSortable": false },
                { "mData": "Integral", "bSortable": false }
            ], "bFilter": false,
            "fnRowCallback": function (nRow) {
                $('td:eq(0)', nRow).addClass('');
                $('td:eq(1)', nRow).addClass('');
            }, 'aaSorting': [[2, 'desc']],
        });
    }
};