$(document).ready(function () {
    var $table = $('#dt');
    var $li = $("#date li");

    $li.on('click', function () {
        $(this).addClass('selected').siblings().removeClass('selected');
        $table.dataTable().fnAdjustColumnSizing();
    });

    $('#buttonSearch').on('click', function () {
        $table.dataTable().fnAdjustColumnSizing();
    });

    LoadTable.init($table);
});

var LoadTable = {
    init: function (element) {
        element.dataTable({
            "bServerSide": true,
            "sAjaxSource": "/Gift/ExchangeCount",
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "month", "value": $(".selected").text().replace('月', '') },
                            { "name": "content", "value": $("#content").val() },
                            { "name": "intId", "value": $("#_hidenId").attr('value') });
            },
            "aoColumns": [
                { "mData": "Name", "bSortable": false },
                { "mData": "GiftName", "bSortable": false },
                { "mData": "OrderTime", "bSortable": false }
            ], "bFilter": false,
            "fnRowCallback": function (nRow) {
                $('td:eq(0)', nRow).addClass('shui');
                $('td:eq(1)', nRow).addClass('shuang');
                $('td:eq(2)', nRow).addClass('shui');
            }, 'aaSorting': [[3, 'desc']],
        });
    }
};