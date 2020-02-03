// Write your Javascript code.


var isGridEditable = false, isWordWrap = false, isFormWrap = false, isLoggedIn = false,
    editfield = "", displayFieldSelectedDD = "", columnNameAddItemDD = "", tableNameAddItemDD = "", addedItemDD = {}, selectedAddItemDD = null,
    isAddNewItemDD = false, isAddNewItemRequestCompletedDD = false, formViewSelectedItem = null, currentFilterColumn, grid, selectedItemIndex,
    gridFooterEle, selectedDropdownEle, filterEnabled = false, gridSelectionIndex;
var browserWindow = $(window);
//register custom validation rules
(function ($, kendo) {
    $.extend(true, kendo.ui.validator, {
        rules: { // custom rules
            requiredvalidation: function (input, params) {
                if (input.is("[datavalidation='required']") && input.val() === "") {
                    input.attr("data-requiredvalidation-msg", "This is required field");
                    return false;
                }
                return true;
            },
            //titlevalidation: function (input, params) {
            //    if (input.is("[name='Title']") && input.val() == "") {
            //        input.attr("data-titlevalidation-msg", "Title is required field");
            //        return false;
            //    }
            //    return true;
            //},
            //namevalidation: function (input, params) {
            //    if (input.is("[name='Name']") && input.val() == "") {
            //        input.attr("data-namevalidation-msg", "Name is required field");
            //        return false;
            //    }
            //    return true;
            //}
        },
        messages: { //custom rules messages
            requiredvalidation: function (input) {
                // return the message text
                return input.attr("data-val-requiredvalidation");
            },
            //titlevalidation: function (input) {
            //    // return the message text
            //    return input.attr("data-val-titlevalidation");
            //},
            //namevalidation: function (input) {
            //    // return the message text
            //    return input.attr("data-val-namevalidation");
            //}

        }
    });
})(jQuery, kendo);

function getSelectTable() {
    var treeView = $('#treeview').data('kendoTreeView');
    if (treeView && treeView.select().length)
        return treeView.dataItem(treeView.select()).tableName;
    //else if (treeView && treeView.select().length === 0) {
    //    return "Work";
    //}
    //else if (selectedSearchOption)
    //    return selectedSearchOption;
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

$(function () {
    //jQuery.fn.scrollTo = function (elem) {
    //    $(this).scrollTop($(this).scrollTop() - $(this).offset().top + elem.offset().top);
    //    return this;
    //};
    populateDDSession('Author', 'FullName');
    populateDDSession('Category', 'Name');
    populateDDSession('City', 'Name');
    populateDDSession('Country', 'Name');
    populateDDSession('Language', 'Name');
    populateDDSession('Publisher', 'Name');
    populateDDSession('Work', 'Title');
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

function login(e) {
    $('#loginForm').find('.k-tooltip').remove();
    var validator = $("#loginForm").data("kendoValidator");
    if (validator.validate()) {
        $.post("/Home/Login", { password: $('#Password').val() }, function (success) {
            var dialog = $("#dialog").data("kendoDialog");
            if (success) {
                isLoggedIn = true;
                $('#Password').val('');
                toggleEditable();
                $('.startEditable').addClass("d-none");
                $('.stopEditable').removeClass("d-none");
                dialog.close();
            }
            else {
                showMessage(dialog.element, 'Password', 'Invalid Password.');
            }
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
    isFormWrap = false;
    resetAddNewProps();
    $("#myModal").kendoSplitter();
    var splitter = $("#myModal").data("kendoSplitter");
    var treeView = $('#treeview').data('kendoTreeView');
    if (treeView) {
        var tableName = treeView.dataItem(e.node).tableName;
        if (tableName) {
            showLoading();
            splitter.ajaxRequest(".k-pane:last", "/Grid/GetView", { type: tableName });
        }
    }
}


function loadEditData(obj) {
    var splitter = $("#myModal").data("kendoSplitter");
    if (!splitter) {
        $("#myModal").kendoSplitter();
        splitter = $("#myModal").data("kendoSplitter");
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
    splitter.ajaxRequest(".k-pane:last", "/Grid/GetView", { type: obj });
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
    hideLoading();
    setTimeout(function () {
        resizeGrid();
        resizeSplitter();
    }, 500);

    if (editfield) {
        showLoading();
        editfield = '';
        setTimeout(function () {
            $('#custom-add-btn').click();
            triggerFormView();
        }, 500);
    }

    if (getSelectTable() === tableNameAddItemDD) {
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
                setTimeout(function () {
                    //row = grid.tbody
                    //    .find("tr[data-uid='" + dataItem.uid + "']");
                    var dropdown = td.find("input[data-role='dropdownlist']");
                    var dropdownList = $(dropdown).data("kendoDropDownList");
                    dropdownList.value(addedItemDD.Id);
                    var dataItem = grid.dataItem(row);
                    dataItem[columnNameAddItemDD + 'ID'] = addedItemDD.Id;
                    dataItem[columnNameAddItemDD].Id = addedItemDD.Id;
                    dataItem[columnNameAddItemDD].Option = addedItemDD.Option;
                    dataItem.dirty = true;
                });
            }
        }
        resetAddNewProps();
    }
}

function onDataBound(e) {
    console.log("event data bound");
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
    $("#myModal").kendoSplitter();
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
            //browserWindow.resize(resizeSplitter);
            hideLoading();
        }).fail(function (xhr) {
            hideLoading();
        });

}

function onClose() {
}

function onOpen(e) {
    $('#dialog input').focus();
}
var chkEdit = false;
$(document).on("click", '.startEditable', function (e) {
    if (!isLoggedIn) {
        $('#dialog').data("kendoDialog").open();
    }
    else {
        toggleEditable();
        $('.startEditable').addClass("d-none");
        $('.stopEditable').removeClass("d-none");
        chkEdit = true;
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
    editfield = '', displayFieldSelectedDD = '';
    var dataItem = grid.dataItem(grid.select());
    gridSelectionIndex = grid.dataSource.data().indexOf(dataItem);
    $('.k-grid-save-changes').click();
    e.preventDefault();
    e.stopPropagation();
});

$(document).on("click", '#custom-cancel-btn', function (e) {
    editfield = '', displayFieldSelectedDD = '';
    $('.k-grid-cancel-changes').click();
    e.preventDefault();
    e.stopPropagation();
});

$(document).on("click", '.removeFilter', function (e) {
    clearFilter();
});

$(document).on("change", "input.chkbx", function (e) {
    var dataItem = grid.dataItem($(e.target).closest("tr"));
    $(e.target).closest("td").prepend("<span class='k-dirty'></span>");
    dataItem.IsOrganization = this.checked;
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
    var ddElement = `#${e.sender.element.attr('id')}-list`;
    setTimeout(function () {
        $(ddElement + ' .k-textbox').width('78%');
    });
}

function onDropdownDataBound(e) {
    var ddElement = `#${e.sender.element.attr('id')}-list`;
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
    columnNameAddItemDD = selectedDropdownEle.attr('id');
    tableNameAddItemDD = getSelectTable();
    selectedAddItemDD = grid.dataItem(grid.select());
    loadEditData(editfield);
});

$(document).on("click", '.dd-clear', function (e) {
    var dropdownlist = selectedDropdownEle.data("kendoDropDownList");
    dropdownlist.value(0);
    dropdownlist.close();
    var selectedProperty = dropdownlist.element.attr('id');
    var td = dropdownlist.element.closest('td'), tdIndex = td.index();
    var selectedRow = $('tr[data-uid=' + dropdownlist.element.closest('tr').attr("data-uid") + ']');
    var dataItem = grid.dataItem(selectedRow);
    dataItem[selectedProperty + 'ID'] = 0;
    dataItem[selectedProperty].Id = 0;
    dataItem[selectedProperty].Option = "";
    dataItem.dirty = true;
    kendoFastReDrawRow(grid, selectedRow);
    setTimeout(function () {
        var row = grid.tbody
            .find("tr[data-uid='" + dataItem.uid + "']");
        //var cell = dropdownlist.element.closest('td');
        var cell = row.find("td").eq(tdIndex);
        cell.removeClass('k-edit-cell').addClass("k-dirty-cell").prepend("<span class='k-dirty' />");
    });
});

function cellChangeEvent(e) {
    //debugger
    //var data = this.data();
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
    var filter = { logic: "or", filters: [] };
    var dataSource = $("#grid").data().kendoGrid.dataSource;
    dataSource.filter(filter);
    toggleFilterbutton();
}

function initColumnMenuFilter(e) {
    var menu = e.container.find(".k-menu").data("kendoMenu");
    var filterMenu = e.container.find("[data-role='filtermenu']").getKendoFilterMenu();
    var grid = e.sender;
    var field = e.field;
    var treeTable = getSelectTable();
    var helpTextElement = e.container.children(":first").children(":last");
    var filterOneValue, filterLogicValue, filterTwoValue, inputFilterOne,
        inputFilterTwo, ddFilterOne, ddFilterTwo, fieldType = 'string', fkDisplayColumn, columnFilterOptions = { listViewPageSize: 500 };

    // Get Column type
    var fieldInfo = grid.dataSource.options.schema.model.fields[field];
    if (fieldInfo) {
        fieldType = fieldInfo.type;
    }

    // Fetch FKDisplay Column from db
    $.get("/Grid/GetFKDisplayColumn", { tableName: treeTable, displayName: field }, function (response) {
        fkDisplayColumn = response;
    });

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

    //var dataSourceListViewScroll = new kendo.data.DataSource({
    //    type: "aspnetmvc-ajax",
    //    transport: {
    //        read: {
    //            type: "POST",
    //            url: "/Grid/BindFilterListView",
    //            data: {
    //                treeTable: treeTable,
    //                optionCol: field,
    //                fieldType: fieldType
    //            }
    //        }
    //    },
    //    pageSize: columnFilterOptions.listViewPageSize,
    //    serverPaging: true,
    //    serverSorting: true
    //});

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

                switch (fieldType) {
                    case 'object':
                        ele = $("<div class='k-textbox k-space-right'><input type='text' id='filter-search-" + field + "' placeholder='Search'><span class='k-icon k-i-zoom'></span></div>").insertAfter(elementToInsertAfter);
                        element = $("<div id='list-view-" + field + "' class='checkbox-ontainer kendo-grid-filter'></div>").insertAfter(ele).kendoGrid({
                            dataSource: dataSourceListView,
                            height: 135,
                            scrollable: {
                                virtual: true
                            },
                            columns: [
                                {
                                    field: '', template: "<div class='list-Item'><input type='checkbox' value='#:Id#'/>#:Option#</div>"
                                }
                            ]
                        });
                        break;
                    case 'boolean':
                        ele = $("<div class='k-textbox k-space-right'><input disabled=disabled id='filter-search-" + field + "' placeholder='Search'><span class='k-icon k-i-zoom'></span></div>").insertAfter(elementToInsertAfter);
                        element = $(`<div id='list-view-${field}' class='checkbox-ontainer'>
                                            <div class='list-Item'><input type='checkbox' value='true' class='filter-chk'/>True</div>
                                            <div class='list-Item'><input type='checkbox' value='false' class='filter-chk'/>False</div>
                                            <div class='list-Item'><input type='checkbox' value='null' class='filter-chk'/>Null</div>
                                        </div>`).insertAfter(ele);
                        break;
                    default:
                        ele = $("<div class='k-textbox k-space-right'><input type='text' id='filter-search-" + field + "' placeholder='Search'><span class='k-icon k-i-zoom'></span></div>").insertAfter(elementToInsertAfter);
                        element = $("<div id='list-view-" + field + "' class='checkbox-ontainer kendo-grid-filter'></div>").insertAfter(ele).kendoGrid({
                            dataSource: dataSourceListView,
                            height: 135,
                            resizable: true,
                            scrollable: {
                                virtual: true
                            },
                            columns: [
                                {
                                    field: '', template: "<div class='list-Item'><input type='checkbox' value='#:data#'/>#:data#</div>"
                                }
                            ]
                        });
                        break;
                }
                //$("#list-view-" + field).resizable({
                //    alsoResize: filterContainer
                //});
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

                                ddFilterOne = $(inputFirst).kendoDropDownList({
                                    dataSource: {
                                        type: "aspnetmvc-ajax",
                                        transport: {
                                            read: `/Grid/GetDropdownOptions?treeTable=${field}&optionCol=${fkDisplayColumn}`
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
                                            read: `/Grid/GetDropdownOptions?treeTable=${field}&optionCol=${fkDisplayColumn}`
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
                $(container).find("#list-view-" + field).nextAll('label').remove();

                // Filter Submit click Callback
                container.find("[type='submit']").click(function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    if (element) {
                        var filter = dataSource.filter() || { logic: "and", filters: [] };
                        var fieldFilters = [];
                        if (fieldType !== 'boolean') {
                            fieldFilters = $.map(element.find(":checkbox:checked"), function (input, index) {
                                return {
                                    field: fieldType === 'object' ? field + 'ID' : field,
                                    operator: 'eq',
                                    value: parseValue(fieldType, input.value)
                                };
                            });
                            // Get Field Filters
                            if (inputFilterOne || chkNullableFilter(filterOneValue)) {
                                fieldFilters.push({
                                    field: fieldType === 'object' ? field + 'ID' : field,
                                    operator: filterOneValue,
                                    value: parseValue(fieldType, inputFilterOne)
                                });
                            }
                            if (inputFilterTwo) {
                                fieldFilters.push({
                                    field: fieldType === 'object' ? field + 'ID' : field,
                                    operator: filterTwoValue,
                                    value: parseValue(fieldType, inputFilterTwo)
                                });
                            }
                        }
                        else {
                            fieldFilters = $.map(element.find(":checkbox:checked"), function (input, index) {
                                return {
                                    field: field,
                                    operator: 'eq',
                                    value: parseValue(fieldType, input.value)
                                };
                            });
                        }


                        if (fieldFilters.length) {
                            removeFiltersForField(filter, field);
                            filter.filters.push({
                                logic: 'or',
                                filters: fieldFilters
                            });
                            dataSource.filter(filter);
                        }
                        var popup = $(helpTextElement.children(":last").children(":first")).data("kendoPopup");
                        popup.close();
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

                    var filter = { logic: "and", filters: [] };
                    dataSource.filter(filter);
                    var gridView = $('#list-view-' + field).data('kendoGrid');
                    gridView.dataSource.filter(filter);
                });
                // Filter search text event
                $('#filter-search-' + field).on('keyup', function (e) {
                    if (fieldType === 'object') {
                        var gridView = $('#list-view-' + field).data('kendoGrid');
                        gridView.dataSource.filter({ field: "Option", operator: "contains", value: this.value });
                    }
                    else {
                        gridView = $('#list-view-' + field).data('kendoGrid');
                        gridView.dataSource.filter({ field: field, operator: "contains", value: this.value });
                    }
                    toggleFilterbutton();
                });

                var resizableAdvanceSearchEle = $('#advanced-menu-' + field).children(":first").children(":last");
                $(resizableAdvanceSearchEle).resizable();
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
                if (this.checked)
                    return { "name": this.name, "value": true };
                else {
                    return { "name": this.name, "value": false };
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
        var formDDs = $("#edit-form input[data-role='dropdownlist']");
        formDDs.each(function (index, item) {
            var value = $(item).data("kendoDropDownList").value();
            var text = $(item).data("kendoDropDownList").text();
            var ddPropName = item.name.slice(0, -2);
            if (value > 0) {
                data[ddPropName] = {
                    Id: parseInt(value),
                    Option: text
                };
            }
            else {
                data[ddPropName] = {};
            }
        });
        var treeTable = getSelectTable();
        var idField = treeTable + "ID";
        $.post('/Grid/SaveForm', { type: treeTable, selectedItem: JSON.stringify(data) })
            .done(function (id) {
                if (!data[idField]) {
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
                        grid.dataSource.add(data);
                        formViewSelectedItem = grid.dataSource.data().find(x => x[idField] === data[idField]);
                        selectedItemIndex = grid.dataSource.data().indexOf(formViewSelectedItem);
                        selectGridRow();
                        getFormView();
                    }
                }
                else {
                    $.each(formViewSelectedItem, function (name, value) {
                        if (data.hasOwnProperty(name) && data[name] !== formViewSelectedItem[name]) {
                            formViewSelectedItem.set(name, data[name]);
                        }
                    });
                    $('.k-dirty').removeClass('k-dirty');
                    hideLoading();
                }
            })
            .fail(function () {
            });
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
    var splitter = $("#myModal").data("kendoSplitter");
    splitter.ajaxRequest(".k-pane:last", "/Grid/GetView", { type: tableNameAddItemDD });
    isAddNewItemDD = false;
}

$(document).on("click", '#cancel-form-btn', function (e) {
    e.preventDefault();
    $('.k-dirty').removeClass('k-dirty');
    if (isAddNewItemDD) {
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
    $("#form-primary-key").hide();
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
var searchGridOptions = {}, gridElement;

function getSearchParams(gridType, pageNo, sortField, sortDirection) {
    if (!pageNo) pageNo = 0;
    var filters = [];
    var searchForms = $('form');
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
                        'operator': "",
                        'number1': value,
                        'tableName': tableName,
                        'columnType': 'number'
                    });
                }
            }
            else if (propArr[1] === "End") {
                if (member && value) {
                    filters.push({
                        'field': member,
                        'operator': "",
                        'number2': value,
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
            number1: item.number1,
            number2: item.number2
        });
    });
    var requestParam = $.extend({}, searchParam, {
        GridType: gridType,
        PageNumber: pageNo,
        SortField: sortField,
        SortDirection: sortDirection
    });
    //params = [{
    //    tableName: "VAuthor",
    //    columnName: "Author",
    //    columnType: "text",
    //    comparisonType: "contains",
    //    textValue: "test",
    //    number1: undefined,
    //    number2: undefined
    //}];
    return {
        searchParams: params,
        request: requestParam
    };
}

function getSearchResults() {
    //showLoading();
    var filters = [];
    var searchForms = $('form');
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
                        'operator': "",
                        'number1': value,
                        'tableName': tableName,
                        'columnType': 'number'
                    });
                }
            }
            else if (propArr[1] === "End") {
                if (member && value) {
                    filters.push({
                        'field': member,
                        'operator': "",
                        'number2': value,
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
            number1: item.number1,
            number2: item.number2
        });
    });
    searchParams = { type: selectedSearchOption, filter: filterArr };
    var tabStrip = $("#search-tab-strip").data("kendoTabStrip");
    tabStrip.options.contentUrls[3].data = getSearchParams(11);  //Need to make them dynamic
    tabStrip.options.contentUrls[4].data = getSearchParams(12);
    tabStrip.options.contentUrls[5].data = getSearchParams(13);
    tabStrip.select(3); 
    tabStrip.reload("li:eq(4)");
    tabStrip.reload("li:eq(5)");
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

var searchDataSource = new kendo.data.DataSource(
    {
        transport: {
            read: function (operation) {
                var data = operation.data.data || [];
                //if (data.length === 0 && dataSourceGridView.data().length) {
                //    data = dataSourceGridView.data();
                //}
                operation.success(data);
                //endGridLoading = operation.data.endLoading;
                //if (operation.data.endLoading && gridElement) {
                //    endKendoLoading(gridElement);
                //    operation.success(operation.data.data);
                //    endGridLoading = false;
                //}
            }
        },
        pageSize: 1000,
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        schema: {
            data: function (response) {
                return response;
            },
            total: function () {
                return searchGridOptions.Total;
            }
        }
    }
);

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
                        operation.success(data);
                    }
                }
            },
            pageSize: 1000,
            serverSorting: true,
            serverFiltering: true,
            serverPaging: true,
            schema: {
                data: function (response) {
                    return response;
                },
                total: function () {
                    return searchGridOptions.Total;
                }
            },
            requestStart: function (e) {
                console.log(e);
                if (e.sender.attachedGrid) {
                    gridElement = e.sender.attachedGrid.element;
                    startKendoLoading(gridElement);
                }
            }
        }
    );
    dataSource.options.schema.model = model;
    var columns = generateColumns(response.Columns);
    var gridOptions = {
        dataSource: dataSource,
        columns: columns,
        sortable: true,
        reorderable: true,
        resizable: true,
        pageable: true,
        page: function (e) {
            var gridType = e.sender.element.attr('id').split('-')[2];
            populateSearch(gridType, e.page - 1, e.sender.dataSource);
        },
        sort: function (e) {
            var gridType = e.sender.element.attr('id').split('-')[2];
            populateSearch(gridType, e.sender.pager.page() - 1, e.sender.dataSource, e.sort.field, e.sort.dir);
            e.sender.dataSource.sort({ field: e.sort.field, dir: e.sort.dir });
        },
        columnMenu: true,
        dataBound: function (e) {
            $(e.sender.element).find('.k-grid-content').height($('.k-tabstrip-wrapper').height() - 125);
            $(e.sender.element).find('td').addClass('text-nowrap');
        }
    };
    var searchResultGrid = $(container).kendoCustomGrid(gridOptions)
        .addClass("custom-dynamic-grid").data("kendoCustomGrid");
    searchResultGrid.dataSource.read({ data: response.Result, endLoading: true });
    resizeSearchGrid();
}

function startKendoLoading(target) {
    kendo.ui.progress(target, true);
}

function endKendoLoading(target) {
    kendo.ui.progress(target, false);
}

function populateSearch(gridType, pageNo, dataSource, sortField = '', sortDirection = 'asc') {
    $.post("/Grid/AdvanceSearchResult", getSearchParams(gridType, pageNo, sortField, sortDirection), function (response) {
        if (response) {
            var gridData = JSON.parse(response);
            dataSource.read({ data: gridData, endLoading: true });
        }
    });
}

function generateColumns(columnsDef) {
    return columnsDef.map(function (item) {
        return {
            field: item.ColumnName,
            width: item.IDColumn ? 50 : 180,
            title: item.DisplayName
        };
    });
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
            } else if (propType === "string") {
                var parsedDate = kendo.parseDate(sampleDataItem[property]);
                if (parsedDate) {
                    fields[property] = {
                        type: "date",
                        validation: {
                            required: true
                        }
                    };
                    isDateField[property] = true;
                } else {
                    fields[property] = {
                        validation: {
                            required: true
                        }
                    };
                }
            } else {
                fields[property] = {
                    validation: {
                        required: true
                    }
                };
            }
        }
        model.fields = fields;
        return model;
    }
}
var selectedSearchOption = 'Author';

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

var searchParams;
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

function clearSearchForm() {
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
    var tabStrip = $("#search-tab-strip").data("kendoTabStrip");
    tabStrip.options.contentUrls[3].data = getSearchParams(11);
    tabStrip.options.contentUrls[4].data = getSearchParams(12);
    tabStrip.options.contentUrls[5].data = getSearchParams(13);
    tabStrip.select(0);
    tabStrip.reload("li:eq(3)"); //Need to make them dynamic
    tabStrip.reload("li:eq(4)");
    tabStrip.reload("li:eq(5)");
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
            location.replace('/');
            break;
        case 'search':
            location.replace('/Home/Search');
            break;
        case 'browseAll':
            location.replace('/Home/BrowseAll');
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