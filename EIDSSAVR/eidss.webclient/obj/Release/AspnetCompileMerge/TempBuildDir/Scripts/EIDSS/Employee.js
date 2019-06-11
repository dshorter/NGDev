﻿var employee = (function () {
    var searchPickerFormNumber = "A08";
    var creatorFormNumber = "A09";

    return {
        initInlinePicker: function (btnShowPickerId, btnClianId, btnAddNewId, btnEditId, isSearchButtonReadOnly, isClianButtonReadOnly, isNewButtonReadOnly, pickerWrapId) {
            var btnShowPicker = $("#" + btnShowPickerId);
            var btnClian = $("#" + btnClianId);
            var btnAddNew = $("#" + btnAddNewId);
            var btnEdit = $("#" + btnEditId);
            if (isSearchButtonReadOnly.toString().toLowerCase() == 'true') {
                btnShowPicker.attr('disabled', 'disabled');
            } else {
                btnShowPicker.removeAttr('disabled');
            }
            if (isClianButtonReadOnly.toString().toLowerCase() == 'true') {
                btnClian.data("disabled", "true");
            } else {
                btnClian.data("disabled", "false");
            }
            if (isNewButtonReadOnly.toString().toLowerCase() == 'true') {
                btnAddNew.attr('disabled', 'disabled');
                btnEdit.attr('disabled', 'disabled');
            } else {
                btnAddNew.removeAttr('disabled');
                btnEdit.removeAttr('disabled');
            }
            
            $("#" + pickerWrapId + ":has(.requiredField)").find(".pickerWrap").addClass('requiredField');
        },

        isInlinePicker: function (objectId, idfsEmployeePropertyName) {
            var mainPickerId = "divEmpSearchPicker_" + objectId + "_" + idfsEmployeePropertyName;
            if ($("#" + mainPickerId).length) {
                return true;
            }
            return false;
        },

        enableInlinePicker: function (objectId, idfsEmployeePropertyName, selectedId) {
            if (!employee.isInlinePicker(objectId, idfsEmployeePropertyName)) {
                return;
            }
            var btnSelect = $("#btnEmpSearchPicker_" + objectId + "_" + idfsEmployeePropertyName);
            var btnClian = $("#btnEmpClian_" + objectId + "_" + idfsEmployeePropertyName);
            var btnAddNew = $("#btnEmpAddNew_" + objectId + "_" + idfsEmployeePropertyName);
            var btnEdit = $("#btnEmpEdit_" + objectId + "_" + idfsEmployeePropertyName);
            btnSelect.removeAttr('disabled');
            btnAddNew.removeAttr('disabled');
            btnEdit.removeAttr('disabled');
            if (selectedId > 0) {
                btnClian.data("disabled", "false");
            } else {
                btnClian.data("disabled", "true");
            }
        },

        disableInlinePicker: function (objectId, idfsEmployeePropertyName) {
            if (!employee.isInlinePicker(objectId, idfsEmployeePropertyName)) {
                return;
            }
            var btnSelect = $("#btnEmpSearchPicker_" + objectId + "_" + idfsEmployeePropertyName);
            var btnClian = $("#btnEmpClian_" + objectId + "_" + idfsEmployeePropertyName);
            var btnAddNew = $("#btnEmpAddNew_" + objectId + "_" + idfsEmployeePropertyName);
            var btnEdit = $("#btnEmpEdit_" + objectId + "_" + idfsEmployeePropertyName);
            btnSelect.attr('disabled', 'disabled');
            btnAddNew.attr('disabled', 'disabled');
            btnEdit.attr('disabled', 'disabled');
            btnClian.data("disabled", "true");
        },
        
        showInlinePicker: function (objectId, idfsEmployeePropertyName, controlName) {
            if (!employee.isInlinePicker(objectId, idfsEmployeePropertyName)) {
                return;
            }
            var mainPickerId = "divEmpSearchPicker_" + objectId + "_" + idfsEmployeePropertyName;
            var label = $("label[for=" + controlName + "]");
            $("#" + mainPickerId).removeClass("invisible");
            label.removeClass("invisible");
        },

        hideInlinePicker: function (objectId, idfsEmployeePropertyName, controlName) {
            if (!employee.isInlinePicker(objectId, idfsEmployeePropertyName)) {
                return;
            }
            var mainPickerId = "divEmpSearchPicker_" + objectId + "_" + idfsEmployeePropertyName;
            var label = $("label[for=" + controlName + "]");
            $("#" + mainPickerId).addClass("invisible");
            label.addClass("invisible");
        },

        showSearchPicker: function(objectId, idfsEmployeePropertyName, strEmployeePropertyName, idfsOrganizationPropertyName, strOrganizationPropertyName, showInInternalWindow) {
            var pickerUrl = bvUrls.getEmployeePickerUrl({
                objectId: objectId,
                idfsEmployeePropertyName: idfsEmployeePropertyName,
                strEmployeePropertyName: strEmployeePropertyName,
                idfsOrganizationPropertyName: idfsOrganizationPropertyName,
                strOrganizationPropertyName: strOrganizationPropertyName,
                showInInternalWindow: showInInternalWindow
            });
            var title = EIDSS.BvMessages['titleEmployeeList'];
            if (showInInternalWindow.toLowerCase() != "true") {
                searchPicker.show(pickerUrl, title, searchPickerFormNumber);
            } else {
                searchPicker.showInternal(pickerUrl, idfsEmployeePropertyName);
            }
        },
        
        moveOrganizationAbbreaviation: function () {
            //$("#panelForm").prepend($("#organizationAbbreaviation"));
            $("#EmpGrid").parents(".listForm").find("#panelForm").prepend($("#organizationAbbreaviation"));
        },
        
        onSearchPickerSelect: function (objectId, idfsEmployeePropertyName, strEmployeePropertyName, idfsOrganizationPropertyName, showInInternalWindow, gridId) {
            var selectedItemId = searchPicker.getSelectedIds(gridId);
            if (selectedItemId) {
                employee.onPickerValueChanged(objectId, idfsEmployeePropertyName, strEmployeePropertyName, selectedItemId, showInInternalWindow);
            } else {
                searchPicker.closePicker(showInInternalWindow);
            }
        },

        onPickerValueChanged: function (objectId, idfsEmployeePropertyName, strEmployeePropertyName, selectedItemId, showInInternalWindow) {
            if (!selectedItemId) {
                var btnClian = $("#btnEmpClian_" + objectId + "_" + idfsEmployeePropertyName);
                if (btnClian.data("disabled") === "true") {
                    return;
                }
            }
            var url = bvUrls.getSetEmployeeUrl();
            showLoading();
            $.ajax({
                url: url,
                type: "POST",
                data: {
                    objectId: objectId,
                    idfsEmployeePropertyName: idfsEmployeePropertyName,
                    strEmployeePropertyName: strEmployeePropertyName,
                    selectedItemId: selectedItemId
                },
                datatype: "json",
                success: function(result) {
                    hideLoading();
                    formRefresher.updateControls(result);
                    if (selectedItemId) {
                        searchPicker.closePicker(showInInternalWindow, idfsEmployeePropertyName);
                    }
                },
                error: function () {
                    hideLoading();
                }
            });
        },

        showEditor: function (objectId, idfsEmployeePropertyName, strEmployeePropertyName, idfsOrganizationPropertyName, isNewEmployee, showInInternalWindow) {
            if (!isNewEmployee) {
                var selectedEmployee = $("input[id*='" + strEmployeePropertyName + "']").val();
                if (!selectedEmployee) {
                    bvDialog.showWarning(EIDSS.BvMessages['noEmployeeSelectedErrorMessage']);
                    return;
                }
            }

            var url = bvUrls.getEmployeeEditorUrl({
                objectId: objectId,
                idfsEmployeePropertyName: idfsEmployeePropertyName,
                strEmployeePropertyName: strEmployeePropertyName,
                idfsOrganizationPropertyName: idfsOrganizationPropertyName,
                isNewEmployee: isNewEmployee,
                showInInternalWindow: showInInternalWindow
            });
            var title = EIDSS.BvMessages['titleEmployeeDetails'];
            if (showInInternalWindow.toLowerCase() != "true") {
                bvPopup.showDefault(url, title, creatorFormNumber, 680, 350);
            } else {
                var pickerId = idfsEmployeePropertyName + "_InternalCreator";
                bvPopup.showInternal(url, pickerId);
            }
        },

        closeEditor: function (showInInternalWindow, propertyName) {
            if (showInInternalWindow.toLowerCase() == "true") {
                var pickerId = propertyName + "_InternalCreator";
                bvPopup.closeInternal(pickerId);
            } else {
                bvPopup.closeDefaultPopup();
            }
        },

        onEditorOkClick: function (newEmployeeId, objectId, idfsEmployeePropertyName, strEmployeePropertyName, idfsOrganizationPropertyName, showInInternalWindow) {
            var contentData;
            if (showInInternalWindow.toLowerCase() == "true") {
                var pickerId = idfsEmployeePropertyName + "_InternalCreator";
                contentData = bvPopup.getInternalData(pickerId);
            } else {
                contentData = bvPopup.getWindowDataByName(bvPopup.defaultName);
            }
            var postUrl = bvUrls.getSaveEmployeeUrl();
            showLoading();
            $.ajax({
                url: postUrl,
                dataType: "json",
                type: "POST",
                data: contentData,
                success: function (data) {
                    hideLoading();
                    var hasError = formRefresher.hasError(data);
                    if (hasError) {
                        formRefresher.updateControls(data);
                    } else {
                        employee.onEmployeeSaved(objectId, idfsEmployeePropertyName, strEmployeePropertyName, newEmployeeId, showInInternalWindow);
                    }
                },
                error: function() {
                    hideLoading();
                    //bvPopup.closeById(windowName);
                }
            });
        },

        onEmployeeSaved: function (objectId, idfsEmployeePropertyName, strEmployeePropertyName, employeeId, showInInternalWindow) {
            var url = bvUrls.getSetEmployeeUrl();
            showLoading();
            $.ajax({
                url: url,
                cache: false,
                type: "POST",
                data: {
                    objectId: objectId,
                    idfsEmployeePropertyName: idfsEmployeePropertyName,
                    strEmployeePropertyName: strEmployeePropertyName,
                    selectedItemId: employeeId
                },
                datatype: "json",
                success: function(result) {
                    hideLoading();
                    formRefresher.updateControls(result);
                    employee.closeEditor(showInInternalWindow, idfsEmployeePropertyName);
                },
                error: function() {
                    hideLoading();
                    //bvPopup.closeById(windowName);
                }
            });
        }
    };
})();