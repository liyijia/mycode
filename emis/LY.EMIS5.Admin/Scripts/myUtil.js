jQuery.extend({
    myDialog: function (id, body, type, callback) {
        if ($('#' + id).length == 0) {
            var html = '<div aria-hidden="true" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="' + id + '" class="modal fade">'
                                            + '<div class="modal-dialog">'
                                                + '<div class="modal-content">'
                                                    + '<div class="modal-header">'
                                                        + '<button aria-hidden="true" data-dismiss="modal" class="close" type="button">×</button>'
                                                        + '<h4 class="modal-title">' + (type == 'confirm' ? '确认框' : '提示框') + '</h4>'
                                                    + '</div>'
                                                    + '<div class="modal-body">'
                                                        + body
                                                    + '</div>'
                                                    + '<div class="modal-footer">'
                                                        + (type == 'confirm' ? '<button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>' : '')
                                                        + '<button type="button" class="btn btn-warning">确定</button>'
                                                    + '</div>'
                                                + '</div>'
                                            + '</div>'
                                        + '</div>';
            $('.panel-body').append(html);
           
        } else {
            $('.modal-body').html(body);
        }
        $('#' + id).find('.btn-warning').click(function () {
            callback($('#' + id));
        });
        $('#' + id).modal('show')
    }
});