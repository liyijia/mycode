TeacherGroup = {
    u_allgroup:[],
    u_grouped: [],
    u_visit: [],
    init: function () {
        TeacherGroup.binddata();
        TeacherGroup.movetomembers();
        TeacherGroup.movetovisit();
        TeacherGroup.deletethis();
        TeacherGroup.bindatuocomplete();
        TeacherGroup.selectall();
        TeacherGroup.moveallteacher();
    },
    binddata: function () {
        TeacherGroup.u_allgroup = [];
        TeacherGroup.u_grouped = [];
        TeacherGroup.u_visit = [];

        TeacherGroup.loaddata($(".teacherSource li"), TeacherGroup.u_allgroup);
        TeacherGroup.loaddata($(".groupedSource li"), TeacherGroup.u_grouped);
        TeacherGroup.loaddata($(".visitSource li"), TeacherGroup.u_visit);
    },
    loaddata: function (obj,json) {
        obj.each(function (Index, domElm) {
            var id = $(domElm).find("input").val();
            var name = $(domElm).data("name");
            var firstLetter = $(domElm).data("firstletter");
            var spelling = $(domElm).data("spelling");
            var newjson = { Id: id, Name: name, FirstLetter: firstLetter, Spelling: spelling };
            if (_.where(json, { Id: id }).length == 0)
                json.push(newjson);
        });
    },
    movetomembers: function () {
        $(".move-members").click(function () {
            $(".teacherSource input:checked").each(function () {
                var li = $(this).parent().parent();
                if (TeacherGroup.is_membersmove($(this).val())) {
                    $(".groupedSource").append(TeacherGroup.html($(this).val(), li.data("name"), li.data("firstletter"), li.data("spelling")));
                    TeacherGroup.bindatuocomplete();
                }
            });
        });
    },
    movetovisit: function () {
        $(".move-visit").click(function () {
            $(".teacherSource input:checked").each(function () {
                var li = $(this).parent().parent();
                if (TeacherGroup.is_visitmove($(this).val())) {
                    $(".visitSource").append(TeacherGroup.html($(this).val(), li.data("name"), li.data("firstletter"), li.data("spelling")));
                    TeacherGroup.bindatuocomplete();
                }
            });
        });
    },
    deletethis: function () {
        $(".groupedSource>li>label>span,.visitSource>li>label>span").click(function () {
            $(this).parent().parent().remove();
        });
    },
    html: function (id, name, firstletter, spelling, isright) {
        var html = "";
        html += "<li data-firstletter='" + firstletter + "' data-spelling='" + spelling + "' data-name='" + name + "'>";
        html += "<label><input type=\"checkbox\" value=\"" + id + "\" /> " + name + "";
        html += "<span class=\"group-people-jt\"><a><i class=\"icon icon-mail-forward\"></i></a></span>";
        html += "</label>";
        html += "</li>";
        return html;
    },
    autocomplete_allgroup: function () {
        $('.text-allgroup').autocomplete(TeacherGroup.u_allgroup, {
            max: 15,
            minChars: 0,
            width: 250,
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
        });
    },
    autocomplete_grouped: function () {
        $('.text-grouped').autocomplete(TeacherGroup.u_grouped, {
            max: 15,
            minChars: 0,
            width: 250,
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
        });
    },
    autocomplete_visit: function () {
        $('.text-visit').autocomplete(TeacherGroup.u_visit, {
            max: 15,
            minChars: 0,
            width: 250,
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
        });
    },
    selectall:function(){
        $(".addall").click(function (event) {
            var choose = $(this).data("id");
            var li = $("." + choose + "ul > li > label").find("input").prop("checked", true);
            event.stopPropagation();
        });
    },
    moveallteacher: function () {
        $(".ycgrouped").click(function () {
            $(".groupedSource li").remove();
            TeacherGroup.bindatuocomplete();
        });
        $(".ycvisit").click(function () {
            $(".visitSource li").remove();
            TeacherGroup.bindatuocomplete();
        });
    },
    bindatuocomplete: function () {
        TeacherGroup.binddata();
        $('.text-allgroup').unautocomplete();
        $('.text-grouped').unautocomplete();
        $('.text-visit').unautocomplete();

        TeacherGroup.autocomplete_allgroup();
        TeacherGroup.autocomplete_grouped();
        TeacherGroup.autocomplete_visit();
    },
    is_membersmove: function (id) {
        if (_.where(TeacherGroup.u_grouped, { Id: id }).length == 0)
            return true;
        return false;
    },
    is_visitmove: function (id) {
        if (_.where(TeacherGroup.u_visit, { Id: id }).length == 0)
            return true;
        return false;
    }
}
$(function ($) {
    TeacherGroup.init();
});