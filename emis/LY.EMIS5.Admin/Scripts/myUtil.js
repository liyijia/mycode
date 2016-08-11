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

// 对Date的扩展，将 Date 转化为指定格式的String   
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
// 例子：   
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423   
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18   
Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}