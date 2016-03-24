;
(function ($) {
    if ($.fn.dataTable) {
        $.extend($.fn.dataTable.defaults, {
            bAutoWidth: false,
            bProcessing: true,
            sPaginationType: "full_numbers",
            sServerMethod: "POST",
            iDisplayLength: 25,
            sDom: '<"top"lf><"clear">tr<"block-actions"ip>'
        });
        $.extend($.fn.dataTable.defaults.oLanguage, {
            sLengthMenu: "每页显示 _MENU_ 条记录",
            sZeroRecords: "没有检索到数据",
            sInfoFiltered: "(从 _MAX_ 条数据中检索)",
            sProcessing: "加载中...",
            sInfo: "从 _START_ 到 _END_ / 共 _TOTAL_ 条数据",
            sInfoEmpty: "共 0 条 数据",
            sEmptyTable: "没有检索到数据",
            sZeroRecords: "没有检索到数据",
            sSearch: "检索:",
            oPaginate: { "sFirst": "首页", "sPrevious": "上一页", "sNext": "下一页", "sLast": "尾页" }
        });
    }
    if ($.validator) {
        $.validator.messages = {
            required: "必填字段",
            remote: "请修正该字段",
            email: "请输入正确格式的电子邮件",
            url: "请输入合法的网址",
            date: "请输入合法的日期",
            dateISO: "请输入合法的日期 (ISO).",
            number: "请输入合法的数字",
            digits: "只能输入整数",
            creditcard: "请输入合法的信用卡号",
            equalTo: "请再次输入相同的值",
            accept: "请输入拥有合法后缀名的字符串",
            maxlength: jQuery.validator.format("请输入一个 长度最多是 {0} 的字符串"),
            minlength: jQuery.validator.format("请输入一个 长度最少是 {0} 的字符串"),
            rangelength: jQuery.validator.format("请输入 一个长度介于 {0} 和 {1} 之间的字符串"),
            range: jQuery.validator.format("请输入一个介于 {0} 和 {1} 之间的值"),
            max: jQuery.validator.format("请输入一个最大为{0} 的值"),
            min: jQuery.validator.format("请输入一个最小为{0} 的值"),
        };
        $.validator.methods.required = function (value, element, param) {
            if (element.nodeName.toLowerCase() === "select") {
                var val = $(element).val();
                return val && val.length > 0;
            }
            if (this.checkable(element))
                return this.getLength(value, element) > 0;
            return $.trim(value).length > 0 && !($(element).val() === $(element).attr("placeholder"));
        };
        $.validator.addMethod("username", function (value, element) {
            return this.optional(element) || /^[\u0391-\uFFE5\w]+$/.test(value);
        }, "用户名只能包括中文字、英文字母、数字和下划线");
        $.validator.addMethod("password", function (value, element) {
            return this.optional(element) || /^.{6,20}$/.test(value);
        }, "密码只能是6至20位的任意字符");
        $.validator.addMethod("mobile", function (value, element) {
            return this.optional(element) || (/^1[3,5,4,7,8]{1}\d{9}$/.test(value));
        }, "请正确填写您的手机号码");
        $.validator.addMethod("telephone", function (value, element) {
            return this.optional(element) || (/^(\d{3,4}-?)?\d{7,9}$/.test(value));
        }, "请正确填写您的电话号码");
        $.validator.addMethod("zipCode", function (value, element) {
            return this.optional(element) || (/^[0-9]{6}$/.test(value));
        }, "请正确填写您的邮政编码");
        $.validator.addMethod("repassword", function (value, element) {
            return this.optional(element) || ($("#password").val() == value);
        }, "两次密码不一致");
        $.validator.setDefaults({
            errorPlacement: function (error, element) {
                if (element[0].type == "checkbox" || element[0].type == "radio")
                    error.appendTo(element.parents(":not(lable):first"));
                else
                    error.insertAfter(element);
            },
            submitHandler: function (form) {
                if (form.submit())
                    event.srcElement.disabled = true;
            }
        });
    }
})(jQuery);


