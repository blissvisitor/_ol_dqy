$.fn.bindSelect = function (options) {
    var defaults = {
        id: "id",
        text: "text",
        search: false,
        url: "",
        param: [],
        change: null,
        required: false
    };
    options = $.extend(defaults, options);
    var $element = $(this);
    if (options.url != "") {
        $.ajax({
            url: options.url,
            data: options.param,
            type: "post",
            dataType: "json",
            async: false,
            success: function (data) {
                $.each(data, function (i) {
                    $element.append($("<option></option>").val(data[i][options.id]).html(data[i][options.text]));
                });
                $element.select2({
                    minimumResultsForSearch: options.search == true ? 0 : -1
                });
                $element.on("change", function (e) {
                    if (options.change != null) {
                        options.change(data[$(this).find("option:selected").index()]);
                    }
                    $("#select2-" + $element.attr('id') + "-container").html($(this).find("option:selected").text().replace(/　　/g, ''));
                });
            }
        });
    } else {
        $element.select2({
            minimumResultsForSearch: -1
        });
    }

    if (options.required) {
        $element.on("change",
            function () {
                if ($(this).val() != '' && $(this).val() != '0') {
                    $.removeValidate(this);
                }
            });
    }
}
$.loading = function (bool, text) {
    var $loadingpage = parent.$("#loadingPage");
    var $loadingtext = $loadingpage.find('.loading-content');
    if (bool) {
        $loadingpage.show();
    } else {
        if ($loadingtext.attr('istableloading') == undefined) {
            $loadingpage.hide();
        }
    }
    if (!!text) {
        $loadingtext.html(text);
    } else {
        $loadingtext.html("数据加载中，请稍后…");
    }
    $loadingtext.css("left", (parent.$('body').width() - $loadingtext.width()) / 2 - 50);
    $loadingtext.css("top", (parent.$('body').height() - $loadingtext.height()) / 2);
}
$.fn.formSerialize = function (formdate) {
    var element = $(this);
    if (!!formdate) {
        for (var key in formdate) {
            if (typeof (formdate[key]) == "object") {
                for (var item in formdate[key]) {
                    var $id = element.find('input[id*=' + item + '][id*=' + key + ']');
                    var value = $.trim(formdate[key][item]).replace(/&nbsp;/g, '');
                    var type = $id.attr('type');
                    if ($id.hasClass("select2-hidden-accessible")) {
                        type = "select";
                    }
                    switch (type) {
                        case "checkbox":
                            if (value == "true") {
                                $id.attr("checked", 'checked');
                            } else {
                                $id.removeAttr("checked");
                            }
                            break;
                        case "select":
                            $id.val(value).trigger("change");
                            break;
                        default:
                            $id.val(value);
                            break;
                    }
                }
            } else {
                var $id = element.find('#' + key);
                var value = $.trim(formdate[key]).replace(/&nbsp;/g, '');
                var type = $id.attr('type');
                if ($id.hasClass("select2-hidden-accessible")) {
                    type = "select";
                }
                switch (type) {
                    case "checkbox":
                        if (value == "true") {
                            $id.attr("checked", 'checked');
                        } else {
                            $id.removeAttr("checked");
                        }
                        break;
                    case "select":
                        $id.val(value).trigger("change");
                        break;
                    default:
                        $id.val(value);
                        break;
                }
            }
        };
        return false;
    }
    var postdata = {};
    element.find('input,select,textarea').each(function (r) {
        var $this = $(this);
        var id = $this.attr('id');
        var type = $this.attr('type');
        switch (type) {
            case "checkbox":
                postdata[id] = $this.is(":checked");
                break;
            default:
                var value = $this.val();//== "" ? "&nbsp;" : $this.val();
                //if (!$.request("keyValue")) {
                //    value = value.replace(/&nbsp;/g, '');
                //}
                postdata[id] = value;
                break;
        }
    });
    if ($('[name=__RequestVerificationToken]').length > 0) {
        postdata["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    return postdata;
}
$.request = function (name) {
    var search = location.search.slice(1);
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == name) {
            if (decodeURI(ar[1]) == 'undefined') {
                return "";
            } else {
                return decodeURI(ar[1]);
            }
        }
    }
    return "";
}
$.urlRequest = function (url, name) {
    if (url.indexOf("?") != -1) {
        var search = url.substr(url.indexOf("?") + 1);
        var arr = search.split("&");
        for (var i = 0; i < arr.length; i++) {
            var ar = arr[i].split("=");
            if (ar[0] == name) {
                if (decodeURI(ar[1]) == 'undefined') {
                    return "";
                } else {
                    return decodeURI(ar[1]);
                }
            }
        }
        return "";
    }
    return "";
}
$.modalLoad = function (options) {
    var defaults = {
        animation: 1,
        shade: [1, '#000'] //0.1透明度的白色背景
    };
    options = $.extend(defaults, options);
    var index = layer.load(options.animation, {
        shade: options.shade
    });
}
$.loadClose = function () {
    parent.layer.closeAll('loading');
}
$.modalOpen = function (options) {
    var defaults = {
        id: null,
        title: '系统窗口',
        width: "100px",
        height: "100px",
        url: '',
        shade: 0.6,
        btn: ['确认', '关闭'],
        btnclass: ['btn btn-primary', 'btn btn-danger'],
        closeBtn: 2,
        maxmin: false,
        layerfull: false,
        callBack: null,
        end: function () { }
    };
    options = $.extend(defaults, options);
    var _width = top.$(window).width() > parseInt(options.width.replace('px', '')) ? options.width : top.$(window).width() + 'px';
    var _height = top.$(window).height() > parseInt(options.height.replace('px', '')) ? options.height : top.$(window).height() + 'px';
    var index = layer.open({
        id: options.id,
        type: 2,
        shade: options.shade,
        border: [0, 0, false],
        shadeClose: true,
        title: options.title,
        area: [_width, _height],
        content: options.url,
        closeBtn: options.closeBtn,
        maxmin: options.maxmin,
        skin: options.skin,
        end:options.end
    });
    if (options.layerfull) {
        layer.full(index);
    }
}
$.modalClose = function () {
    var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
    parent.layer.close(index);
}
$.modalAlert = function (content, type) {
    layer.alert(content, {
        icon: type,
        closeBtn: 2,
        title: "系统提示",
        btn: ['确认'],
        btnclass: ['btn btn-primary'],
    });
}
$.modalMsg = function (content, type, callBack) {
    if (type != undefined) {
        layer.msg(content, { icon: type, time: 2000 }, function () { if (callBack) callBack(); });
    } else {
        layer.msg(content);
    }
}
$.modalConfirm = function (content, callBack) {
    layer.confirm(content, {
        icon: 3,
        closeBtn: 2,
        title: "",
        btn: ['确认'],
        btnclass: ['btn btn-primary', 'btn btn-danger'],
    }, function (index) {
        callBack(true);
        layer.close(index);
    }, function () {
        callBack(false);
    });
}
$.fn.formValid = function () {
    return $(this).valid({
        errorPlacement: function (error, element) {
            element.parents('.formValue').addClass('has-error');
            element.parents('.has-error').find('i.error').remove();
            element.parents('.has-error').append('<i class="form-control-feedback fa fa-exclamation-circle error" data-placement="left" data-toggle="tooltip" title="' + error + '" ></i>');
            $("[data-toggle='tooltip']").tooltip();
            //if (element.parents('.input-group').hasClass('input-group')) {
            //    element.parents('.has-error').find('i.error').css('right', '33px');
            //}
            if (!$(element).hasClass("required")) {
                $(element).unbind("keyup").bind("keyup", function () {
                    if ($(element).val() === '') {
                        $(element).parents('.has-error').find('i.error').remove();
                        $(element).parents('.has-error').removeClass('has-error');
                    }
                });
            }
        },
        success: function (element) {
            element.parents('.has-error').find('i.error').remove();
            $(element).parents('.has-error').removeClass('has-error');
        }
    });
}
$.submitForm = function (options) {
    var defaults = {
        url: "",
        param: [],
        //loading: "正在提交数据...",
        success: null,
        close: true,
        returned: false,
        isAlter: true
    };
    options = $.extend(defaults, options);
    // $.loading(true, options.loading);
    window.setTimeout(function () {
        if ($('[name=__RequestVerificationToken]').length > 0) {
            options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
        }
        $.ajax({
            url: options.url,
            data: options.param,
            type: "post",
            dataType: "json",
            success: function (data) {
                //$.loading(false);
                if (data.State == "1") {
                    options.success(data);
                    if (options.isAlter) {
                        $.modalMsg(data.Msg, data.State);
                    }
                    window.setTimeout(function () {
                        if (options.close == true) {
                            $.modalClose();
                        }
                    }, 500);
                    if (options.returned == true) {
                        $("#returnedValue").val(data.DataObj);
                    }
                } else {
                    $.modalAlert(data.Msg, data.State);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                //$.loading(false);
                $.modalMsg(errorThrown, "error");
            },
            //beforeSend: function () {
            //    $.loading(true, options.loading);
            //}
        });
    }, 500);
}
$.confirmForm = function (options) {
    var defaults = {
        prompt: "注：您确定要删除该项数据吗？",
        url: "", 
        param: [],
        //loading: "正在删除数据...",
        success: null,
        close: true
    };
    options = $.extend(defaults, options);
    if ($('[name=__RequestVerificationToken]').length > 0) {
        options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    $.modalConfirm(options.prompt, function (r) {
        if (r) {
            //$.loading(true, options.loading);
            window.setTimeout(function () {
                $.ajax({
                    url: options.url,
                    data: options.param,
                    type: "post",
                    dataType: "json",
                    success: function (data) {
                        //$.loading(false);
                        if (data.State == "1") {
                            $.modalMsg(data.Msg, data.State, options.success(data));
                        } else {
                            $.modalAlert(data.Msg, data.State);
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        // $.loading(false);
                        $.modalMsg(errorThrown, "error");
                    }
                    //beforeSend: function () {
                    //    $.loading(true, options.loading);
                    //},
                    //complete: function () {
                    //    $.loading(false);
                    //}
                });
            }, 500);
        }
    });

}
$.attachList = function () {
    $('.list-box ul').hide();
    $('.list-box p.title span').click(function () {
        $(this).parent().next('.child').slideToggle();
        $(this).parent().toggleClass('guan');
    })
    $('.list-box p.title .upload').click(function () {
        $(this).parent().next('.child').slideDown();
        $(this).parent().addClass('guan');
    })
    $('ul.child li').click(function () {
        $(this).addClass('active').siblings().removeClass('active');
        $(this).parent().siblings().children().removeClass('active');
    })
    var ulNum = $('.list-box p.title').size();
    $('.list-box ul.child').css("max-height", 500 - ulNum);
}
var loadIndex;
$.mainNav = function () {
    $('.main-nav li p').click(function () {
        var parent=$(this).text();
       
        $(this).parent().toggleClass('active').siblings().removeClass('active');
        $(this).next().slideToggle();
        $(this).parent().siblings().find('ul').slideUp();
        $(this).parent().siblings().find('li').removeClass('active');
        if ($(this).parent().hasClass('active')) {
            if (parent == '查询统计' || parent == '客流分析' || parent == '站点管理' || parent == '线路管理' || parent == '场站管理') {
                loadIndex = layer.load(1);
                var vl = $(this).find('a').first().attr('name');
                ChangeFrame(vl);
            } else {
               var a= $(this).parent().find('ul li').first()[0];
               a.childNodes[0].click();
            }
        }
    })
    $('.main-nav li ul li a').click(function () {
        $(this).parent().addClass('active').siblings().removeClass('active');
    });
   
}
$.corpMainNav = function () {

    var firstNowNum;
    $("#ul li:not([class='data-work'])").click(function () {
        $(this).addClass('active');
        $(this).addClass('active').siblings().removeClass('active');
        $('.shaw-bg').hide();
        $('.data-nav').hide();
    })
    $('#ul li.data-work').click(function () {
        var _this = $(this);
        firstNowNum = $('ul>li.active').index();
        var childList = $('.nav-child').children();
        $(this).addClass('active').siblings().removeClass();
        $(childList).click(function () {
            $('.nav-child').unbind('mouseleave');
            $('.data-nav,.shaw-bg').hide();
            $(_this).addClass('active').siblings().removeClass('active');
        })
        $('.nav-child').mouseleave(function () {
            $('#ul li').eq(firstNowNum).addClass('active').siblings().removeClass('active');
            $('.data-nav,.shaw-bg').hide();
            $('.nav-child').unbind('mouseleave');
        })
        $('.data-nav,.shaw-bg').slideDown(100);
        $('.data-nav .nav-child').show();
    })
    //$('.shaw-bg,.cor-header').mouseleave(function () {
    //    $('.shaw-bg').hide();
    //    $('.data-nav').hide();
    //})
}
$.timePro = function (jsonDate) {
    try {
        if (!jsonDate) {
            return "";
        }
        var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();
        var milliseconds = date.getMilliseconds();
        return date.getFullYear() + "-" + month + "-" + day; //+ " " + hours + ":" + minutes + ":" + seconds + "." + milliseconds
    } catch (ex) {
        return "";
    }
}
$.DatetimePro = function (jsonDate) {
    try {
        if (!jsonDate) {
            return "";
        }
        var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        var milliseconds = date.getMilliseconds();
        return date.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minutes + ":" + seconds;
    } catch (ex) {
        return "";
    }
}
//获取年月日
$.GetTime = function (time) {
    str = '';
    if (time != "") {
        var d = new Date(time);
        str += d.getFullYear() + '-';
        if (d.getMonth() + 1 < 10) {
            str += "0" + (d.getMonth() + 1) + '-';
        } else {
            str += (d.getMonth() + 1) + '-';
        }
        if (d.getDate() < 10) {
            str += "0" + d.getDate();
        } else {
            str += d.getDate();
        }
    }

    return str;
}
//获取当前时间
$.CurrentTime=function() {
    var mydate = new Date();
    var str = "" + mydate.getFullYear() + "-";
    if (mydate.getMonth() + 1 < 10) {
        str += "0" + (mydate.getMonth() + 1) + "-";
    } else {
        str += (mydate.getMonth() + 1) + "-";
    }
    if (mydate.getDate() < 10) {
        str += "0" + mydate.getDate();
    } else {
        str += mydate.getDate();
    }

    return str;
}
$.setActive = function (id) {
    $("#" + id).siblings('li').removeClass('active');
    $("#" + id).addClass("active");
}
