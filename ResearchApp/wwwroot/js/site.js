// Write your Javascript code.
var isGridEditable = false, isWordWrap = false, isFormWrap = false, editfield = "", displayFieldSelectedDD = "", columnNameAddItemDD = "",
    tableNameAddItemDD = "", addedItemDD = {}, selectedAddItemDD = null, isAddNewItemDD = false, isAddNewItemRequestCompletedDD = false,
    formViewSelectedItem = null, currentFilterColumn, grid, currentFontSize, selectedItemIndex, gridFooterEle, selectedDropdownEle, filterEnabled = false,
    gridSelectionIndex, gridColumns, enableMultiRowEditMode = false, updatedRowsList = [], userObj;
var browserWindow = $(window);
//register custom validation rules
(function ($, kendo) {
    $.extend(true, kendo.ui.validator, {
        rules: { // custom rules
            requiredvalidation: function (input, params) {
                if (input.is("[datavalidation='required']") && input.val() === "") {
                    input.attr("data-requiredvalidation-msg", `${input.attr('id')} field is required`);
                    return false;
                }
                return true;
            }
        },
        messages: { //custom rules messages
            requiredvalidation: function (input) {
                // return the message text
                return input.attr("data-val-requiredvalidation");
            },
            //dropdownlistValidation: function (input) {
            //    // return the message text
            //    return input.attr("data-dropdownlistValidation-msg");
            //}
        }
    });
})(jQuery, kendo);

function getSelectTable() {
    var treeView = $('#treeview').data('kendoTreeView');
    if (treeView && treeView.select().length)
        return treeView.dataItem(treeView.select()).tableName;
    else
        return "Work";
}

function showLoading() {
    document.getElementById("loader").style.display = "block";
    document.getElementById("body-container").style.display = "none";
}

function hideLoading() {
    document.getElementById("loader").style.display = "none";
    document.getElementById("body-container").style.display = "block";
}

function resizeSplitter() {
    var outerSplitter = $("#vertical").data("kendoSplitter");
    if (outerSplitter) {
        outerSplitter.wrapper.height($('.grid-container').height());
        outerSplitter.resize();
    }
}

function resizeGrid() {
    if (grid) {
        grid.resize();
        var height = $(window)[0].innerHeight, gridHeaderFooterHeight;
        gridHeaderFooterHeight = $('.k-grid-header').height() + $('.k-grid-pager').height();
        $('.grid-container').height(height - 50);
        $('.k-grid-content').height(height - (65 + gridHeaderFooterHeight));
        if (isFormWrap) {
            gridHeaderFooterHeight = $('#edit-form .k-grid-pager').height();
            $('#edit-form .k-grid-content').height(height - (60 + gridHeaderFooterHeight));
        }
    }
}

function resizeSearchGrid() {
    if ($('.search-results').length) {
        var height = $(window)[0].innerHeight;
        $('.k-grid-content').height(height - 195);
    }
}

$(window).resize(function () {
    setTimeout(function () {
        resizeGrid();
        resizeSplitter();
        resizeTabStrip();
        resizeSearchSplitter();
        resizeSearchGrid();
    });
});

// ready method
$(function () {
    var userDetail = localStorage.getItem("user-detail");
    if (userDetail) {
        userObj = JSON.parse(userDetail);
    }
});

kendo.syncReady(function () { jQuery("#switch").kendoSwitch({}); });

function showMessage(container, name, errors) {
    //add the validation message to the form
    var validationMessageTmpl = kendo.template($("#message").html());
    var output = validationMessageTmpl({
        field: name,
        message: errors
    }).trim();

    $(output).insertAfter(container.find("[name='" + name + "']"));
}

function error_handler(e) {
    if (e.errors) {
        var message = "Errors:\n";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });
    }
}

$(document).on("submit", '#form-login', function (e) {
    e.preventDefault();
    loginUser();
});

var cat = {
    get name() {
        return "Miffy";
    },
    age: 12
};

$(document).on("click", '#logout-btn', function (e) {
    logOffFunc();
    $.post("/Grid/LogOff", {}, function (response) {
        location.href = '/';
    });
});

$(document).on("click", '#forgot-password-btn', function (e) {
    forgotPassword();
});

function logOffFunc() {
    localStorage.removeItem("user-detail");
    userObj = null;
    toggleLoginForm();
    // if user is in editable mode
    if (isGridEditable) {
        toggleEditable();
        $('.stopEditable').addClass("d-none");
        $('.startEditable').removeClass("d-none");
    }
}

function toggleLoginForm() {
    if (userObj) {
        $('#form-login').addClass('d-none');
        $('#logout-block').removeClass('d-none');
        if (userObj.SuperUser) {
            $('.startEditable').removeClass('k-state-disabled');
            $('.admin-link').removeClass('d-none');
        }
        else {
            $('.startEditable').addClass('k-state-disabled');
            $('.admin-link').addClass('d-none');
        }
    }
    else {
        $('#logout-block').addClass('d-none');
        $('#form-login').removeClass('d-none');
        $('.admin-link').addClass('d-none');
        $('.startEditable').addClass('k-state-disabled');
    }

}

function loginUser() {
    var validator = $("#form-login").data("kendoValidator");
    if (validator.validate()) {
        showLoading();
        var data = objectifyForm($('#form-login').serializeArray());
        $.post("/Grid/Login", data, function (response) {
            hideLoading();
            if (response) {
                // valid credentials
                userObj = response;
                $('.loggedin-user').text(userObj.Name);
                localStorage.setItem("user-detail", JSON.stringify(response));
                toggleLoginForm();
            }
            else {
                // invalid credentials
                showErrorDialog("Error", "Invalid Credentials");
            }
        });

    }
}

function forgotPassword() {
    var validator = $("#form-login").data("kendoValidator");
    if (validator.validate()) {
        showLoading();
        var data = objectifyForm($('#form-login').serializeArray());
        $.post("/Grid/ForgotPassword", data, function (success) {
            showErrorDialog("Success", "Email with login credentials sent successfully.");
            hideLoading();
        });
    }
}

function makeGridEditable() {
    isGridEditable = true;
}

function toggleEditable() {
    isGridEditable = !isGridEditable;
    if (isGridEditable) {
        $('.chkbx').removeAttr('disabled');
        grid.showColumn(grid.columns[0]);
        $('.action-btns').removeClass('k-state-disabled');
    }
    else {
        $('.chkbx').attr('disabled', 'disabled');
        grid.hideColumn(grid.columns[0]);
        $('.action-btns').addClass('k-state-disabled');
        // Enable Single Row Mode
        enableMultiRowEditMode = false;
    }
    toggleFormEnable();
}

function numericEditor(container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoNumericTextBox({
            decimals: 0,
            format: "#",
            spinners: false
        });
}

function checkIfGridEditable(e) {
    return isGridEditable;
}

function loadData(e) {
    if (updatedRowsList.length > 0) {
        showMultiModeConfirmation(true);
        e.preventDefault();
        return;
    }
    isFormWrap = false;
    resetAddNewProps();
    var splitter = $("#horizontal").data("kendoSplitter");
    var treeView = $('#treeview').data('kendoTreeView');
    if (treeView) {
        var tableName = treeView.dataItem(e.node).tableName;
        if (tableName) {
            showLoading();
            splitter.ajaxRequest("#myModal", "/Grid/AdminSearchForm", { TableName: tableName });
        }
    }
}

function loadEditData(obj) {
    var splitter = $("#horizontal").data("kendoSplitter");
    if (!splitter) {
        $("#horizontal").kendoSplitter();
        splitter = $("#horizontal").data("kendoSplitter");
    }
    var treeview = $("#treeview").data("kendoTreeView");
    treeview.dataSource.data().forEach(function (item, index) {
        var foundItem = item.items.find(x => x.tableName === obj);
        if (foundItem) {
            var searchedItem = treeview.findByText(foundItem.Text);
            treeview.select(searchedItem);
        }
    });
    showLoading();
    splitter.ajaxRequest("#myModal", "/Grid/AdminSearchForm", { TableName: obj });
    //splitter.ajaxRequest(".k-pane:last", "/Grid/GetView", { type: obj });
}

function gridDataBoundConfig() {
    customActions();
    toggleEdit();
    toggleFilterbutton();
    if (isWordWrap) {
        $('.k-grid td').removeClass('text-nowrap').addClass('text-wrap');
    }
    else {
        $('.k-grid td').removeClass('text-wrap').addClass('text-nowrap');
    }
    //hideLoading();
    setTimeout(function () {
        resizeGrid();
        resizeSplitter();
        grid.content.scrollLeft(0);
    }, 500);

    if (editfield) {
        showLoading();
        editfield = '';
        setTimeout(function () {
            $('#custom-add-btn').click();
            triggerFormView();
        }, 500);
    }

    if (getSelectTable() === tableNameAddItemDD && grid.dataSource.data().length) {
        var selectedRow = grid.dataSource.data().find(x => x[tableNameAddItemDD + 'ID'] === selectedAddItemDD[tableNameAddItemDD + 'ID']);
        var row = grid.element.find("tr[data-uid='" + selectedRow.uid + "']");
        if (row.length) {
            grid.select(row);
            // Scroll to the item to ensure it is visible
            grid.content.scrollTop(grid.select().position().top);
            var columnIndex = grid.options.columns.findIndex(x => x.field === columnNameAddItemDD);
            if (columnIndex) {
                var td = row.find(`td:eq(${columnIndex})`);
                grid.editCell(td);
                // select added item in dropdown
                if (addedItemDD.Id) {
                    setTimeout(function () {
                        var dropdown = td.find("select[data-role='dropdownlist']");
                        var dropdownList = $(dropdown).data("kendoDropDownList");
                        dropdownList.value(addedItemDD.Id);
                        var dataItem = grid.dataItem(row);
                        dataItem.set(columnNameAddItemDD + "ID", addedItemDD.Id);
                        dataItem.set(columnNameAddItemDD, addedItemDD.Option);
                        dataItem.dirty = true;
                    });
                }
            }
        }
        resetAddNewProps();
    }
}

function onDataBound(e) {
    grid = $("#grid").data("kendoGrid");
    if (gridSelectionIndex) {
        if (grid.dataSource.data().length - 1 > gridSelectionIndex)
            gridSelectionIndex++;
        e.sender.select(`tr:eq(${gridSelectionIndex})`);
    } else {
        e.sender.select("tr:eq(0)");
    }
    gridDataBoundConfig();
    if (isFormWrap) {
        formSwitchEvents();
    }
    if (userObj)
        $('.loggedin-user').text(userObj.Name);
}

function adminGridOnDataBound(e) {
    if (gridSelectionIndex) {
        if (grid.dataSource.data().length - 1 > gridSelectionIndex)
            gridSelectionIndex++;
        grid.select(`tr:eq(${gridSelectionIndex})`);
    } else {
        grid.select("tr:eq(0)");
    }
    gridDataBoundConfig();
    if (isFormWrap) {
        formSwitchEvents();
    }
    if (userObj)
        $('.loggedin-user').text(userObj.Name);
}

function treeViewDataBound() {
    selectTreeViewItem("Works");
}

function selectTreeViewItem(itemText) {
    var treeview = $("#treeview").data("kendoTreeView");
    if (treeview.select().length === 0) {
        var text = treeview.findByText(itemText);
        var itemroot1 = treeview.dataItem(text);
        treeview.select(text);
        treeview.expandTo(itemroot1);
    }
}

function formSwitchEvents() {
    if (grid.dataSource.page() === grid.dataSource.totalPages()) {
        selectedItemIndex = grid.dataSource.data().length - 1;
    }
    formViewSelectedItem = grid.dataSource.data()[selectedItemIndex];
    getFormView();
    selectGridRow();
}

function toggleEdit() {
    if (isGridEditable) {
        $('.stopEditable').removeClass("d-none");
        $('.startEditable').addClass("d-none");
        $('.chkbx').removeAttr('disabled');
        grid.showColumn(grid.columns[0]);
        $('.action-btns').removeClass('k-state-disabled');
    }
    else {
        $('.stopEditable').addClass("d-none");
        $('.startEditable').removeClass("d-none");
        $('.chkbx').attr('disabled', 'disabled');
        grid.hideColumn(grid.columns[0]);
        $('.action-btns').addClass('k-state-disabled');
    }
}

function customActions() {
    if ($("#custom-add-btn").length == 0) {
        var customToolbarTemp = kendo.template($("#custom-toolbar").html());
        $(".k-grid .k-pager-info").addClass('pager-info').after(customToolbarTemp);
        $(".k-grid .k-grid-toolbar").addClass('d-none');
        $("#wrap-switch").kendoSwitch({
            checked: isWordWrap,
            change: function (e) {
                isWordWrap = !isWordWrap;
                if (isWordWrap) {
                    $('.k-grid td').removeClass('text-nowrap').addClass('text-wrap');
                }
                else {
                    $('.k-grid td').removeClass('text-wrap').addClass('text-nowrap');
                }
            }
        });

        $("#form-switch").kendoSwitch({
            checked: isFormWrap,
            change: function (e) {
                resetAddNewProps();
                isFormWrap = !isFormWrap;
                if (isFormWrap) {
                    triggerFormView();
                }
            }
        });
    }
}

function resetAddNewProps() {
    isAddNewItemDD = false, tableNameAddItemDD = "", selectedAddItemDD = null, editfield = '';
}

function triggerFormView() {
    isFormWrap = true;
    formViewSelectedItem = grid.dataItem(grid.select());
    selectedItemIndex = grid.dataSource.data().indexOf(formViewSelectedItem);
    gridFooterEle = $('.k-pager-wrap').clone(true);
    getFormView();
}

function getFormView(tableName, addNewItem) {
    showLoading();
    var treeTable = getSelectTable();
    var data = {
        type: addNewItem ? tableName : treeTable,
        selectedItem: addNewItem ? null : JSON.stringify(formViewSelectedItem, (k, v) => v === undefined ? null : v)
    };
    $.ajax({
        url: '/Grid/GetFormView',
        type: 'POST',
        cache: false,
        dataType: "html",
        data: data
    })
        .done(function (result) {
            hideLoading();
            $('#form-container').html(result);
            $('#form-container').css('visibility', 'visible');
            $('#kendo-grid-container').css('visibility', 'hidden');
            resizeGrid();
            resizeSplitter();
        }).fail(function (xhr) {
            hideLoading();
        });

}

function onClose() {
}

function onOpen(e) {
    $('.dialog input').focus();
}

$(document).on("click", '.startEditable', function (e) {
    var isSuperUser = false, userDetail = localStorage.getItem("user-detail");
    if (userDetail) {
        userObj = JSON.parse(userDetail);
        isSuperUser = userObj.SuperUser;
    }
    if (isSuperUser) {
        isLoggedIn = true;
        toggleEditable();
        $('.startEditable').addClass("d-none");
        $('.stopEditable').removeClass("d-none");
    }
    e.preventDefault();
});

$(document).on("click", '.stopEditable', function (e) {
    toggleEditable();
    $(this).addClass("d-none");
    $('.startEditable').removeClass("d-none");
    e.preventDefault();
});

$(document).on("click", '#custom-add-btn', function (e) {
    $('.k-grid-add').click();
    e.preventDefault();
    e.stopPropagation();
});

$(document).on("click", '#custom-save-btn', function (e) {
    editfield = '', displayFieldSelectedDD = '', updatedRowsList = [];
    var dataItem = grid.dataItem(grid.select());
    gridSelectionIndex = grid.dataSource.data().indexOf(dataItem);
    $('.k-grid-save-changes').click();
    e.preventDefault();
    e.stopPropagation();
});

$(document).on("click", '#custom-cancel-btn', function (e) {
    editfield = '', displayFieldSelectedDD = '';
    $('.k-grid-cancel-changes').click();
    updatedRowsList = [];
    e.preventDefault();
    e.stopPropagation();
});

$(document).on("click", '.removeFilter', function (e) {
    clearFilter();
});

$(document).on("change", "input.chkbx", function (e) {
    var dataItem = grid.dataItem($(e.target).closest("tr"));
    var index = grid.dataItems().indexOf(dataItem);
    if (!updatedRowsList.some(x => x === index))
        updatedRowsList.push(index);
    if (updatedRowsList.length > 1 && !enableMultiRowEditMode) {
        showMultiModeConfirmation();
        $(this).prop('checked', !this.checked);
        return;
    }
    $(e.target).closest("td").prepend("<span class='k-dirty'></span>");
    dataItem[e.target.title] = this.checked;
    dataItem.dirty = true;

});

function getAdditionalParam(e) {
    var widget = $("#dropDownList").data("kendoDropDownList");
    return { items: widget };
}

function populateDDSession(tableName, optionCol) {
    $.post('/Grid/PopulateDD', { treeTable: tableName, optionCol: optionCol }, function (success) {

    });
}

function populateGridSession(type) {
    $.post('/Grid/List', { type: type }, function (success) {

    });
}

function onSelect(e, field) {
    if (e.dataItem.Option === 'Add New' && !editfield) {
        editfield = field;
        loadEditData(editfield);
    }
}

function onDropdownOpen(e, field, displayCol) {
    selectedDropdownEle = e.sender.element;
    editfield = field;
    displayFieldSelectedDD = displayCol;
    var uid = $(e.sender.element.closest('span')).attr('aria-owns').split('_')[0];
    var ddElement = `#${uid}-list`;
    setTimeout(function () {
        $(ddElement + ' .k-textbox').width('78%');
    });
}

function onDropdownDataBound(e) {
    var uid = $(e.sender.element.closest('span')).attr('aria-owns').split('_')[0];
    var ddElement = `#${uid}-list`;
    if (!$(ddElement).hasClass('ui-resizable')) {
        $(ddElement).resizable({
            handles: 'e, w'
        });
    }
}

$(document).on("click", '.dd-new-item', function (e) {
    var dropdownlist = selectedDropdownEle.data("kendoDropDownList");
    dropdownlist.close();
    isAddNewItemDD = true;
    columnNameAddItemDD = selectedDropdownEle.attr('name');
    tableNameAddItemDD = getSelectTable();
    selectedAddItemDD = grid.dataItem(grid.select());
    loadEditData(editfield);
});

$(document).on("click", '.dd-clear', function (e) {
    var dropdownlist = selectedDropdownEle.data("kendoDropDownList");
    dropdownlist.value(0);
    dropdownlist.close();
    var selectedProperty = dropdownlist.element.attr('name');
    var td = dropdownlist.element.closest('td'), tdIndex = td.index();
    var selectedRow = $('tr[data-uid=' + dropdownlist.element.closest('tr').attr("data-uid") + ']');
    var dataItem = grid.dataItem(selectedRow);
    dataItem.set(selectedProperty + "ID", null);
    dataItem.set(selectedProperty, "");
    dataItem.dirty = true;
    kendoFastReDrawRow(grid, selectedRow);
    setTimeout(function () {
        var row = grid.tbody
            .find("tr[data-uid='" + dataItem.uid + "']");
        var cell = row.find("td").eq(tdIndex);
        cell.removeClass('k-edit-cell').addClass("k-dirty-cell").prepend("<span class='k-dirty' />");
    });
});

function cellChangeEvent(e) {
}

function kendoFastReDrawRow(grid, row) {
    var dataItem = grid.dataItem(row);

    var rowChildren = $(row).children('td[role="gridcell"]');

    for (var i = 0; i < grid.columns.length; i++) {

        var column = grid.columns[i];
        var template = column.template;
        var cell = rowChildren.eq(i);

        if (template !== undefined) {
            var kendoTemplate = kendo.template(template);

            // Render using template
            cell.html(kendoTemplate(dataItem));
        } else {
            var fieldValue = dataItem[column.field];

            var format = column.format;
            var values = column.values;

            if (values !== undefined && values != null) {
                // use the text value mappings (for enums)
                for (var j = 0; j < values.length; j++) {
                    var value = values[j];
                    if (value.value == fieldValue) {
                        cell.html(value.text);
                        break;
                    }
                }
            } else if (format !== undefined) {
                // use the format
                cell.html(kendo.format(format, fieldValue));
            } else {
                // Just dump the plain old value
                cell.html(fieldValue);
            }
        }
    }
}

function valueMapperLanguage(options) {
    $.ajax({
        url: "/Grid/Dropdown_ValueMapper",
        data: convertValues(options.value, 'Language'),
        success: function (data) {
            options.success(data);
        }
    });
}

function valueMapperAuthor(options) {
    $.ajax({
        url: "/Grid/Dropdown_ValueMapper",
        data: convertValues(options.value, 'Author'),
        success: function (data) {
            options.success(data);
        }
    });
}

function valueMapperCity(options) {
    $.ajax({
        url: "/Grid/Dropdown_ValueMapper",
        data: convertValues(options.value, 'City'),
        success: function (data) {
            options.success(data);
        }
    });
}

function valueMapperCategory(options) {
    $.ajax({
        url: "/Grid/Dropdown_ValueMapper",
        data: convertValues(options.value, 'Category'),
        success: function (data) {
            options.success(data);
        }
    });
}

function valueMapperCountry(options) {
    $.ajax({
        url: "/Grid/Dropdown_ValueMapper",
        data: convertValues(options.value, 'Country'),
        success: function (data) {
            options.success(data);
        }
    });
}

function valueMapperRegion(options) {
    $.ajax({
        url: "/Grid/Dropdown_ValueMapper",
        data: convertValues(options.value, 'Region'),
        success: function (data) {
            options.success(data);
        }
    });
}

function valueMapperWork(options) {
    $.ajax({
        url: "/Grid/Dropdown_ValueMapper",
        data: convertValues(options.value, 'Work'),
        success: function (data) {
            options.success(data);
        }
    });
}

function valueMapperPublisher(options) {
    $.ajax({
        url: "/Grid/Dropdown_ValueMapper",
        data: convertValues(options.value, 'Publisher'),
        success: function (data) {
            options.success(data);
        }
    });
}

function toggleFilterbutton() {
    if (grid.dataSource.filter() && grid.dataSource.filter().filters.length > 0) {
        $('.removeFilter').removeClass('k-state-disabled');
    }
    else {
        $('.removeFilter').addClass('k-state-disabled');
    }
}

function convertValues(value, type) {
    var data = {};
    value = $.isArray(value) ? value : [value];
    for (var idx = 0; idx < value.length; idx++) {
        data["values[" + idx + "]"] = value[idx];
    }
    data.treeTable = type;
    return data;
}

function dropdownFilter(element) {
    element.removeAttr("data-bind");
    element.kendoMultiSelect({
        dataTextField: "Option",
        dataValueField: "Id",
        dataSource: {
            transport: {
                read: {
                    type: "POST",
                    url: "/Grid/PopulateDD",
                    data: {
                        treeTable: this.treeTable,
                        optionCol: this.optionCol
                    }
                }
            }
        },
        change: function (e) {
            var filter = { logic: "or", filters: [] };
            var values = this.value();
            $.each(values, function (i, v) {
                filter.filters.push({ field: currentFilterColumn, operator: "eq", value: v });
            });
            var dataSource = $("#grid").data().kendoGrid.dataSource;
            dataSource.filter(filter);
            toggleFilterbutton();
        }
    });
}

function columnMenuInit(e) {
    initColumnMenuFilter.call(this, e);
}

function clearFilter() {
    var params = { dataSource: grid.dataSource };
    populateSearch(getSelectTable(), false, params);
    var filter = { logic: "or", filters: [] };
    grid.dataSource.filter(filter);
    toggleFilterbutton();
}

function initColumnMenuFilter(e, isAdminFilter = false) {
    var menu = e.container.find(".k-menu").data("kendoMenu");
    var grid = e.sender;
    var field = e.field;
    var gridName = grid.element.attr('id').split('-')[2];
    var treeTable = isAdminFilter ? getSelectTable() : gridName;
    var helpTextElement = e.container.children(":first").children(":last");
    var filterOneValue, filterLogicValue = 'or', filterTwoValue, inputFilterOne,
        inputFilterTwo, ddFilterOne, ddFilterTwo, fieldType = 'string', fkDisplayColumn, columnFilterOptions = { listViewPageSize: 500 };

    // Get Column type
    var fieldInfo = grid.dataSource.options.schema.model.fields[field];
    if (fieldInfo) {
        fieldType = fieldInfo.type;
    }

    var dataSource = this.dataSource;
    var dataSourceListView = new kendo.data.DataSource({
        type: "aspnetmvc-ajax",
        transport: {
            read: {
                type: "POST",
                url: "/Grid/BindFilterListView",
                data: {
                    treeTable: treeTable,
                    optionCol: field,
                    fieldType: fieldType
                }
            }
        },
        schema: {
            data: function (response) {
                return response.Data;
            },
            total: function (response) {
                return response.Total;
            }
        },
        pageSize: columnFilterOptions.listViewPageSize,
        serverPaging: true,
        serverFiltering: true
    });

    var pageModel = {};
    pageModel[field] = 1;

    menu.bind("open", function (e) {
        if ($(e.item).find('.k-link').first().text() === "Filter") {
            var filterContainer = helpTextElement.children(":last");
            var parentEle = filterContainer.children(":first");
            var elementToInsertAfter = parentEle.find('.k-filter-help-text');
            var container = parentEle.find('form');

            if (parentEle.find('#list-view-' + field).length === 0) {
                $(helpTextElement.children(":last")).resizable({
                    alsoResize: "#list-view-" + field,
                    handles: 'e, w'
                });
                elementToInsertAfter.addClass('d-none');  //hide text above search box
                var advancedOption = $(container).clone();
                var element, ele;
                var gridRowTemplateDD = "<div class='list-Item'>#:Option#</div>";
                var gridRowTemplate = "<div class='list-Item'>#:data#</div>";
                if (isAdminFilter) {
                    gridRowTemplateDD = "<div class='list-Item'><input type='checkbox' value='#:Id#'/>#:Option#</div>";
                    gridRowTemplate = "<div class='list-Item'><input type='checkbox' value='#:data#'/>#:data#</div>";
                }
                switch (fieldType) {
                    case 'object':
                        ele = $("<div class='k-textbox k-space-right'><input type='text' id='filter-search-" + field + "' placeholder='Search'><span class='k-icon k-i-zoom'></span></div>").insertAfter(elementToInsertAfter);
                        element = $("<div id='list-view-" + field + "' class='checkbox-ontainer kendo-grid-filter'></div>").insertAfter(ele).kendoGrid({
                            dataSource: dataSourceListView,
                            height: 210,
                            resizable: true,
                            selectable: !isAdminFilter ? "multiple" : false,
                            scrollable: {
                                virtual: true
                            },
                            columns: [
                                { field: 'Id', hidden: true },
                                {
                                    field: '', template: gridRowTemplateDD
                                }
                            ],
                            dataBound: function (e) {
                                var filter = dataSource.filter() || { logic: "and", filters: [] };
                                var field = e.sender.element.attr('id').split('-')[2] + 'ID';
                                var existingFilters = Array.prototype.concat(...filter.filters.map(x => x.filters));
                                var list = e.sender.dataSource.data();
                                existingFilters.forEach(function (item, index) {
                                    // Check existing checkboxes
                                    var selectedItem = list.find(x => x.Id === item.value);
                                    if (selectedItem && item.field === field) {
                                        var selectedIndex = list.indexOf(selectedItem);
                                        e.sender.tbody.find(`tr:eq(${selectedIndex}) td:eq(0) input`).prop('checked', true);
                                    }
                                });
                                $(e.sender.element).find('.k-grid-content').height(198);
                            }
                        });
                        break;
                    case 'boolean':
                        ele = $("<div class='k-textbox k-space-right'><input disabled=disabled id='filter-search-" + field + "' placeholder='Search'><span class='k-icon k-i-zoom'></span></div>").insertAfter(elementToInsertAfter);
                        if (isAdminFilter) {
                            gridTemplateBool = `<div id='list-view-${field}' class='checkbox-ontainer'>
                                            <div class='list-Item'><input type='checkbox' value='true' class='filter-chk'/>True</div>
                                            <div class='list-Item'><input type='checkbox' value='false' class='filter-chk'/>False</div>
                                            <div class='list-Item'><input type='checkbox' value='null' class='filter-chk'/>Null</div>
                                        </div>`;
                            element = $(gridTemplateBool).insertAfter(ele);
                        }
                        else {
                            element = $("<div id='list-view-" + field + "' class='checkbox-ontainer kendo-grid-filter'></div>").insertAfter(ele).kendoGrid({
                                dataSource: ["true", "false", "null"],
                                height: 210,
                                resizable: true,
                                selectable: !isAdminFilter ? "multiple" : false,
                                columns: [
                                    {
                                        field: '', template: gridRowTemplate
                                    }
                                ],
                                dataBound: function (e) {
                                    var filter = dataSource.filter() || { logic: "and", filters: [] };
                                    var existingFilters = Array.prototype.concat(...filter.filters.map(x => x.filters));
                                    var list = e.sender.dataSource.data();
                                    var field = e.sender.element.attr('id').split('-')[2];
                                    existingFilters.forEach(function (item, index) {
                                        // Check existing checkboxes
                                        var selectedItem = list.find(x => x === item.value);
                                        if (selectedItem && item.field === field) {
                                            var selectedIndex = list.indexOf(selectedItem);
                                            e.sender.tbody.find(`tr:eq(${selectedIndex}) td:eq(0) input`).prop('checked', true);
                                        }
                                    });
                                    $(e.sender.element).find('.k-grid-content').height(198);
                                }
                            });
                        }
                        break;
                    default:
                        ele = $("<div class='k-textbox k-space-right'><input type='text' id='filter-search-" + field + "' placeholder='Search'><span class='k-icon k-i-zoom'></span></div>").insertAfter(elementToInsertAfter);
                        element = $("<div id='list-view-" + field + "' class='checkbox-ontainer kendo-grid-filter'></div>").insertAfter(ele).kendoGrid({
                            dataSource: dataSourceListView,
                            height: 210,
                            resizable: true,
                            selectable: !isAdminFilter ? "multiple" : false,
                            scrollable: {
                                virtual: true
                            },
                            columns: [
                                {
                                    field: '', template: gridRowTemplate
                                }
                            ],
                            dataBound: function (e) {
                                var filter = dataSource.filter() || { logic: "and", filters: [] };
                                var existingFilters = Array.prototype.concat(...filter.filters.map(x => x.filters));
                                var list = e.sender.dataSource.data();
                                var field = e.sender.element.attr('id').split('-')[2];
                                existingFilters.forEach(function (item, index) {
                                    // Check existing checkboxes
                                    var selectedItem = list.find(x => x === item.value);
                                    if (selectedItem && item.field === field) {
                                        var selectedIndex = list.indexOf(selectedItem);
                                        e.sender.tbody.find(`tr:eq(${selectedIndex}) td:eq(0) input`).prop('checked', true);
                                    }
                                });
                                $(e.sender.element).find('.k-grid-content').height(198);
                            }
                        });
                        break;
                }
                var filterGrid = element.data('kendoGrid');

                if (isAdminFilter) {
                    var advancedMenu = $("<ul id='advanced-menu-" + field + "'></ul>").insertAfter($("#list-view-" + field)).kendoMenu({
                        orientation: 'vertical'
                    }).data("kendoMenu");

                    $(advancedOption).find("[data-role='numerictextbox']").remove();
                    advancedMenu.append([
                        {
                            text: "Advanced",
                            encoded: false,
                            content: "<div class='p-1'>" + advancedOption.html() + "</div>"
                        }
                    ]);

                    if (fieldType === 'boolean') {
                        var item = advancedMenu.element.eq(0).children("li").eq(0);
                        advancedMenu.enable(item, false);
                    }

                    if (fieldType !== 'boolean') {
                        // Bind advance search options
                        advancedMenu.bind("open", function (e) {
                            if (!inputFilterOne && !inputFilterTwo) {
                                var advancedOptionFrm = $('#advanced-menu-' + field).find('.k-filter-menu-container');
                                if (fieldType === 'object') {
                                    var inputFirst = advancedOptionFrm.find("input:eq(0)");
                                    var inputSecond = advancedOptionFrm.find("input:eq(1)");
                                    inputFirst.removeClass('k-textbox').removeAttr("data-bind");
                                    inputSecond.removeClass('k-textbox').removeAttr("data-bind");
                                    var fkTableDetail = gridColumns.find(x => x.DisplayName === field);
                                    ddFilterOne = $(inputFirst).kendoDropDownList({
                                        dataSource: {
                                            type: "aspnetmvc-ajax",
                                            transport: {
                                                read: `/Grid/GetDropdownOptions?treeTable=${fkTableDetail.Fktable}&optionCol=${fkTableDetail.FkdisplayCol}`
                                            },
                                            schema: {
                                                model: {
                                                    fields: {
                                                        Id: { type: "number" },
                                                        Option: { type: "string" }
                                                    }
                                                },
                                                data: function (response) {
                                                    return response.Data;
                                                },
                                                total: function (response) {
                                                    return response.Total;
                                                }
                                            },
                                            pageSize: 80,
                                            serverPaging: true,
                                            serverFiltering: true
                                        },
                                        dataTextField: "Option",
                                        dataValueField: "Id",
                                        filter: "contains",
                                        virtual: {
                                            itemHeight: 26,
                                            valueMapper: function (options) {
                                                $.ajax({
                                                    url: "/Grid/Dropdown_ValueMapper",
                                                    data: convertValues(options.value, field),
                                                    success: function (data) {
                                                        options.success(data);
                                                    }
                                                });
                                            }
                                        },
                                        change: function (e) {
                                            inputFilterOne = this.value();
                                        },
                                        height: 135
                                    }).data("kendoDropDownList");

                                    ddFilterOne.setOptions({ optionLabel: "Select" });
                                    ddFilterOne.refresh();
                                    ddFilterOne.select(0);

                                    ddFilterTwo = $(inputSecond).kendoDropDownList({
                                        dataSource: {
                                            type: "aspnetmvc-ajax",
                                            transport: {
                                                read: `/Grid/GetDropdownOptions?treeTable=${fkTableDetail.Fktable}&optionCol=${fkTableDetail.FkdisplayCol}`
                                            },
                                            schema: {
                                                model: {
                                                    fields: {
                                                        Id: { type: "number" },
                                                        Option: { type: "string" }
                                                    }
                                                },
                                                data: function (response) {
                                                    return response.Data;
                                                },
                                                total: function (response) {
                                                    return response.Total;
                                                }
                                            },
                                            pageSize: 80,
                                            serverPaging: true,
                                            serverFiltering: true
                                        },
                                        dataTextField: "Option",
                                        dataValueField: "Id",
                                        filter: "contains",
                                        virtual: {
                                            itemHeight: 26,
                                            valueMapper: function (options) {
                                                $.ajax({
                                                    url: "/Grid/Dropdown_ValueMapper",
                                                    data: convertValues(options.value, field),
                                                    success: function (data) {
                                                        options.success(data);
                                                    }
                                                });
                                            }
                                        },
                                        change: function (e) {
                                            inputFilterTwo = this.value();
                                        },
                                        height: 135
                                    }).data("kendoDropDownList");

                                    ddFilterTwo.setOptions({ optionLabel: "Select" });
                                    ddFilterTwo.refresh();
                                    ddFilterTwo.select(0);
                                    // operation and logic dropdowns
                                    var firstValueDropDown = advancedOptionFrm.find("select:eq(0)");
                                    var logicDropDown = advancedOptionFrm.find("select:eq(1)");
                                    var secondValueDropDown = advancedOptionFrm.find("select:eq(2)");
                                    logicDropDown.kendoDropDownList();

                                    // operation dropdowns
                                    var data = [
                                        { text: "Is equal to", value: "eq" },
                                        { text: "Is not equal to", value: "neq" },
                                        { text: "Is null", value: "isnull" },
                                        { text: "Is not null", value: "isnotnull" },
                                        { text: "Less than or equal to", value: "lte" },
                                        { text: "Greater than or equal to", value: "gte" }
                                    ];
                                    $(firstValueDropDown).kendoDropDownList({
                                        dataTextField: "text",
                                        dataValueField: "value",
                                        dataSource: data,
                                        index: 0,
                                        change: function (e) {
                                            filterOneValue = this.value();
                                        }
                                    });
                                    $(secondValueDropDown).kendoDropDownList({
                                        dataTextField: "text",
                                        dataValueField: "value",
                                        dataSource: data,
                                        index: 0,
                                        change: function (e) {
                                            filterTwoValue = this.value();
                                        }
                                    });

                                    filterOneValue = firstValueDropDown.val();
                                    filterTwoValue = secondValueDropDown.val();
                                    filterLogicValue = logicDropDown.val();

                                    $(logicDropDown).change(function (e) {
                                        filterLogicValue = this.value;
                                    });
                                }
                                else {
                                    var filterOne = advancedOptionFrm.find('select:eq(0)');
                                    var filterLogic = advancedOptionFrm.find('select:eq(1)');
                                    var filterTwo = advancedOptionFrm.find('select:eq(2)');
                                    filterOne.kendoDropDownList();
                                    filterLogic.kendoDropDownList();
                                    filterTwo.kendoDropDownList();
                                    filterOneValue = filterOne.val();
                                    filterTwoValue = filterTwo.val();
                                    filterLogicValue = filterLogic.val();
                                    if (fieldType === 'number') {
                                        advancedOptionFrm.children('.k-numerictextbox').remove();

                                        $("<input />").insertAfter(advancedOptionFrm.children("span:first")).kendoNumericTextBox({
                                            spinners: false,
                                            decimals: 0,
                                            format: "#"
                                        });
                                        $("<input />").insertAfter(advancedOptionFrm.children("span:last")).kendoNumericTextBox({
                                            spinners: false,
                                            decimals: 0,
                                            format: "#"
                                        });
                                    }
                                    else {
                                        advancedOptionFrm.children('.k-textbox').remove();
                                        $("<textarea class='k-textbox'></textarea>").insertAfter(advancedOptionFrm.children("span:first"));
                                        $("<textarea class='k-textbox'></textarea>").insertAfter(advancedOptionFrm.children("span:last"));
                                    }
                                    var inputFilters = advancedOptionFrm.find('input[style*="display: none"]');
                                    if (inputFilters.length === 0) {
                                        inputFilters = advancedOptionFrm.find('input:not([style*="display: none"])');
                                    }
                                    if (inputFilters.length) {
                                        $(inputFilters[0]).on('keyup mouseup change', function (e) {
                                            inputFilterOne = this.value;
                                        });

                                        $(inputFilters[1]).on('keyup mouseup change', function (e) {
                                            inputFilterTwo = this.value;
                                        });
                                    }

                                    var textareaFilters = advancedOptionFrm.find('textarea');

                                    if (textareaFilters.length) {
                                        $(textareaFilters[0]).on('keyup mouseup change', function (e) {
                                            inputFilterOne = this.value;
                                        });

                                        $(textareaFilters[1]).on('keyup mouseup change', function (e) {
                                            inputFilterTwo = this.value;
                                        });
                                    }

                                    $(filterOne).change(function (e) {
                                        filterOneValue = this.value;
                                    });

                                    $(filterTwo).change(function (e) {
                                        filterTwoValue = this.value;
                                    });

                                    $(filterLogic).change(function (e) {
                                        filterLogicValue = this.value;
                                    });
                                }
                            }
                            setTimeout(function () {
                                $('#advanced-menu-' + field).find('input:first').focus();
                            }, 100);
                        });
                    }

                    // remove elements after advanced menu
                    $(container).find("ul").nextAll('span').remove();
                    $(container).find("ul").nextAll('input').remove();
                }
                else {
                    $(container).find("#list-view-" + field).nextAll('span').remove();
                    $(container).find("#list-view-" + field).nextAll('input').remove();
                }
                $(container).find("#list-view-" + field).nextAll('label').remove();
                // Filter Submit click Callback
                container.find("[type='submit']").click(function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    if (element) {
                        var filter = dataSource.filter() || { logic: "and", filters: [] };
                        var fieldFilters = [];
                        if (isAdminFilter) {
                            if (fieldType !== 'boolean') {
                                if (isAdminFilter) {
                                    fieldFilters = $.map(element.find(":checkbox:checked"), function (input, index) {
                                        return {
                                            field: fieldType === 'object' ? field + 'ID' : field,
                                            operator: 'in list',
                                            value: parseValue(fieldType, input.value),
                                            tableName: treeTable,
                                            columnType: fieldType,
                                            andOr: 'or'
                                        };
                                    });
                                }
                                // Get Field Filters
                                if (inputFilterOne || chkNullableFilter(filterOneValue)) {
                                    fieldFilters.push({
                                        field: fieldType === 'object' ? field + 'ID' : field,
                                        operator: filterOneValue,
                                        value: parseValue(fieldType, inputFilterOne),
                                        tableName: treeTable,
                                        columnType: fieldType,
                                        andOr: filterLogicValue
                                    });
                                }
                                if (inputFilterTwo) {
                                    fieldFilters.push({
                                        field: fieldType === 'object' ? field + 'ID' : field,
                                        operator: filterTwoValue,
                                        value: parseValue(fieldType, inputFilterTwo),
                                        tableName: treeTable,
                                        columnType: fieldType,
                                        andOr: filterLogicValue
                                    });
                                }
                            }
                            else {
                                fieldFilters = $.map(element.find(":checkbox:checked"), function (input, index) {
                                    return {
                                        field: field,
                                        operator: 'in list',
                                        value: parseValue(fieldType, input.value),
                                        tableName: treeTable,
                                        columnType: fieldType,
                                        andOr: 'or'
                                    };
                                });
                            }
                        }
                        else {
                            var selectedItems = filterGrid.select();
                            selectedItems.each(function () {
                                var value = filterGrid.dataItem(this);
                                fieldFilters.push({
                                    field: field,
                                    operator: 'in list',
                                    value: parseValue(fieldType, value),
                                    tableName: treeTable,
                                    columnType: fieldType,
                                    andOr: 'or'
                                });
                            });
                            //if (fieldType !== 'boolean') {
                            //    selectedItems.each(function () {
                            //        var value = filterGrid.dataItem(this);
                            //        fieldFilters.push({
                            //            field: field,
                            //            operator: 'in list',
                            //            value: parseValue(fieldType, value),
                            //            tableName: treeTable,
                            //            columnType: fieldType,
                            //            andOr: 'or'
                            //        });
                            //    });
                            //}
                            //else {

                            //}
                        }

                        if (fieldFilters.length) {
                            removeFiltersForField(filter, field);
                            //filter.filters = [];  //change
                            var existingFilters = Array.prototype.concat(...filter.filters.map(x => x.filters));
                            if (existingFilters && existingFilters.length) {
                                //filter.filters.filters = filter.filters.filters.filter(x => x.field !== fieldFilters[0].field);
                                var otherColumnfilters = existingFilters.filter(x => fieldFilters.some(y => y.field !== x.field));
                                if (otherColumnfilters.length) {
                                    //filter.filters = ; 
                                    var indexToRemove = null;
                                    filter.filters.forEach(function (value, index) {
                                        if (value.filters[0].field === fieldFilters[0].field) {
                                            indexToRemove = index;
                                            return false;
                                        }
                                    });
                                    if (indexToRemove !== null) {
                                        filter.filters.splice(indexToRemove, 1);
                                    }
                                }
                                else {
                                    filter.filters = [];
                                }
                            }
                            filter.filters.push({
                                logic: filterLogicValue,
                                filters: fieldFilters
                            });

                            var columnFilters = Array.prototype.concat(...filter.filters.map(x => x.filters));
                            var sortArr = grid.dataSource.sort();
                            var sortField = '', sortDir = 'asc';
                            if (sortArr && sortArr.length) {
                                sortField = sortArr[0].field;
                                sortDir = sortArr[0].dir;
                            }
                            var params = {
                                dataSource: grid.dataSource,
                                sortField: sortField,
                                sortDir: sortDir,
                                columnFilters: columnFilters
                            };
                            populateSearch(treeTable, false, params);
                            dataSource.filter(filter);
                        }
                        var popup = $(helpTextElement.children(":last").children(":first")).data("kendoPopup");
                        popup.close();
                        menu.wrapper.parent().hide();
                        toggleFilterbutton();
                    }
                });

                // Filter reset click callback
                container.find("[type='reset']").click(function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    container.find("input[type=text], textarea").val('');
                    container.find('input:checkbox').removeAttr('checked');
                    container.find("input[data-role='dropdownlist']").each(function () {
                        var item = $(this).data("kendoDropDownList");
                        if (item) {
                            item.select(0);
                        }
                    });
                    //var gridType = getSelectTable(); // grid.element.attr('id').split('-')[2];
                    var params = { dataSource: grid.dataSource };
                    populateSearch(treeTable, false, params);
                    var filter = { logic: "and", filters: [] };
                    dataSource.filter(filter);
                    var gridView = $('#list-view-' + field).data('kendoGrid');
                    if (gridView) {
                        gridView.dataSource.filter(filter);
                    }
                    if (filterGrid) {
                        filterGrid.clearSelection();
                    }
                    var popup = $(helpTextElement.children(":last").children(":first")).data("kendoPopup");
                    popup.close();
                    menu.wrapper.parent().hide();
                });
                // Filter search text event
                $('#filter-search-' + field).on('keyup', function (e) {
                    if (e.which <= 90 && e.which >= 48) {
                        if (fieldType === 'object') {
                            var gridView = $('#list-view-' + field).data('kendoGrid');
                            gridView.dataSource.filter({ field: "Option", operator: "contains", value: this.value });
                        }
                        else {
                            gridView = $('#list-view-' + field).data('kendoGrid');
                            gridView.dataSource.filter({ field: field, operator: "contains", value: this.value });
                        }
                        toggleFilterbutton();
                    }
                });

                var resizableAdvanceSearchEle = $('#advanced-menu-' + field).children(":first").children(":last");
                $(resizableAdvanceSearchEle).resizable();
            }
            else {
                parentEle.find('#list-view-' + field).find('.k-grid-content').height(198);
            }
            setTimeout(function () {
                $('#filter-search-' + field).focus();
            }, 600);

        }
    });
}

function chkNullableFilter(operator) {
    switch (operator) {
        case 'isnull':
        case 'isnotnull':
        case 'isempty':
        case 'isnotempty':
        case 'isnullorempty':
        case 'isnotnullorempty':
            return true;
        default:
            return false;
    }
}

function removeDuplicates(items, field) {
    var getter = function (item) { return item[field] },
        result = [],
        index = 0,
        seen = {};

    while (index < items.length) {
        var item = items[index++],
            text = getter(item);

        if (text !== undefined && text !== null && !seen.hasOwnProperty(text)) {
            result.push(item);
            seen[text] = true;
        }
    }

    return result;
}

//////////////////////////////////  Custom Filter  //////////////////////////////

function parseValue(type, value) {
    switch (type) {
        case 'number':
            return parseInt(value);
        case 'boolean':
            if (value == 'true')
                return true;
            else if (value == 'false')
                return false;
            else
                return null;
        case 'object':
            return parseInt(value);//(typeof value == 'string') ? value : parseInt(value)
        default:
            return value;
    }
}

function removeFiltersForField(expression, field) {
    if (expression.filters) {
        expression.filters = $.grep(expression.filters, function (filter) {
            removeFiltersForField(filter, field);
            if (filter.filters) {
                return filter.filters.length;
            } else {
                return filter.field != field;
            }
        });
    }
}

function uniqueForField(data, field, fieldType) {
    var map = {};
    var result = [];
    var item;
    for (var i = 0; i < data.length; i++) {
        item = data[i];
        if (fieldType === 'object') {
            if (!map[item[field]['Option']] && item[field]['Option']) {
                result.push(item);
                map[item[field]['Option']] = true;
            }
        }
        else {
            if (!map[item[field]] && item[field]) {
                result.push(item);
                map[item[field]] = true;
            }
        }
    }

    if (fieldType === 'object') {
        result.sort(function (a, b) {
            var optionA = a[field]['Option'].toLowerCase(), optionB = b[field]['Option'].toLowerCase();
            if (optionA < optionB) //sort string ascending
                return -1;
            if (optionA > optionB)
                return 1;
            return 0; //default return value (no sorting)
        });
    }
    return result;
}

function objectifyForm(formArray) {//serialize data function
    formArray = formArray.concat(
        $('form input[type=checkbox]').map(
            function () {
                if (this.name) {
                    if (this.checked)
                        return { "name": this.name, "value": true };
                    else {
                        return { "name": this.name, "value": false };
                    }
                }
            }).get());
    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = $.isNumeric(formArray[i]['value']) ? parseInt(formArray[i]['value']) : formArray[i]['value'];
    }
    return returnArray;
}

$(document).on("click", '.k-grid-delete', function (e) {
    var ids = [];
    $('.grid-item:checked').each(function () {
        ids.push($(this).attr("id"));
    });
    if (ids.length) {
        // Show dialog before delete
        var dialog = $('#dialog-confirm');
        dialog.kendoDialog({
            width: "300px",
            title: "Confirmation",
            closable: false,
            modal: false,
            content: "<p>Delete checked rows?<p>",
            actions: [
                {
                    text: 'Yes',
                    primary: true,
                    action: function (e) {
                        openDeleteDialog(ids);
                        return true;
                    }
                },
                { text: 'No' },
                { text: 'Cancel' }
            ]
        });
        dialog.data("kendoDialog").open();
    }
    e.preventDefault();
});

function openDeleteDialog(ids) {
    var dialog = $('#dialog-confirm-delete');
    dialog.kendoDialog({
        width: "400px",
        title: "Confirmation",
        closable: false,
        modal: false,
        content: `<p>This will permanently delete ${ids.length} rows, and cannot be undone. Are you really sure?<p>`,
        actions: [
            {
                text: 'Yes',
                primary: true,
                action: function (e) {
                    removeItems(ids);
                    return true;
                }
            },
            { text: 'No' },
            { text: 'Cancel' }
        ]
    });
    dialog.data("kendoDialog").open();
}

function removeItems(ids) {
    var uidArray = [];
    $.each(ids, function (index, value) {
        var uid = $('#' + value).closest('tr').attr("data-uid");
        uidArray.push(uid);
    });
    ids = [];
    $.each(uidArray, function (index, value) {
        var dataItem = grid.dataItem($('tr[data-uid=' + value + ']'));
        grid.dataSource.remove(dataItem);
    });
    grid.dataSource.sync();
}

// Form View Operations
$(document).on("click", '#save-form-btn', function (e) {
    e.preventDefault();
    var validator = $("#edit-form").data("kendoValidator");
    if (validator.validate()) {
        showLoading();
        var data = objectifyForm($('#edit-form').serializeArray());
        var arr = [];
        arr.push(data);
        var treeTable = getSelectTable();
        var idField = $("#edit-form").find('input[id-col]').attr('name'); //treeTable + "ID";
        if (!data[idField]) {  //Add
            $.post('/Grid/Add', { tableName: treeTable, models: JSON.stringify(arr), columns: gridColumns })
                .done(function (response) {
                    var id = response.Data[0][idField];

                    // If add form comes from adding new option to dropdown
                    if (isAddNewItemDD) {
                        // Show up admin grid where add new item get triggered
                        // Select that perticular item
                        // Select that perticular cell
                        addedItemDD = {
                            Id: id,
                            Option: data[displayFieldSelectedDD]
                        };
                        backToGridDD();
                    }
                    else {
                        data[idField] = id;
                        data["RowNum"] = grid.dataSource.total() + 1;
                        grid.dataSource.add(data);
                        formViewSelectedItem = grid.dataSource.data().find(x => x[idField] === data[idField]);
                        selectedItemIndex = grid.dataSource.data().indexOf(formViewSelectedItem);
                        selectGridRow();
                        getFormView();
                    }
                })
                .fail(function () {
                    hideLoading();
                });
        }
        else {  //Update
            $.post('/Grid/Update', { tableName: treeTable, models: JSON.stringify(arr), columns: gridColumns })
                .done(function (response) {
                    $.each(formViewSelectedItem, function (name, value) {
                        if (data.hasOwnProperty(name) && data[name] !== formViewSelectedItem[name]) {
                            formViewSelectedItem.set(name, data[name]);  //TBD Call GerFormView each time
                        }
                    });
                    $('.k-dirty').removeClass('k-dirty');
                    hideLoading();
                })
                .fail(function () {
                    hideLoading();
                });
        }
    }
});

function backToGridDD() {
    isFormWrap = false;
    var treeview = $("#treeview").data("kendoTreeView");
    treeview.dataSource.data().forEach(function (item, index) {
        var foundItem = item.items.find(x => x.tableName === tableNameAddItemDD);
        if (foundItem) {
            var searchedItem = treeview.findByText(foundItem.Text);
            treeview.select(searchedItem);
        }
    });
    var splitter = $("#horizontal").data("kendoSplitter");
    splitter.ajaxRequest("#myModal", "/Grid/AdminSearchForm", { TableName: tableNameAddItemDD });
    //splitter.ajaxRequest(".k-pane:last", "/Grid/GetView", { type: tableNameAddItemDD });
    isAddNewItemDD = false;
}

$(document).on("click", '#cancel-form-btn', function (e) {
    e.preventDefault();
    $('.k-dirty').removeClass('k-dirty');
    if (isAddNewItemDD) {
        showLoading();
        backToGridDD();
    }
    else {
        if ($('input[type=hidden]').val()) {
            $("form").trigger("reset");
        }
        else {
            addNewRecord();
        }
    }
});

$(document).on("click", '#first-form-btn', function (e) {
    showLoading();
    grid.dataSource.page(1);
    selectedItemIndex = 0;
    $('#first-form-btn').addClass('k-state-disabled');
    if ($('#last-form-btn').hasClass('k-state-disabled')) {
        $('#last-form-btn').removeClass('k-state-disabled');
    }
});

$(document).on("click", '#last-form-btn', function (e) {
    showLoading();
    var totalPage = grid.dataSource.totalPages();
    grid.dataSource.page(totalPage);
    $('#last-form-btn').addClass('k-state-disabled');
    if ($('#first-form-btn').hasClass('k-state-disabled')) {
        $('#first-form-btn').removeClass('k-state-disabled');
    }
});

$(document).on("click", '#prev-form-btn', function (e) {
    showLoading();
    selectedItemIndex--;
    formViewSelectedItem = grid.dataSource.data()[selectedItemIndex];
    getFormView();
    if (selectedItemIndex === 0) {
        $('#prev-form-btn').addClass('k-state-disabled');
    }
    else if ($('#prev-form-btn').hasClass('k-state-disabled')) {
        $('#prev-form-btn').removeClass('k-state-disabled');
    }
    selectGridRow();
});

$(document).on("click", '#next-form-btn', function (e) {
    showLoading();
    selectedItemIndex++;
    formViewSelectedItem = grid.dataSource.data()[selectedItemIndex];
    getFormView();
    if (selectedItemIndex === grid.dataSource.data().length - 1) {
        $('#prev-form-btn').addClass('k-state-disabled');
    }
    else if ($('#prev-form-btn').hasClass('k-state-disabled')) {
        $('#prev-form-btn').removeClass('k-state-disabled');
    }
    selectGridRow();
});

$(document).on("click", '#add-form-btn', function (e) {
    e.preventDefault();
    addNewRecord();
});

// Increase Font Size          
$(document).on("click", '.k-grid-fontPlus', function (e) {
    currentFontSize = $('.k-grid').css('font-size');
    currentFontSize = parseFloat(currentFontSize) * 1.2;
    if (currentFontSize <= 22) {
        $('.k-grid').css('font-size', currentFontSize);
        resizeGrid();
        resizeSplitter();
    }
    return false;
});

// Decrease Font Size       
$(document).on("click", '.k-grid-fontMinus', function (e) {
    currentFontSize = $('.k-grid').css('font-size');
    currentFontSize = parseFloat(currentFontSize) * 0.8;
    if (currentFontSize >= 8) {
        $('.k-grid').css('font-size', currentFontSize);
        resizeGrid();
        resizeSplitter();
    }
    return false;
});
//$(document).on("click", '.k-grid-fontPlus', function (e) {
//    //var originalSize = $('div').css('font-size');
//    //$('.k-grid').style('font-size', )
//    //e.preventDefault();
//    //addNewRecord();
//});

//$(document).on("click", '.k-grid-fontMinus', function (e) {
//    //e.preventDefault();
//    //addNewRecord();
//});

function addNewRecord() {
    $('.k-dirty').removeClass('k-dirty');
    var formName = '#edit-form';
    $(formName + " input[data-role='dropdownlist']").each(function () {
        var item = $(this).data("kendoDropDownList");
        if (item) {
            item.value("");
            item.text("");
        }
    });
    $(formName + " input[data-role='numerictextbox']").each(function () {
        var item = $(this).data("kendoNumericTextBox");
        if (item) {
            item.value(null);
        }
    });
    $(formName + " :input").each(function () {
        this.value = '';
    });
    $(formName + ' input:checkbox').removeAttr('checked');
    $(".form-primary-key").hide();
}

function toggleFormView() {
    isFormWrap = !isFormWrap;
    if (!isFormWrap) {
        $('#form-container').css('visibility', 'hidden');
        $('#kendo-grid-container').css('visibility', 'visible');
        resizeGrid();
        resizeSplitter();
        browserWindow.resize(resizeSplitter);
        var formSwitch = $("#form-switch").data('kendoSwitch');
        formSwitch.check(isFormWrap);
        selectGridRow();
    }
}

function loadFormData() {
    var splitter = $("#myModal").data("kendoSplitter");
    var treeTable = getSelectTable();
    splitter.ajaxRequest(".k-pane:last", "/Grid/GetFormView", { type: treeTable, selectedItem: JSON.stringify(formViewSelectedItem) });
}

function selectGridRow() {
    grid.select(`tr:eq(${selectedItemIndex})`);
    grid.content.animate({ scrollTop: grid.select().position().top }, 400);
}

function toggleFormEnable() {
    var formName = '#edit-form';
    if ($(formName).length) {
        $(formName + " input[data-role='dropdownlist']").each(function () {
            var item = $(this).data("kendoDropDownList");
            if (item) {
                item.enable(isGridEditable);
            }
        });

        $(formName + " input[data-role='numerictextbox']").each(function () {
            var item = $(this).data("kendoNumericTextBox");
            if (item) {
                item.enable(isGridEditable);
            }
        });

        if (!isGridEditable) {
            $(formName + " :input").attr('disabled', 'disabled');
            $(formName + ' input:checkbox').attr('disabled', 'disabled');
        }
        else {
            $(formName + " :input").removeAttr('disabled');
            $(formName + ' input:checkbox').removeAttr('disabled');
        }
    }
}

//$('.k-popup').resizable();

function displayLoading(target) {
    var element = $(target);
    kendo.ui.progress(element, true);
}

function endLoading(target) {
    var element = $(target);
    kendo.ui.progress(element, false);
}

// End Form View Operations


//Searh Form operations
var searchParam = {
    PageSize: 1000,
    SortField: '',
    SortDirection: 'asc'
};
var searchGridOptions = {}, adminGridOptions = {}, gridElement;

function getSearchParams(tableName, isView, config = {}) {
    if (!config.page) config.page = 0;
    var filters = [], requestParam = [], params = [];
    var searchForms = $('form');
    if (isView && !config.isGlobalSearch) {
        $.each(searchForms, function (index, value) {
            var data = objectifyForm($(this).serializeArray());
            $.each(data, function (name, value) {
                var propArr = name.split('.');
                var member = propArr[2];
                var tableName = propArr[0];
                if (name.indexOf("Filter") >= 0) {
                    var operator = value;  // Operator Value
                    switch (operator) {
                        case 'in list':
                            var memberValue = data[`${tableName}.${member}ID.hdn`];
                            break;
                        default:
                            memberValue = data[`${tableName}.${member}`];
                            break;
                    }
                    if (Array.isArray(memberValue)) {
                        if (memberValue.length)
                            memberValue = memberValue.join('|');
                        else
                            memberValue = '';
                    }
                    if (memberValue) {
                        filters.push({
                            'field': member,
                            'operator': operator,
                            'value': memberValue,
                            'tableName': tableName,
                            'columnType': 'text'
                        });
                    }
                }
                else if (propArr[1] === "Start") {
                    if (member && value) {
                        filters.push({
                            'field': member,
                            'operator': "gte",
                            'value': value,
                            'tableName': tableName,
                            'columnType': 'number'
                        });
                    }
                }
                else if (propArr[1] === "End") {
                    if (member && value) {
                        filters.push({
                            'field': member,
                            'operator': "lte",
                            'value': value,
                            'tableName': tableName,
                            'columnType': 'number'
                        });
                    }
                }
            });
        });

        if (config && config.isAdvanceSearch) {
            filters.forEach(x => x.andOr = 'and');
        }
        var filterArr = { logic: "and", filters: [] };
        filterArr.filters = filters;
        filters.forEach(function (item, index) {
            params.push({
                tableName: item.tableName,
                columnName: item.field,
                columnType: item.columnType,
                comparisonType: item.operator,
                textValue: item.value,
                andOr: item.andOr
            });
        });
    }
    else if (config.columnFilters) {
        for (var i = 0; i < config.columnFilters.length; i++) {
            var item = config.columnFilters[i], columnType;
            if (!params.some(x => x.columnName === item.field && x.comparisonType === item.operator)) {
                var value = item.value, sameColumns = config.columnFilters.filter(x => x.field === item.field && (x.operator === 'eq' || x.operator === 'in list'));
                if (sameColumns.length > 1) {
                    value = sameColumns.map(x => x.value).join('|');
                }
                textValue = value;
                switch (item.columnType) {
                    case 'string':
                        columnType = 'text';
                        break;
                    case 'number':
                        columnType = 'number';
                        break;
                    case 'object':
                        columnType = 'number';
                        break;
                    default:
                        columnType = 'text';
                        break;
                }
                params.push({
                    tableName: item.tableName,
                    columnName: item.field,
                    columnType: columnType,
                    comparisonType: item.operator,
                    textValue: value,
                    andOr: item.andOr
                });
            }
        }
    }
    requestParam = $.extend({}, searchParam, {
        TableName: tableName,
        IsView: isView,
        PageNumber: config.page,
        SortField: config.sortField,
        SortDirection: config.sortDir,
        IsAdvanceSearch: config.isAdvanceSearch,
        IsGlobalSearch: config.isGlobalSearch,
        SearchText: config.searchText
    });

    if (config.isGlobalSearch) {
        return {
            request: requestParam
        };
    }
    return {
        searchParams: params,
        request: requestParam
    };
}

function getSearchResults(config) {
    showLoading();
    var filterArr = getSearchFilterArr().filterArr;
    searchParams = { type: selectedSearchOption, filter: filterArr };
    var tabStrip = $("#search-tab-strip").data("kendoTabStrip");
    tabStrip.options.contentUrls[0].data = getSearchParams('vAuthor', true, config);  //Need to make them dynamic
    tabStrip.options.contentUrls[1].data = getSearchParams('vWork', true, config);
    tabStrip.options.contentUrls[2].data = getSearchParams('vElement', true, config);
    tabStrip.select(0);
    setTimeout(function () {
        tabStrip.reload("li:eq(0)");
        tabStrip.reload("li:eq(1)");
        tabStrip.reload("li:eq(2)");
    }, 500);
}

function getSearchFilterArr() {
    var filters = [];
    var searchForms = $('form');
    $.each(searchForms, function (index, value) {
        var data = objectifyForm($(this).serializeArray());
        $.each(data, function (name, value) {
            var propArr = name.split('.');
            var member = propArr[2];
            var tableName = propArr[0];
            if (name.indexOf("Filter") >= 0) {
                var operator = value; // Operator Value
                switch (operator) {
                    case 'in list':
                        var memberValue = data[`${tableName}.${member}ID.hdn`];
                        break;
                    default:
                        memberValue = data[`${tableName}.${member}`];
                        break;
                }
                if (Array.isArray(memberValue)) {
                    if (memberValue.length)
                        memberValue = memberValue.join('|');
                    else
                        memberValue = '';
                }
                if (memberValue) {
                    filters.push({
                        'field': member,
                        'operator': operator,
                        'value': memberValue,
                        'tableName': tableName,
                        'columnType': 'text'
                    });
                }
            }
            else if (propArr[1] === "Start") {
                if (member && value) {
                    filters.push({
                        'field': member,
                        'operator': "gte",
                        'value': value,
                        'tableName': tableName,
                        'columnType': 'number'
                    });
                }
            }
            else if (propArr[1] === "End") {
                if (member && value) {
                    filters.push({
                        'field': member,
                        'operator': "lte",
                        'value': value,
                        'tableName': tableName,
                        'columnType': 'number'
                    });
                }
            }
        });
    });
    var filterArr = { logic: "and", filters: [] };
    filterArr.filters = filters;
    var params = [];
    filters.forEach(function (item, index) {
        params.push({
            tableName: item.tableName,
            columnName: item.field,
            columnType: item.columnType,
            comparisonType: item.operator,
            textValue: item.value,
            andOr: 'and'
        });
    });
    return { filterArr, params };
}

function noRecordFound(ele) {
    $(ele).empty().html(`<div class="alert alert-primary" role="alert">
                          No Record to display.
                        </div>`);
}

function getFilterForSearch(value) {
    switch (typeof (value)) {
        case 'string':
            return `'${value}'`;
        default:
            return value;
    }
}

function generateGrid(response, container) {
    response.Result = JSON.parse(response.SearlizeResult);
    searchGridOptions.Total = response.Total;
    var model = generateModel(response.Result);
    var dataSource = new kendo.data.DataSource(
        {
            transport: {
                read: function (operation) {
                    var data = operation.data.data || [];
                    operation.success(data);
                    if (operation.data.endLoading && gridElement) {
                        endKendoLoading(gridElement);
                        hideLoading();
                        resizeSearchGrid();
                        resizeTabStrip();
                        //operation.success(data);
                    }
                }
            },
            pageSize: 1000,
            serverSorting: true,
            serverFiltering: true,
            serverPaging: true,
            schema: {
                model: model,
                //data: function (response) {
                //    return response;
                //},
                total: function (e) {
                    return searchGridOptions.Total;
                }
            },
            requestStart: function (e) {
                console.log(e);
                if (e.sender.attachedGrid) {
                    gridElement = e.sender.attachedGrid.element;
                    showLoading();
                    //startKendoLoading(gridElement);
                }
            }
        }
    );

    var columns = generateColumns(response.Columns);
    var gridOptions = {
        dataSource: dataSource,
        columns: columns,
        sortable: true,
        filterable: true,
        reorderable: true,
        resizable: true,
        pageable: {
            buttonCount: 7,
            messages: {
                display: "{0} - {1} of {2} rows"
            }
        },
        page: function (e) {
            var gridType = e.sender.element.attr('id').split('-')[2];
            var sortFieldArr = e.sender.dataSource.sort();
            var sortField = '', sortDir = 'asc';
            var searchText = $('#SearchQuery').val();
            searchGridOptions.Total = e.sender.dataSource.total();
            if (sortFieldArr && sortFieldArr.length) {
                sortField = sortFieldArr[0].field;
                sortDir = sortFieldArr[0].dir;
            }

            var filter = e.sender.dataSource.filter(), columnFilters;
            if (filter) {
                columnFilters = Array.prototype.concat(...filter.filters.map(x => x.filters));
            }
            var params = {
                dataSource: e.sender.dataSource, page: e.page - 1, sortField, sortDir, columnFilters, isAdvanceSearch: true,
                isGlobalSearch: searchText ? true : false,
                searchText: $('#SearchQuery').val()
            };
            populateSearch(gridType, true, params);
        },
        sort: function (e) {
            searchGridOptions.Total = e.sender.dataSource.total();
            var gridType = e.sender.element.attr('id').split('-')[2];
            var filter = e.sender.dataSource.filter(), columnFilters;
            if (filter) {
                columnFilters = Array.prototype.concat(...filter.filters.map(x => x.filters));
            }
            var searchText = $('#SearchQuery').val();
            var params = {
                dataSource: e.sender.dataSource,
                page: e.sender.pager.page() - 1,
                sortField: e.sort.field,
                sortDir: e.sort.dir,
                isGlobalSearch: searchText ? true : false,
                searchText: searchText,
                columnFilters
            };

            populateSearch(gridType, true, params);
            e.sender.dataSource.sort({
                field: e.sort.field, dir: e.sort.dir
            });
        },
        columnMenu: true,
        selectable: true,
        dataBound: function (e) {
            grid = e.sender;
            $(e.sender.element).find('.k-grid-content').height($('.k-tabstrip-wrapper').height() - 125);
            $(e.sender.element).find('td').addClass('text-nowrap');
            if (userObj)
                $('.loggedin-user').text(userObj.Name);
        },
        columnMenuInit: columnMenuInit
    };
    //setTimeout(function () {
    var searchResultGrid = $(container).kendoCustomGrid(gridOptions)
        .addClass("custom-dynamic-grid").data("kendoCustomGrid");
    searchResultGrid.dataSource.read({ data: response.Result, endLoading: true });
    searchResultGrid.thead.kendoTooltip({
        filter: ".k-header",
        content: function (e) {
            var target = e.target; // element for which the tooltip is shown
            if (!target.find('.k-link').text()) {
                tooltip.hide();
            }
            return $(target).text();
        }
    });
    resizeSearchGrid();
    resizeTabStrip();
    resizeSearchSplitter();
    searchForm.ajaxRequest = false;
    hideLoading();
    //});
}

function generateAdminGrid(response, container) {
    var treeTable = getSelectTable();
    updatedRowsList = [];
    response.Result = JSON.parse(response.SearlizeResult);
    gridColumns = response.Columns;
    var fkColumnNames = gridColumns.filter(x => x.Fktable !== null).map(x => x.ColumnName);
    adminGridOptions.Total = response.Total;
    var model = generateAdminGridModel(gridColumns);
    var columns = generateAdminGridColumns(gridColumns);
    var dataSource = new kendo.data.DataSource(
        {
            batch: true,
            transport: {
                read: function (operation) {
                    var data = operation.data.data || [];
                    data.forEach(function (item) {
                        fkColumnNames.forEach(function (fkCol) {
                            // Remove ID from fkColumn Name
                            item[fkCol.slice(0, -2)] = item[fkCol];
                            delete item[fkCol];
                        });
                    });
                    operation.success(data);
                    if (operation.data.endLoading && gridElement) {
                        endKendoLoading(gridElement);
                        if (!isAddNewItemDD)
                            hideLoading();
                    }
                },
                create: function (options) {
                    options.data.models.forEach(function (obj) {
                        $.each(obj, function (i, val) {
                            if (val === '') {
                                obj[i] = null;
                            }
                        });
                    });
                    $.ajax({
                        url: "/Grid/Add",
                        type: 'POST',
                        data: {
                            models: kendo.stringify(options.data.models),
                            columns: gridColumns,
                            tableName: treeTable
                        },
                        success: function (result) {
                            if (result.Errors) {
                                showError('Error: Unique Value Required', result);
                            }
                            else {
                                options.success(result.Data);
                            }
                            endKendoLoading(gridElement);
                            hideLoading();
                        },
                        error: function (result) {
                            // notify the data source that the request failed
                            options.error(result);
                            endKendoLoading(gridElement);
                            hideLoading();
                        },
                        complete: function (result) {
                            resizeGrid();
                            resizeSplitter();
                        }
                    });
                },
                update: function (options) {
                    options.data.models.forEach(function (obj) {
                        $.each(obj, function (i, val) {
                            if (val === '') {
                                obj[i] = null;
                            }
                        });
                    });
                    $.ajax({
                        url: "/Grid/Update",
                        type: 'POST',
                        data: {
                            models: kendo.stringify(options.data.models),
                            columns: gridColumns,
                            tableName: treeTable
                        },
                        success: function (result) {
                            if (result.Errors) {
                                showError('Error: Unique Value Required', result);
                            }
                            else {
                                options.success(result.Data);
                            }
                            endKendoLoading(gridElement);
                            hideLoading();
                        },
                        error: function (result) {
                            // notify the data source that the request failed
                            endKendoLoading(gridElement);
                            hideLoading();
                        },
                        complete: function (result) {
                            resizeGrid();
                            resizeSplitter();
                        }
                    });
                },
                destroy: function (options) {
                    $.ajax({
                        url: "/Grid/Destroy",
                        type: 'POST',
                        data: {
                            models: kendo.stringify(options.data.models),
                            tableName: treeTable,
                            primaryKey: gridColumns.find(x => x.IDColumn).ColumnName
                        },
                        success: function (result) {
                            options.success(result.Data);
                            endKendoLoading(gridElement);
                            hideLoading();
                        },
                        error: function (result) {
                            options.error(result);
                        }
                    });
                }
            },
            pageSize: 1000,
            serverSorting: true,
            serverFiltering: true,
            serverPaging: true,
            schema: {
                model: model,
                data: function (response) {
                    return response;
                },
                total: function () {
                    return adminGridOptions.Total;
                }
            },
            requestStart: function (e) {
                console.log(e);
                if (e.sender.attachedGrid) {
                    gridElement = e.sender.attachedGrid.element;
                    //startKendoLoading(gridElement);
                    showLoading();
                }
            }
        }
    );
    var gridOptions = {
        toolbar: [
            { name: "create" },
            { name: "save" },
            { name: "cancel" }
        ],
        dataSource: dataSource,
        columns: columns,
        sortable: true,
        filterable: true,
        reorderable: true,
        resizable: true,
        pageable: {
            buttonCount: 7,
            messages: {
                display: "{0} - {1} of {2} rows"
            }
        },
        page: function (e) {
            var gridType = getSelectTable(); // e.sender.element.attr('id').split('-')[2];
            var sortFieldArr = e.sender.dataSource.sort();
            var sortField = '', sortDir = 'asc';
            if (sortFieldArr && sortFieldArr.length) {
                sortField = sortFieldArr[0].field;
                sortDir = sortFieldArr[0].dir;
            }
            var filter = e.sender.dataSource.filter(), columnFilters;
            if (filter) {
                columnFilters = Array.prototype.concat(...filter.filters.map(x => x.filters));
            }
            var params = { dataSource: e.sender.dataSource, page: e.page - 1, sortField, sortDir, columnFilters };
            populateSearch(gridType, false, params);
        },
        sort: function (e) {
            var gridType = getSelectTable(); //e.sender.element.attr('id').split('-')[2];
            var columnName = e.sender.columns.find(x => x.field === e.sort.field).columnName;
            var filter = e.sender.dataSource.filter(), columnFilters;
            if (filter) {
                columnFilters = Array.prototype.concat(...filter.filters.map(x => x.filters));
            }
            var params = {
                dataSource: e.sender.dataSource,
                page: e.sender.pager.page() - 1,
                sortField: columnName,
                sortDir: e.sort.dir,
                columnFilters
            };
            populateSearch(gridType, false, params);
            e.sender.dataSource.sort({ field: columnName, dir: e.sort.dir });
        },
        columnMenu: true,
        editable: true,
        selectable: true,
        dataBound: function (e) {
            grid = e.sender;
            adminGridOnDataBound();
        },
        save: function (e) {
            var index = grid.dataItems().indexOf(e.model);
            if (e.model.dirty && !updatedRowsList.some(x => x === index)) {
                updatedRowsList.push(index);
            }
        },
        change: function (e) {
            var index = this.select().index();
            if (updatedRowsList.length > 0 && !updatedRowsList.some(x => x === index) && !enableMultiRowEditMode) {
                showMultiModeConfirmation();
            }
        },
        columnMenuInit: function (e) {
            initColumnMenuFilter.call(this, e, true);
        },
        saveChanges: function (e) {
            e.preventDefault();

            var grid = e.sender;
            var data = grid.dataSource.data();
            var validationPassed = true;
            var updatedRows = data.filter(x => x.dirty || x.id === 0);
            updatedRows.forEach(function (e, i) {
                var uid = e.uid;
                var row = grid.element.find("tr[data-uid=" + uid + "]");
                var tds = row.find("td");
                tds.each(function (i, e) {
                    grid.editCell(e);
                    if (grid.editable) {
                        var valid = grid.editable.validatable.validate();
                        if (!valid) {
                            validationPassed = false;
                            return false;
                        }
                    }
                });
            });
            if (validationPassed) {
                grid.dataSource.sync();
            }
        }
    };

    grid = $(container).kendoCustomGrid(gridOptions)
        .data("kendoCustomGrid");
    grid.dataSource.read({ data: response.Result, endLoading: true });
    grid.thead.find("[data-index=0]>.k-header-column-menu").remove();
    grid.thead.kendoTooltip({
        filter: ".k-header",
        content: function (e) {
            var target = e.target; // element for which the tooltip is shown
            if (!target.find('.k-link').text()) {
                tooltip.hide();
            }
            return $(target).text();
        }
    });
    adminGridOnDataBound();
    toggleLoginForm();
    grid.table.on('keydown', function (e) {
        if (e.keyCode === kendo.keys.TAB && ($($(e.target).closest('.k-edit-cell'))[0] || e.target.type === 'checkbox')) {
            e.preventDefault();
            var newRow = false, columns = grid.columns, columnLength = columns.length - 1;
            var currentNumberOfItems = grid.dataSource.view().length;
            var row = $(e.target).closest('tr').index();
            var col = grid.cellIndex($(e.target).closest('td'));

            if (!$(e.target).hasClass('k-dropdown') && !$(e.target).is(':checkbox')) {
                var dataItem = grid.dataItem($(e.target).closest('tr'));
                var field = columns[col].field;
                var value = $(e.target).val();
                if (!value) value = null;
                dataItem.set(field, value);
            }

            if (row >= 0 && row < currentNumberOfItems && col >= 0 && col < columnLength) {
                var nextCellRow = row;
                var nextCellCol = col;

                if (e.shiftKey) {
                    if (nextCellCol - 1 < 0) {
                        newRow = 0;
                        nextCellCol = columnLength - 1;
                        while (nextCellCol < columnLength) {
                            nextCellCol--;
                            if (columns[nextCellCol].isEditable) {
                                break;
                            }
                        }
                    } else {
                        while (nextCellCol < columnLength) {
                            nextCellCol--;
                            if (nextCellCol < 0) {
                                nextCellCol = columnLength - 1;
                                nextCellRow--;
                            }
                            if (columns[nextCellCol].isEditable) {
                                break;
                            }
                        }
                    }
                }
                else {
                    // logic to tab in next new row
                    if (nextCellCol + 1 > columnLength - 1) {
                        nextCellCol = 0;
                        newRow = true;
                        while (nextCellCol < columnLength) {
                            nextCellCol++;
                            if (columns[nextCellCol].isEditable) {
                                break;
                            }
                        }
                        nextCellRow++;
                    } else {
                        while (nextCellCol < columnLength) {
                            nextCellCol++;
                            if (columns[nextCellCol].isEditable) {
                                break;
                            }
                        }
                    }
                }

                if (nextCellCol >= columnLength || nextCellCol < 0) {
                    return;
                }

                // wait for cell to close and Grid to rebind when changes have been made
                var cell = grid.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")");
                if (columns[nextCellCol].isBoolean) {
                    var checkbox = cell.find('input');
                    $(checkbox).focus();
                }
                //else if (nextCellCol === 0) {
                //    grid.tbody.find(`tr:eq(${nextCellRow}) td:eq(0) input`).focus();
                //}
                else {
                    grid.editCell(cell);
                }

                if (updatedRowsList.length > 0 && !updatedRowsList.some(x => x === nextCellRow) && !enableMultiRowEditMode) {
                    showMultiModeConfirmation();
                }

                if (newRow) {
                    grid.content.scrollLeft(0);
                }
            }
        }
    });
}

function showError(title, result) {
    var message = "";
    $.each(result.Errors, function (key, value) {
        if ('errors' in value) {
            $.each(value.errors, function () {
                message += this + "\n";
            });
        }
    });
    showErrorDialog(title, message);
}

function showAlertWindow(title, message) {
    var alertWindow = $('#alertWindow').data('kendoWindow');
    alertWindow.title(title);
    alertWindow.content(message);
    alertWindow.refresh();
    alertWindow.center();
    alertWindow.open();
}

function showMultiModeConfirmation(hideMultiRowBtn) {
    var dialog = $('#dialog-confirm');
    if (dialog.data("kendoDialog")) {
        dialog.data("kendoDialog").close();
    }

    var actions = [
        {
            text: 'Save Changes',
            primary: true,
            action: function (e) {
                var dataItem = grid.dataItem(grid.select());
                gridSelectionIndex = grid.dataSource.data().indexOf(dataItem);
                updatedRowsList = [];
                $('.k-grid-save-changes').click();
                return true;
            }
        },
        {
            text: 'Cancel',
            action: function (e) {
                updatedRowsList = [];
                $('.k-grid-cancel-changes').click();
                return true;
            }
        }];
    if (!enableMultiRowEditMode && !hideMultiRowBtn) {
        actions.push({
            text: 'Multi-Row Edit Mode',
            action: function (e) {
                enableMultiRowEditMode = true;
                return true;
            }
        });
    }

    dialog.kendoDialog({
        width: "400px",
        title: "Confirmation",
        closable: false,
        modal: true,
        content: "<p>Do you want to save changes?<p>",
        actions: actions
    });
    dialog.data("kendoDialog").open();
}

function showErrorDialog(title, content) {
    var dialog = $('#dialog-confirm');
    if (dialog.data("kendoDialog")) {
        dialog.data("kendoDialog").close();
    }
    dialog.kendoDialog({
        width: "350px",
        title: title,
        closable: true,
        modal: true,
        content: `<p>${content}<p>`,
        actions: [
            {
                text: 'Ok',
                action: function (e) {
                    return true;
                }
            }
        ]
    });
    dialog.data("kendoDialog").open();
}

function startKendoLoading(target) {
    kendo.ui.progress(target, true);
}

function endKendoLoading(target) {
    kendo.ui.progress(target, false);
}

function populateSearch(gridType, isView, { dataSource, page, sortField = '', sortDir = 'asc', columnFilters, isGlobalSearch, isAdvanceSearch, searchText }) {
    $.post("/Grid/AdvanceSearchResult", getSearchParams(gridType, isView, { page, sortField, sortDir, columnFilters, isGlobalSearch, isAdvanceSearch, searchText }), function (response) {
        if (response) {
            var gridData = JSON.parse(response.SearlizeResult);
            searchGridOptions.Total = response.Total;
            adminGridOptions.Total = response.Total;
            dataSource.read({ data: gridData, endLoading: true });
        }
    });
}

function generateColumns(columnsDef) {
    return columnsDef.map(function (item) {
        return {
            field: item.ColumnName,
            width: item.PixelWidth,
            title: item.DisplayName
        };
    });
}

function generateAdminGridColumns(columnsDef) {
    var columns = columnsDef.map(function (item, index) {
        item.DisplayName = item.DisplayName.replace('#', '##');
        var isLastColumn = columnsDef.length - 1 === index,
            column = {
                headerTemplate: kendo.template(`# if (${item.IsRequired}) 
                                                { # ${item.DisplayName ? item.DisplayName : item.ColumnName} <strong class="text-danger">*</strong> # }
                                                else { # ${item.DisplayName ? item.DisplayName : item.ColumnName} # } #`),
                isUnique: item.IsUnique,
                isEditable: item.IsEditable,
                columnName: item.ColumnName
            };
        if (item.Fktable) {
            column = $.extend({}, column, {
                field: item.ColumnName.slice(0, -2),
                editable: function (e) {
                    if (item.IsEditable) {
                        return isGridEditable;
                    }
                    return false;
                },
                editor: function (container, options) {
                    dropDownEditor(container, options, item);
                },
                template: function (dataItem) {
                    return dataItem[item.ColumnName.slice(0, -2)] ? dataItem[item.ColumnName.slice(0, -2)] : "";
                }
            });
        }
        else if ((item.ColType === "number" || item.ColType === 'int') && item.Fktable === null && !item.IDColumn) {
            column = $.extend({}, column, {
                field: item.ColumnName,
                editor: numericEditor,
                editable: function (e) {
                    if (item.IsEditable) {
                        return isGridEditable;
                    }
                    return false;
                }
            });
        }
        else if (item.ColType === "boolean") {
            column = $.extend({}, column, {
                field: item.ColumnName,
                isBoolean: true,
                editable: function (e) {
                    return false;
                },
                template: `<input disabled="disabled" type="checkbox" #= ${item.ColumnName} ? \'checked="checked"\' : "" # title="${item.ColumnName}" 
                                class='chkbx' data-bind="checked: ${item.ColumnName}">`
            });
        }
        else if (item.ColType === 'date' || item.ColType === 'datetime') {
            column = $.extend({}, column, {
                field: item.ColumnName,
                format: "{0: yyyy-MM-dd}",
                template: function customFunction(dataItem) {
                    if (item.ColType === 'date') {
                        return dataItem[item.ColumnName] ? kendo.toString(dataItem[item.ColumnName], "yyyy-MM-dd") : "";
                    }
                    else {
                        return dataItem[item.ColumnName] ? kendo.toString(dataItem[item.ColumnName], "yyyy-MM-dd HH:mm:ss") : "";
                    }
                },
                editable: function (e) {
                    if (item.IsEditable) {
                        return isGridEditable;
                    }
                    return false;
                },
                editor: function (container, options) {
                    var input = $("<input/>");
                    input.attr("name", options.field);
                    input.appendTo(container);
                    if (item.ColType === 'date') {
                        input.kendoDatePicker({});
                    }
                    else {
                        input.kendoDateTimePicker({});
                    }
                }
            });
        }
        else {
            column = $.extend({}, column, {
                field: item.ColumnName,
                editable: function (e) {
                    if (item.IsEditable) {
                        return isGridEditable;
                    }
                    return false;
                }
            });
            if (item.ColumnName === 'Gender') {
                column.editor = genderDropDownEditor;
            }
            else if (item.ColumnName === 'Role') {
                column.editor = roleDropDownEditor;
            }
        }
        if (!isLastColumn) {
            column.width = item.PixelWidth;
        }
        if (!item.IsEditable) {
            column.attributes = {
                "class": "muted-text"
            };
        }
        return column;
    });
    var idColumn = columnsDef.find(x => x.IDColumn);
    if (idColumn) {
        columns.unshift({
            field: idColumn.ColumnName,
            menu: false,
            sortable: false,
            width: 60,
            template: `<div class='text-center'><input type='checkbox' id='#=${idColumn.ColumnName}#' class='grid-item'/></div>`,
            headerTemplate: `<div class='k-grid-delete text-center'><a role='button' class='k-button k-button-icontext' href=''>Delete</a></div>`
        });
    }
    return columns;
}

function dirtyField(data, fieldName) {
    var hasClass = $("[data-uid=" + data.uid + "]").find(".k-dirty-cell").length < 1;
    if (data.dirty && data.dirtyFields[fieldName] && hasClass) {
        return "<span class='k-dirty'></span>";
    }
    else {
        return "";
    }
}

function generateModel(response) {
    if (response && response.length) {
        var sampleDataItem = response[0];
        var model = {};
        var fields = {};
        for (var property in sampleDataItem) {
            if (property.indexOf("ID") !== -1) {
                model["id"] = property;
            }
            var propType = typeof sampleDataItem[property];

            if (propType === "number") {
                fields[property] = {
                    type: "number",
                    validation: {
                        required: true
                    }
                };
                if (model.id === property) {
                    fields[property].editable = false;
                    fields[property].validation.required = false;
                }
            } else if (propType === "boolean") {
                fields[property] = {
                    type: "boolean"
                };
            }
            else {
                fields[property] = {
                    type: "string"
                };
            }
        }
        model.fields = fields;
        return model;
    }
}
//var isDateField = [];
function generateAdminGridModel(columnDef) {
    if (columnDef && columnDef.length) {
        var model = {};
        var fields = {};
        columnDef.forEach(function (property, index) {
            var propType = property.ColType;
            if (property.IDColumn) {
                model["id"] = property.ColumnName;
                fields[property.ColumnName] = {
                    type: "number",
                    editable: false
                };
            }
            else if ((propType === 'int' || propType === 'number') && property.Fktable === null) {
                fields[property.ColumnName] = {
                    type: "number",
                    nullable: true
                };
            }
            else if (propType === "boolean") {
                fields[property.ColumnName] = {
                    type: "boolean",
                    nullable: true
                };
            }
            else if (property.Fktable !== null) {
                fields[property.ColumnName.slice(0, -2)] = {
                    type: 'object',
                    nullable: true,
                    editable: true
                };
            }
            else if (propType === 'date' || propType === 'datetime') {
                fields[property.ColumnName] = {
                    type: "date",
                    nullable: true
                };
            }
            else {
                fields[property.ColumnName] = {
                    type: "string"
                };
            }

            // validation 
            if (property.IsRequired) {
                if ((propType === 'int' || propType === 'number') && property.Fktable === null) {
                    fields[property.ColumnName].validation = {
                        required: {
                            message: `${property.ColumnName} field is required.`
                        },
                        min: 0
                    };
                }
                else if (property.Fktable !== null) {
                    fields[property.ColumnName.slice(0, -2)].validation = {
                        dropdownlistValidation: function (input) {
                            if (input.val()) {
                                return true;
                            }
                            input.attr("data-dropdownlistValidation-msg", `${property.DisplayName} field is required.`);
                            return false;
                        }
                    };
                }
                else {
                    fields[property.ColumnName].validation = {
                        required: {
                            message: `${property.ColumnName} field is required.`
                        }
                    };
                }
            }
            //else if (propType === "string" && kendo.parseDate(sampleDataItem[property])) {
            //    var parsedDate = kendo.parseDate(sampleDataItem[property]);
            //    if (parsedDate) {
            //        fields[property] = {
            //            type: "date",
            //            validation: {
            //                required: false
            //            }
            //        };
            //        isDateField[property] = true;
            //    }
            //}
        });
        model.fields = fields;
        return model;
    }
}

var selectedSearchOption = 'Author', searchParams;
$(document).on("click", '.search-Opt', function (e) {
    showLoading();
    var tabStrip = $("#search-tab-strip").data("kendoTabStrip");
    selectedSearchOption = this.value;
    var item = tabStrip.contentElement(0);
    $.ajax({
        url: "/Grid/GetSearchForm",
        type: 'Post',
        cache: false,
        data: { type: selectedSearchOption }
    }).done(function (result) {
        $(item).html(result);
        hideLoading();
    });
});

function getGridParams(e) {
    if (!searchParams) {
        var treeTable = getSelectTable();
        return {
            type: isAddNewItemDD ? columnNameAddItemDD : treeTable
        };
    }
    else {
        e.filter = searchParams.filter;
        return searchParams;
    }
}

function resizeTabStrip() {
    if ($('.k-tabstrip-wrapper').length) {
        var tabStrip = $('#search-tab-strip').data("kendoTabStrip");
        var height = $(window)[0].innerHeight;
        var wrapperHeight = height - 50;
        var headerHeight = $('.k-tabstrip-wrapper').find('ul').height();
        $('.k-tabstrip-wrapper').find('.k-content').height(wrapperHeight - headerHeight - 42);
    }
}

function resizeSearchSplitter() {
    var outerSplitter = $("#vertical").data("kendoSplitter");
    if (outerSplitter) {
        var height = $(window)[0].innerHeight;
        outerSplitter.wrapper.height(height - 50);
        outerSplitter.resize();
    }
}

function onSearchDropdownChange(e, elementToShow) {
    var elementId = '#' + elementToShow.replace('.', '_');
    if (e.sender.value() !== 'in list') {
        // show textboxes
        $(elementId).removeClass('d-none');
        var multiselect = $(elementId + 'ID').data("kendoMultiSelect");
        multiselect.wrapper.hide();
    }
    else {
        // show dropdown
        $(elementId).addClass('d-none');
        multiselect = $(elementId + 'ID').data("kendoMultiSelect");
        multiselect.wrapper.show();
    }
}

function onChangeMutliSelect(e) {
    var hiddenField = $(`#${e.sender.element.attr('id')}_hdn`);
    hiddenField.val(e.sender.dataItems().map(x => x.Option).join('|'));
    console.log(e.sender.dataItems());
}

function redirectPage(page, ele) {
    localStorage.setItem('menuItem', ele);
    switch (page) {
        case 'home':
            location.replace('/Home/Admin');
            break;
        case 'search':
            location.replace('/Home/Search');
            break;
        case 'browseAll':
            location.replace('/');
            break;
        default:
            location.replace('/');
    }
}
//End Search Form operations

function tabStripOnSelect(e) {
    //var tabStrip = $("#search-tab-strip").kendoTabStrip().data("kendoTabStrip");
    //tabStrip.activateTab(e.item);

    setTimeout(function () {
        resizeTabStrip();
        resizeSearchSplitter();
    }, 400);
}

function gridOnChange(e) {
    console.log('test');
}

function genderDropDownEditor(container, options) {
    var data = [
        { text: "", value: "" },
        { text: "Male", value: "Male" },
        { text: "Female", value: "Female" },
        { text: "Other", value: "Other" }
    ];
    $(`<select name="${options.field}" data-bind="value: ${options.field}"></select>`)
        .appendTo(container)
        .kendoDropDownList({
            autoBind: false,
            dataTextField: "text",
            dataValueField: "value",
            dataSource: data,
            valuePrimitive: true,
            value: options.model[options.field],
            change: function (e) {
                var element = e.sender.element,
                    td = element.closest('td'),
                    row = element.closest('tr');
                var dataItem = grid.dataItem(row);
                var colIdx = $("td", row).index(td);
                var colName = grid.columns[colIdx].field;
                dataItem.set(colName, e.sender.value());
            }
        });
}

function roleDropDownEditor(container, options) {
    var data = [
        { text: "", value: "" },
        { text: "Author", value: "Author" },
        { text: "Editor", value: "Editor" },
        { text: "Translator", value: "Translator" }
    ];
    $(`<select name="${options.field}" data-bind="value: ${options.field}"></select>`)
        //$('<input required name="' + options.field + '"/>')
        .appendTo(container)
        .kendoDropDownList({
            autoBind: false,
            dataTextField: "text",
            dataValueField: "value",
            dataSource: data,
            valuePrimitive: true,
            change: function (e) {
                var element = e.sender.element,
                    td = element.closest('td'),
                    row = element.closest('tr');
                var dataItem = grid.dataItem(row);
                var colIdx = $("td", row).index(td);
                var colName = grid.columns[colIdx].field;
                dataItem.set(colName, e.dataItem.value);
            }
        });
}

function dropDownOptionMapper(options, fkTableName) {
    $.ajax({
        url: "/Grid/Dropdown_OptionMapper",
        data: convertValues(options.value, fkTableName),
        success: function (data) {
            options.success(data);
        }
    });
}

function dropDownEditor(container, options, colOptions) {
    var fkTableName = colOptions.Fktable;
    var url = `/Grid/GetDropdownOptions?treeTable=${fkTableName}&optionCol=${colOptions.FkdisplayCol}`;
    $(`<select name="${options.field}" data-bind="value: ${options.field}"></select>`)
        .appendTo(container)
        .kendoDropDownList({
            dataSource: {
                type: "aspnetmvc-ajax",
                transport: {
                    read: url
                },
                schema: {
                    model: {
                        fields: {
                            Id: { type: "number" },
                            Option: { type: "string" }
                        }
                    },
                    data: function (response) {
                        return response.Data;
                    },
                    total: function (response) {
                        return response.Total;
                    }
                },
                pageSize: 80,
                serverPaging: true,
                serverFiltering: true
            },
            autoBind: false,
            dataTextField: "Option",
            dataValueField: "Option",
            valuePrimitive: true,
            filter: "contains",
            headerTemplate: $("#optionLabelTemplate").html(),
            dataBound: onDropdownDataBound,
            open: function (e) {
                onDropdownOpen(e, fkTableName, colOptions.FkdisplayCol);
            },
            virtual: {
                itemHeight: 26,
                valueMapper: function (options) {
                    $.ajax({
                        url: "/Grid/Dropdown_OptionMapper",
                        data: convertValues(options.value, fkTableName),
                        success: function (data) {
                            options.success(data);
                        }
                    });
                }
            },
            select: function (e) {
                var element = e.sender.element,
                    td = element.closest('td'),
                    row = element.closest('tr');
                var dataItem = grid.dataItem(row);
                var colIdx = $("td", row).index(td);
                var colName = grid.columns[colIdx].field;
                dataItem.set(colName, e.dataItem.Option);
                dataItem.set(colName + "ID", e.dataItem.Id);
            }
        });
    $(`<span class="k-invalid-msg" data-for="${options.field}"></span>`).appendTo(container);
}


function onFormDropdownSelect(e) {
    var element = e.sender.element;
    var colName = element.attr('name');
    if (element.attr('name').indexOf('ID') < 0) {
        colName = element.attr('name') + 'ID';
    }
    $('<input>').attr({
        type: 'hidden',
        id: colName,
        name: colName,
        value: e.dataItem.Id
    }).appendTo('#edit-form');
}

$(document).on("click", '#save-search-search-btn', function (e) {
    var searchString = $('#SearchQuery').val();
    if (searchString || getSearchFilterArr().params.length) {
        var dialog = $("#saveSearchDialog").data("kendoDialog");
        dialog.open();
    }
});

function saveSearch() {
    $('#saveSearchForm').find('.k-tooltip').remove();
    var validator = $("#saveSearchForm").data("kendoValidator");
    var searchString = $('#SearchQuery').val();
    var dialog = $("#saveSearchDialog").data("kendoDialog");
    if (validator.validate()) {
        displayLoading(dialog.element);
        $.post("/Grid/SaveSearch", {
            Name: $('#SearchName').val(),
            MemberID: userObj.MemberId,
            SearchString: searchString,
            SearchParams: !searchString ? getSearchFilterArr().params : []
        }, function (success) {
            endLoading(dialog.element);
            if (success) {
                var dropdownlist = $("#SavedSearchDD").data("kendoDropDownList");
                dropdownlist.dataSource.read();
                dropdownlist.refresh();
                $('#SearchName').val('');
                dialog.close();
            }
        });
    }
}

function onSavedSearchSelection(e) {
    if (e.dataItem.SearchString) {
        $('#SearchQuery').val(e.dataItem.SearchString);
    }
    else {
        $.get("/Grid/GetSavedSearchParams", { id: e.dataItem.SavedSearchId }, function (params) {
            console.log(params);
            params.forEach(function (item, index) {
                var form = $('#' + item.TableName);
                switch (item.ColumnType) {
                    case 'number':
                        var startEle = form.find(`#${item.TableName}_Start_${item.ColumnName}`);
                        var endEle = form.find(`#${item.TableName}_End_${item.ColumnName}`);
                        if (startEle)
                            startEle.value(item.TextValue);
                        break;
                    default:
                        var element = form.find(`#${item.TableName}_${item.ColumnName}`);
                        var filter = form.find(`#${item.TableName}_Filter_${item.ColumnName}`);
                        if (element.length) {
                            element.val(item.TextValue);
                            var dropdownList = $(filter).data("kendoDropDownList");
                            if (dropdownList)
                                dropdownList.value(item.ComparisonType);
                        }
                        break;
                }
            });
        });
    }
}

$(document).on("change", '#SearchName', function (e) {
    var value = this.value;
    if (value) {
        $.get('/Grid/ValidateSearchName', { name: value }, function (isDuplicate) {
            if (isDuplicate) {
                console.log(isDuplicate);
            }
        });
    }
});

var searchForm = {
    ajaxRequest: false
};

$(document).on("submit", '#form-common-search', function (e) {
    e.preventDefault();
    populateSearchResult();
});

$(document).on("submit", '#form-field-search', function (e) {
    e.preventDefault();
    populateSearchResult();
});

function populateSearchResult() {
    searchForm.ajaxRequest = true;
    if ($('#SearchQuery').val()) {
        getSearchResults({
            isAdvanceSearch: true,
            isGlobalSearch: true,
            searchText: $('#SearchQuery').val()
        });
    }
    else {
        getSearchResults({ isAdvanceSearch: true });
    }
    //clearSearchFormControls();
}

function clearSearchForm() {
    clearSearchFormControls();
    var tabStrip = $("#search-tab-strip").data("kendoTabStrip");
    tabStrip.options.contentUrls[0].data = getSearchParams('vAuthor', true);
    tabStrip.options.contentUrls[1].data = getSearchParams('vWork', true);
    tabStrip.options.contentUrls[2].data = getSearchParams('vElement', true);
    tabStrip.reload("li:eq(0)");
    tabStrip.reload("li:eq(1)");
    setTimeout(function () {
        tabStrip.reload("li:eq(2)");
    }, 500);

}

function clearSearchFormControls() {
    $("form").each(function (index, form) {
        $(form).trigger("reset");
        $(form).find('input[type=hidden]').val("");
        setTimeout(function () {
            $(form).find("input[data-role='dropdownlist']").each(function () {
                var item = $(this).data("kendoDropDownList");
                if (item) {
                    item.select(0);
                }
            });
            $(form).find("select[data-role='multiselect']").each(function () {
                var multiSelect = $(this).data("kendoMultiSelect");
                if (multiSelect) {
                    multiSelect.value([]);
                }
            });
        }, 50);
    });
}

$(document).on("click", '#clear-search-btn', function (e) {
    e.preventDefault();
    clearSearchForm();
});