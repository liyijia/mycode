UserGroup = {
    u_allgroup: [],
    u_grouped: [],
    u_nogroup: [],
    u_jsonright: [],
    u_tabspan: 1,
    u_treesetting: {
        check: {
            enable: true
        },
        data: {
            key: {
                name: "name",
                checked: "_checked"
            },
            simpleData: {
                enable: true,
                idKey: "id",
                pIdKey: "pId",
                rootPId: "0"
            }
        }
    },
    //初始化
    init: function () {
        if (UserGroup.u_tabspan == 1) {
            UserGroup.autocomplete_allgroup();
            UserGroup.autocomplete_right();
        }
        //树形加载
        InitMenu();
        UserGroup.tabchange();
        UserGroup.binddata();
        UserGroup.arrowright();
        UserGroup.arrowleft();
        UserGroup.movetoright();
        UserGroup.movetoleft();
        UserGroup.addusertoright();
        UserGroup.removeusertoright();
        UserGroup.autocomplete_right();
        UserGroup.bindatuocomplete();
    },
    //绑定JSON数据
    binddata: function () {
        UserGroup.u_allgroup=[];
        UserGroup.u_grouped = [];
        UserGroup.u_nogroup = [];
        UserGroup.u_jsonright = [];
        UserGroup.loaddata($("#allgroup li"), UserGroup.u_allgroup);
        UserGroup.loaddata($("#groupedlist li"), UserGroup.u_grouped);
        UserGroup.loaddata($("#nogroup ul li"), UserGroup.u_nogroup);
        UserGroup.loaddata($("#ModelData-Right li"), UserGroup.u_jsonright);
    },
    //获取html对象绑定JSON
    loaddata: function (obj, json) {
        obj.each(function (Index, domElm) {
            var id = $(domElm).find("input").val();
            var name = $(domElm).find("label").attr("title");
            var firstLetter = $(domElm).find(".FirstLetter").val();
            var spelling = $(domElm).find(".Spelling").val();
            json.push({ Id: id, Name: name, FirstLetter: firstLetter, Spelling: spelling });
        });
    },
    //批量右移
    movetoright: function () {
        $(".gtoup-jt .icon-long-arrow-right").click(function () {
            if (UserGroup.u_tabspan == 1) {
                $("#allgroup input:checked").each(function () {
                    var li = $(this).parent().parent();
                    if (UserGroup.is_rightmove($(this).val())) {
                        $(".group-right ul").append(UserGroup.html($(this).val(), $(this).parent().attr("title"), li.find(".FirstLetter").val(), li.find(".Spelling").val(), true));
                        li.remove();
                    }
                });
            }
            else if (UserGroup.u_tabspan == 2) {
                var treeObj = $.fn.zTree.getZTreeObj("menuTree");
                var nodes = treeObj.getCheckedNodes(true);
                for (var i = 0; i < nodes.length; i++) {
                    if (nodes[i].id.toString().indexOf("s") > -1) {
                        if (UserGroup.is_rightmove(nodes[i].id.replace("s", ""))) {
                            var letter = nodes[i].title.split(",");
                            $("#ModelData-Right").append(UserGroup.html(nodes[i].id.replace("s", ""), nodes[i].name, letter[0], letter[1], true));
                        }
                    }
                }
            }
            else {
                $("#nogroup input:checked").each(function (i) {
                    var li = $(this).parent().parent();
                    if (UserGroup.is_rightmove($(this).val())) {
                        $(".group-right ul").append(UserGroup.html($(this).val(), $(this).parent().attr("title"), li.find(".FirstLetter").val(), li.find(".Spelling").val(), true));
                        li.remove();
                    }
                });
            }
            UserGroup.bindatuocomplete();
        });
       
    },
    //批量左移
    movetoleft: function () {
        $(".gtoup-jt .icon-long-arrow-left").click(function () {
            if (UserGroup.u_tabspan == 1) {
                $("#ModelData-Right input:checked").each(function () {
                    var li = $(this).parent().parent();
                    if (UserGroup.is_allgroupmove($(this).val())) {
                        $("#allgroup ul").append(UserGroup.html($(this).val(), $(this).parent().attr("title"), li.find(".FirstLetter").val(), li.find(".Spelling").val(), false));
                        li.remove();
                    }
                });
            }
            else if (UserGroup.u_tabspan == 2) {
                $("#ModelData-Right input:checked").each(function (i) {
                    $(this).parent().parent().remove();
                });
            }
            else {
                $("#ModelData-Right input:checked").each(function () {
                    var li = $(this).parent().parent();
                    if (UserGroup.is_nogroupmove($(this).val())) {
                        $("#nogroup ul").append(UserGroup.html($(this).val(), $(this).parent().attr("title"), li.find(".FirstLetter").val(), li.find(".Spelling").val(), false));
                        li.remove();
                    }
                });
            }
            UserGroup.bindatuocomplete();
        });
    },
    //所有人感应查询
    autocomplete_allgroup: function () {        
        $('.text-left').autocomplete(UserGroup.u_allgroup, {
            max: 15,
            minChars: 0,
            width: 300,
            scrollHeight: 300,
            matchContains: true,
            autoFill: false,
            formatItem: function (row, i, max) {
                return row.Name;
            },
            formatMatch: function (row, i, max) {
                return row.Name + row.FirstLetter + row.Spelling;
            },
            formatResult: function (row) {
                return row.Name;
            }
        }).result(function (event, row, formatted) {
            if (UserGroup.is_rightmove(row.Id)) {
                //指向左 true
                $("#ModelData-Right").append(UserGroup.html(row.Id, row.Name, row.FirstLetter, row.Spelling, true));
                $("#allgroup ul li input[type='checkbox'][value=" + row.Id + "]").parent().parent().remove();
                UserGroup.bindatuocomplete();
            }
        });
    },
    //已分组感应查询
    autocomplete_grouped: function () {
        $('.text-left').autocomplete(UserGroup.u_grouped, {
            max: 15,
            minChars: 0,
            width: 300,
            scrollHeight: 300,
            matchContains: true,
            autoFill: false,
            formatItem: function (row, i, max) {
                return row.Name;
            },
            formatMatch: function (row, i, max) {
                return row.Name + row.FirstLetter + row.Spelling;
            },
            formatResult: function (row) {
                return row.Name;
            }
        }).result(function (event, row, formatted) {
            if (UserGroup.is_rightmove(row.Id)) {
                $("#ModelData-Right").append(UserGroup.html(row.Id, row.Name, row.FirstLetter, row.Spelling, true));
                $("#ModelData-Left li input[type='checkbox'][value=" + row.Id + "]").parent().parent().remove();
                UserGroup.bindatuocomplete();
            }
        });
    },
    //未分组感应查询
    autocomplete_nogroup: function () {
        $('.text-left').autocomplete(UserGroup.u_nogroup, {
            max: 15,    //列表里的条目数
            minChars: 0,    //自动完成激活之前填入的最小字符
            width: 300,     //提示的宽度，溢出隐藏
            scrollHeight: 300,   //提示的高度，溢出显示滚动条
            matchContains: true,    //包含匹配，就是data参数里的数据，是否只要包含文本框里的数据就显示
            autoFill: false,    //自动填充
            formatItem: function (row, i, max) {//显示
                return row.Name;
            },
            formatMatch: function (row, i, max) {//匹配
                return row.Name + row.FirstLetter + row.Spelling;
            },
            formatResult: function (row) { //返回
                return row.Name;
            }
        }).result(function (event, row, formatted) {
            if (UserGroup.is_rightmove(row.Id)) {
                $("#ModelData-Right").append(UserGroup.html(row.Id, row.Name, row.FirstLetter, row.Spelling, true));
                $("#nogroup ul li input[type='checkbox'][value=" + row.Id + "]").parent().parent().remove();
                UserGroup.bindatuocomplete();
            }
        });
    },
    //右边感应查询
    autocomplete_right: function () {
        $('.text-right').autocomplete(UserGroup.u_jsonright, {
            max: 15,
            minChars: 0,
            width: 300,
            scrollHeight: 300,
            matchContains: true,
            autoFill: false,
            formatItem: function (row, i, max) {
                return row.Name;
            },
            formatMatch: function (row, i, max) {
                return row.Name + row.FirstLetter + row.Spelling;
            },
            formatResult: function (row) {
                return row.Name;
            }
        }).result(function (event, row, formatted) {
            var isMoved = false;
            if (UserGroup.u_tabspan == 1)
            {
                if (UserGroup.is_allgroupmove(row.Id)) {
                    $("#allgroup ul").append(UserGroup.html(row.Id, row.Name, row.FirstLetter, row.Spelling, false));
                    isMoved = true;
                }
            }
            else if (UserGroup.u_tabspan == 2)
            {
                isMoved = true;
            }
            else {
                if (UserGroup.is_nogroupmove(row.Id)) {
                    $("#nogroup ul").append(UserGroup.html(row.Id, row.Name, row.FirstLetter, row.Spelling, false));
                    isMoved = true;
                }
            }
            if (isMoved) {
                $("#ModelData-Right li input[type='checkbox'][value=" + row.Id + "]").parent().parent().remove();
                UserGroup.bindatuocomplete();
            }
        });
    },
    //单个右移
    arrowright: function () {
        $(".group-nav-content .icon-long-arrow-right").unbind("click").bind("click",function () {
            var label = $(this).parent().parent().parent();
            var id = label.find("input[type=checkbox]").val();
            var name = label.attr("title");
            var firstLetter = label.parent().find(".FirstLetter").val()
            var spelling = label.parent().find(".Spelling").val();
            if (UserGroup.u_tabspan == 1) {
                if (UserGroup.is_rightmove(id)) {
                    $(".group-right ul").append(UserGroup.html(id, name, firstLetter, spelling, true));
                    label.parent().remove();
                }
            }
            else if (UserGroup.u_tabspan == 3) {
                if (UserGroup.is_rightmove(id)) {
                    $(".group-right ul").append(UserGroup.html(id, name, firstLetter, spelling, true));
                    label.parent().remove();
                }
            }
            UserGroup.bindatuocomplete();
            UserGroup.arrowleft();
        });
    },
    //单个左移
    arrowleft: function () {
        $(".group-right .icon-long-arrow-left").unbind("click").bind("click",function () {
            var label = $(this).parent().parent().parent();
            var id = label.find("input[type=checkbox]").val();
            var name = label.attr("title");
            var firstLetter = label.parent().find(".FirstLetter").val()
            var spelling = label.parent().find(".Spelling").val();
            if (UserGroup.u_tabspan == 1) {
                if (UserGroup.is_allgroupmove(id)) {
                    $("#allgroup ul").append(UserGroup.html(id, name, firstLetter, spelling, false));
                }
                label.parent().remove();
            }
            else if (UserGroup.u_tabspan == 2) {
                label.parent().remove();
            }
            else {
                if (UserGroup.is_nogroupmove(id)) {
                    $("#nogroup ul").append(UserGroup.html(id, name, firstLetter, spelling, false));
                }
                label.parent().remove();
            }
            UserGroup.bindatuocomplete();
            UserGroup.arrowright();
        });
    },
    //生成html
    html: function (id, name, firstletter, spelling, isright) {
        var html = "";
        html += "<li>";
        html += "<label title='" + name + "'><input type=\"checkbox\" value=\"" + id + "\" />" + name + "";
        if (isright)//指向左
            html += "<span class=\"group-people-jt\"><a><i class=\"icon icon-long-arrow-left\"></i></a></span>";
        else
            html += "<span class=\"group-people-jt\"><a><i class=\"icon icon-long-arrow-right\"></i></a></span>";
        html += "</label>";
        html += " <input type=\"hidden\" class=\"FirstLetter\" value='" + firstletter + "' />";
        html += " <input type=\"hidden\" class=\"Spelling\" value='" + spelling + "' />";
        html += "</li>";
        return html;
    },
    //全部增加
    addusertoright: function () {
        $(".zjleft>a").click(function () {
            if (UserGroup.u_tabspan == 1) {
                $("#allgroup input[type=checkbox]").each(function (i) {
                    if (UserGroup.is_rightmove($(this).val())) {
                        var li = $(this).parent().parent();
                        $(".group-right ul").append(UserGroup.html($(this).val(), $(this).parent().attr("title"), li.find(".FirstLetter").val(), li.find(".Spelling").val(), true));
                        li.remove();
                    }
                });
            }
            else if (UserGroup.u_tabspan == 2) {
                $("#grouped input[type=checkbox]").each(function (i) {
                    if (UserGroup.is_rightmove($(this).val())) {
                        var li = $(this).parent().parent();
                        $(".group-right ul").append(UserGroup.html($(this).val(), $(this).parent().attr("title"), li.find(".FirstLetter").val(), li.find(".Spelling").val(), true));
                        li.remove();
                    }
                });
            }
            else {
                $("#nogroup input[type=checkbox]").each(function (i) {
                    if (UserGroup.is_rightmove($(this).val())) {
                        var li = $(this).parent().parent();
                        $(".group-right ul").append(UserGroup.html($(this).val(), $(this).parent().attr("title"), li.find(".FirstLetter").val(), li.find(".Spelling").val(), true));
                        li.remove();
                    }
                });
            }
            UserGroup.bindatuocomplete();
        });
    },
    //全部移除
    removeusertoright: function () {
        $(".ycright>a").click(function () {
            $(".group-right>ul>li").remove();
            UserGroup.bindatuocomplete();
        });
    },
    //切换选项卡
    tabchange: function () {
        $(".group-nav-ul li").click(function () {
            UserGroup.u_tabspan = $(this).data("tab");
            $(".group-nav-ul li").removeClass("group-nav-on");
            $(this).show().addClass("group-nav-on");
            $(".group-nav-content").hide();
            if (UserGroup.u_tabspan == 1) {
                $("#allgroup").show();
            }
            else if (UserGroup.u_tabspan == 2) {
                $("#grouped").show();
            }
            else {
                $("#nogroup").show();
            }
            UserGroup.bindatuocomplete();
        });
    },
    //绑定所有的感应查询
    bindatuocomplete: function () {
        UserGroup.binddata();
        $('.text-left').unautocomplete();
        $('.text-right').unautocomplete();
        if (UserGroup.u_tabspan == 1)
            UserGroup.autocomplete_allgroup();
        else if (UserGroup.u_tabspan == 2)
            UserGroup.autocomplete_grouped();
        else
            UserGroup.autocomplete_nogroup();
        UserGroup.autocomplete_right();
    },
    is_rightmove: function (id) {
        if (_.where(UserGroup.u_jsonright, { Id: id }).length == 0)
            return true;
        return false;
    },
    is_groupedmove: function (id) {
        if (_.where(UserGroup.u_grouped, { Id: id }).length == 0)
            return true;
        return false;
    },
    is_allgroupmove: function (id) {
        if (_.where(UserGroup.u_allgroup, { Id: id }).length == 0)
            return true;
        return false;
    },
    is_nogroupmove: function (id) {
        if (_.where(UserGroup.u_nogroup, { Id: id }).length == 0)
            return true;
        return false;
    }
}
$(function ($) {
    UserGroup.init();
});
