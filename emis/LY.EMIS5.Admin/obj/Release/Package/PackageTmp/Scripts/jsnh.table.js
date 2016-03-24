jsnh = {};
jsnh.table =
{
    sortableTable: function (table) {
        this._rows = null;
        this._table = table;
        this.initialize = function () {
            var input_list = new Array();
            var rows = this._table.children("tbody").children("tr");
            for (var i = 0; i < rows.length; i++) {
                input_list[i] = new Array();
                var columns = rows[i].cells;
                for (var j = 0; j < columns.length; j++) {
                    var children = $(columns[j]).children();
                    if (children.length == 0)
                        input_list[i][j] = columns[j];
                    else {
                        var ipts = children.filter("input[type=text]");
                        if (ipts.length == 0)
                            input_list[i][j] = children[0];
                        else {
                            input_list[i][j] = ipts[0];
                            $(ipts[0]).attr("row", i);
                            $(ipts[0]).attr("col", j);
                            ipts[0].onkeydown = function () {
                                m = $(event.srcElement).parent().parent().index();
                                n = $(event.srcElement).parent().index();

                                switch (event.keyCode) {
                                    case 37: //左
                                        if (n > 1) {
                                            n--;
                                        }
                                        break;
                                    case 38: //上
                                        if (m > 0) {
                                            m--;
                                        }
                                        break;
                                    case 39: //右
                                        if (n < j - 1) {
                                            n++;
                                        }
                                        break;
                                    case 40: //下
                                        if (m < i - 1) {
                                            m++;
                                        }
                                        break;
                                }
                                $(event.srcElement).parents('tbody').find('tr:eq(' + m + ')').find('td:eq(' + n + ')>input[type="text"]').focus();
                            };
                        }
                    }
                }
            }
            this._rows = input_list;
        };

    }
}

$(function () {
    $("table").filter(".sortable").each(function () {
        var sorter = new jsnh.table.sortableTable($(this));
        sorter.initialize();
    });
});