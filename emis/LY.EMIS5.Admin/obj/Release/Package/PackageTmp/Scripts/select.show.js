function getSchool(id) {
    ajax("/Student/getSchool", { "schoolId": 0 }, $("#school"), id);
}
function getGrade(data, id) {
    ajax("/Student/getGrade", { "schoolId": data }, $("#grade"), id);
}
function getClazz(data, id) {
    ajax("/Student/getClazz", { "gradeId": data }, $("#clazz"), id);
}
// 1 请求地址 2 请求数据 3 下拉对象 4 默认选中
function ajax(url, data, select, id) {
    $.ajax({
        url: url, data: data, type: "post", success: function (result) {
            select.append($("<option/>").text("请选择").attr("value", ""));
            $.each(result, function (i) {
                select.append($("<option/>").val(result[i].Id).text(result[i].Name));
            });
            if (id != 0)
                select.val(id);
        }, error: function (data) {
            alert("请求数据失败，请重试！");
        }
    });
}
//清空下拉菜单
function clear(obj) {
    obj.empty();
    obj.append($("<option/>").text("请选择").attr("value", ""));
}