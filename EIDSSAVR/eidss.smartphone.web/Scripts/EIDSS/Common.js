﻿(function () {
    $.ajaxSetup({
        cache: false,
        error: function (jqXhr, textStatus, errorThrown) {
            alert(jqXhr.status);
        }
    });

})();


(function ($) {
    $.extend({
        bvAjax: function(url, data, type, async, onSuccess, onError) {
            $.ajax({
                url: url,
                dataType: "json",
                type: type,
                async: async,
                data: data,
                success: onSuccess,
                error: onError
            });
        },

        bvGet: function(url, data, async, onSuccess, onError) {
            $.bvAjax(url, data, "GET", async, onSuccess, onError);
        },
        
        bvPost: function(url, data, async, onSuccess, onError) {
            $.bvAjax(url, data, "POST", async, onSuccess, onError);
        }
    });
})(jQuery);


function GetSiteLanguage() {
    return bvUrls.getLanguage();
}

function GetUrlPrefixLanguage() {
    return '/' + GetSiteLanguage() + '/';
}

function GlobalAjaxCursorChange() {
    $("html").bind("ajaxStart", function() {
        $(this).addClass('busy');
    }).bind("ajaxStop", function() {
        $(this).removeClass('busy');
    });
}

function UnbindEnterClick() {
    document.onkeypress = function (e) {
        e = window.event || e;
        if (e.keyCode == 13) {
            return false;
        }
    };
}

function ChangeMessageWindowHeigth() {
    var window = $("#Message.t-widget.t-window");
    var content = $('#Message' + ' .t-window-content.t-content');
    var contentHeight = content.outerHeight();
    var popupContentHeight = $('#Message .t-window-content.t-content .popupContent').outerHeight();
    if (popupContentHeight > contentHeight) {
        var oldHeight = window.height();
        var newWindowHeight = popupContentHeight + 50;
        var top = window.position().top - (newWindowHeight - oldHeight) / 2;
        window.css({ height: newWindowHeight, top: (top > 0 ? top : 0) });
        content.css({ height: newWindowHeight - 40 });
    }
}


function dump(obj) {
    obj = obj || {};
    var result = [];
    $.each(obj, function (key, value) {
        result.push('"' + key + '":"' + value + '"');
    });
    return '{' + result.join(',') + '}';
}

(function ($) {
    $.fn.extend({
        center: function (options) {
            var options = $.extend({ // Default values
                inside: window, // element, center into window
                transition: 0, // millisecond, transition time
                minX: 0, // pixel, minimum left element value
                minY: 0, // pixel, minimum top element value
                vertical: true, // booleen, center vertical
                withScrolling: true, // booleen, take care of element inside scrollTop when minX < 0 and window is small or when window is big 
                horizontal: true // booleen, center horizontal
            }, options);
            return this.each(function () {
                var props = { position: 'absolute' };
                if (options.vertical) {
                    var top = ($(options.inside).height() - $(this).outerHeight()) / 2;
                    if (options.withScrolling) top += $(options.inside).scrollTop() || 0;
                    top = (top > options.minY ? top : options.minY);
                    $.extend(props, { top: top + 'px' });
                }
                if (options.horizontal) {
                    var left = ($(options.inside).width() - $(this).outerWidth()) / 2;
                    if (options.withScrolling) left += $(options.inside).scrollLeft() || 0;
                    left = (left > options.minX ? left : options.minX);
                    $.extend(props, { left: left + 'px' });
                }
                if (options.transition > 0) $(this).animate(props, options.transition);
                else $(this).css(props);
                return $(this);
            });
        }
    });
})(jQuery);

function showLoading() {
    if ($("#loading").length == 1) {
        var imgSrc = $("#loadingImg")[0].src;
        $("#loading")[0].innerHTML = '<img id="loadingImg" src="' + imgSrc + '" />';
        $("#loadingImg").center();
        $("#loading").show();
    }
}

function hideLoading() {
    if($("#loading").length == 1){
        $("#loading").hide();
    }
}

function LimitTextArea(textAreaId) {
    var textArea = $("#" + textAreaId);
    var maxlength = textArea.attr('maxlength');
    var val = textArea.val();
    if (val.length > maxlength) {
        textArea.val(val.slice(0, maxlength));
    }
}


