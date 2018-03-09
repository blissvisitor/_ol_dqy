 $(function(){
    $(window).resize(function(){
        resizeBox();
    })
    resizeBox();
    function resizeBox(){
        var leftH=$('.project-left').height();
        var selectH=$('.table-select').height();
        $('.table-box').height(leftH-selectH);
    }
    console.log(1);
 })