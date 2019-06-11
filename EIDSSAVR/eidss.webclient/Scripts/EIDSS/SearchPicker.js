﻿var searchPicker = (function () {
    var defaultSize = { width: 715, height: 600 };
    var defaultGridId = "Grid";

    return {
        internalWindowId: "SearchPickerWindow",

        show: function (url, title, formNumber) {
            bvPopup.showDefault(url, title, formNumber, defaultSize.width, defaultSize.height);
        },

        showInternal: function (url, propertyName) {
            var pickerId = propertyName + "_InternalPicker";
            bvPopup.showInternal(url, pickerId);
        },
        
        closePicker: function (showInInternalWindow, propertyName) {
            if (showInInternalWindow && showInInternalWindow.toLowerCase() == "true") {
                var pickerId = propertyName + "_InternalPicker";
                bvPopup.closeInternal(pickerId);
            } else {
                bvPopup.closeDefaultPopup();
            }
        },
        
        getSelectedIds: function(gridId) {
            var id = bvGrid.getSelectedItemId((gridId == undefined) ? defaultGridId : gridId);
            return id;
        }
    };
})();

function GetSelectedItemId() {
    var id = bvGrid.getSelectedItemId("Grid");
    return id;
}