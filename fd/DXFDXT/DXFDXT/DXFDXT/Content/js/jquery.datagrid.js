$.fn.customDataGrid = function (options) {
    var $table = $(this);
    var obj = new CustomDataGrid($table, options);
    obj.bindData();
    obj.resize();
};

var CustomDataGrid = function (el, options) {
    var defaults = {
        url: '', //请求后台的URL（*）
        method: 'post', //请求方式（*）
        showHeader: true,//是否显示表头
        toolbar: '#toolbar', //工具按钮用哪个容器
        striped: true, //是否显示行间隔色
        cache: false, //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
        pagination: true, //是否显示分页（*）
        sortable: true,                     //是否启用排序
        sortOrder: "asc",                   //排序方式
        sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
        pageNumber: 1,                       //初始化加载第一页，默认第一页
        pageSize: 10,                       //每页的记录行数（*）
        pageList: [10, 20, 30, 50],        //可供选择的每页的行数（*）
        queryParams: function (params) {
            var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
                rows: params.limit,   //页面大小
                page: params.offset / params.limit,  //页码
                sort: params.sort,  //排序列名
                order: params.order//排位命令（desc，asc）
                //keyword: $("#txt_keyword").val()
            };
            if (typeof options.Params == 'object') {
                for (var item in options.Params) {
                    temp[options.Params[item]] = $("#" + options.Params[item]).val();
                }
            }

            return temp;
        }       //传递参数（*）
        //search: false,                       //是否显示表格搜索
        //showColumns: true,                  //是否显示所有的列
        //showRefresh: true,                  //是否显示刷新按钮
        //minimumCountColumns: 2,             //最少允许的列数
        //clickToSelect: true,                //是否启用点击选中行
        //height: $(window).height(),                        //如果没有设置height属性，表格自动根据记录条数觉得表格高度
        //uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
        //showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
        //cardView: top.$(window).width() <= 375,                    //是否显示详细视图
        //detailView: false                //是否显示父子表
    };
    var heightDiff = $(window).height() - options.height;
    this.options = $.extend(defaults, options);
    this.el = $(el);
    this.bindData = function () {
        el.bootstrapTable(this.options);
    }
    this.resize = function () {
        $(window).resize(function (e) {
            el.bootstrapTable("resetView",
            {
                height: $(window).height() - heightDiff
            });
        });
    }
}
