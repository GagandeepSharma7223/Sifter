// Write your Javascript code.


var isGridEditable = false, isWordWrap = false, isFormWrap = false, isLoggedIn = false,
    editfield = "", formViewSelectedItem = null, currentFilterColumn, grid, selectedItemIndex, gridFooterEle;
var browserWindow = $(window);
//register custom validation rules
(function ($, kendo) {
    $.extend(true, kendo.ui.validator, {
        rules: { // custom rules
            titlevalidation: function (input, params) {
                if (input.is("[name='Title']") && input.val() == "") {
                    input.attr("data-titlevalidation-msg", "Title is required field");
                    return false;
                }
                return true;
            },
            namevalidation: function (input, params) {
                if (input.is("[name='Name']") && input.val() == "") {
                    input.attr("data-namevalidation-msg", "Name is required field");
                    return false;
                }
                return true;
            }
        },
        messages: { //custom rules messages
            titlevalidation: function (input) {
                // return the message text
                return input.attr("data-val-titlevalidation");
            },
            namevalidation: function (input) {
                // return the message text
                return input.attr("data-val-namevalidation");
            }

        }
    });
})(jQuery, kendo);

function getSelectTable() {
    var treeView = $('#treeview').data('kendoTreeView');
    return treeView.dataItem(treeView.select()).text;
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
    outerSplitter.wrapper.height($('.grid-container').height());
    outerSplitter.resize();
    //outerSplitter.trigger("resize");
}

function resizeGrid() {
    grid.resize();
    var height = $(window)[0].innerHeight;
    $('.grid-container').height(height - 50);
    $('#grid .k-grid-content').height(height - 120);
}

$(window).resize(function () {
    resizeGrid();
});

$(function () {
    jQuery.fn.scrollTo = function (elem) {
        $(this).scrollTop($(this).scrollTop() - $(this).offset().top + elem.offset().top);
        return this;
    };
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
    //var grid = $("#grid").data("kendoGrid");
    if (isGridEditable) {
        $('.chkbx').removeAttr('disabled');
        grid.showColumn(grid.columns[0]);
    }
    else {
        $('.chkbx').attr('disabled', 'disabled');
        grid.hideColumn(grid.columns[0]);
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
    showLoading();
    isFormWrap = false;
    $("#myModal").kendoSplitter();
    var splitter = $("#myModal").data("kendoSplitter");
    var view = $(e.node).attr("param");
    splitter.ajaxRequest(".k-pane:last", "/Grid/GetView", { type: view.charAt(0).toUpperCase() + view.slice(1) });
}


function loadEditData(obj) {
    showLoading();
    var splitter = $("#myModal").data("kendoSplitter");
    var treeview = $("#treeview").data("kendoTreeView");
    var bar = treeview.findByText(obj);
    treeview.select(bar);
    splitter.ajaxRequest(".k-pane:last", "/Grid/GetView", { type: obj });
}

function gridDataBoundConfig() {
    customActions();
    toggleEdit();
    if (isWordWrap) {
        $('.k-grid td').removeClass('text-nowrap').addClass('text-wrap');
    }
    else {
        $('.k-grid td').removeClass('text-wrap').addClass('text-nowrap');
    }
    hideLoading();
    resizeGrid();
    resizeSplitter();
    browserWindow.resize(resizeSplitter);

    if (editfield) {
        editfield = '';
        setTimeout(function () {
            $('#custom-add-btn').click();
        }, 500);
    }
}

function onDataBound(e) {
    console.log("event data bound");
    grid = $("#grid").data("kendoGrid");
    e.sender.select("tr:eq(0)");
    $('.k-grid-content').scrollTop(0);
    gridDataBoundConfig();

    if (isFormWrap) {
        formSwitchEvents();
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
    }
    else {
        $('.stopEditable').addClass("d-none");
        $('.startEditable').removeClass("d-none");
        $('.chkbx').attr('disabled', 'disabled');
        grid.hideColumn(grid.columns[0]);
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
                isFormWrap = !isFormWrap;
                if (isFormWrap) {
                    formViewSelectedItem = grid.dataItem(grid.select());
                    selectedItemIndex = grid.dataSource.data().indexOf(formViewSelectedItem);
                    $("#myModal").kendoSplitter();
                    gridFooterEle = $('.k-pager-wrap').clone(true);
                    getFormView();
                }
            }
        });
    }
}

function getFormView() {
    showLoading();
    var treeTable = getSelectTable();
    var data = {
        type: treeTable,
        selectedItem: JSON.stringify(formViewSelectedItem, (k, v) => v === undefined ? null : v)
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
            browserWindow.resize(resizeSplitter);
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
    $('.k-grid-save-changes').click();
    e.preventDefault();
    e.stopPropagation();
});

$(document).on("click", '#custom-cancel-btn', function (e) {
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
    if (e.dataItem.Option == 'Add New' && !editfield) {
        editfield = field;
        loadEditData(editfield);
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
}

function initColumnMenuFilter(e) {
    var menu = e.container.find(".k-menu").data("kendoMenu");
    var filterMenu = e.container.find("[data-role='filtermenu']").getKendoFilterMenu();
    var grid = e.sender;
    var field = e.field;

    var helpTextElement = e.container.children(":first").children(":last");
    var fieldType = 'string';
    var filterOneValue, filterLogicValue, filterTwoValue, inputFilterOne, inputFilterTwo, ddFilterOne, ddFilterTwo;

    // Get Column type
    var fieldInfo = grid.dataSource.options.schema.model.fields[field];
    if (fieldInfo) {
        fieldType = fieldInfo.type;
    }
    var dataSource = this.dataSource;
    var treeTable = getSelectTable();
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
        pageSize: 150,
        serverPaging: true
    });

    var dataSourceListViewScroll = new kendo.data.DataSource({
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
        pageSize: 150,
        serverPaging: true,
        serverSorting: true
    });
    var pageModel = {};
    pageModel[field] = 1;

    var columnDataSource = new kendo.data.DataSource({
        data: uniqueForField(dataSource.data(), field, fieldType),
        sort: { field: field, dir: "asc" }
    });

    menu.bind("open", function (e) {
        if ($(e.item).find('.k-link').first().text() == "Filter") {
            var filterContainer = helpTextElement.children(":last");
            var parentEle = filterContainer.children(":first");
            var elementToInsertAfter = parentEle.find('.k-filter-help-text');
            var container = parentEle.find('form');

            if (!dataSource.filter()) {
                var gridDataSource = $("#grid").data().kendoGrid.dataSource;
                columnDataSource.data(uniqueForField(gridDataSource.data(), field, fieldType));
                columnDataSource.sort({ field: field, dir: "asc" });
            }

            if (parentEle.find('#list-view-' + field).length === 0) {
                $(helpTextElement.children(":last")).resizable({
                    alsoResize: "#list-view-" + field
                });
                elementToInsertAfter.addClass('d-none');  //hide text above search box
                var advancedOption = $(container).clone();
                var element, ele;

                switch (fieldType) {
                    case 'object':
                        ele = $("<div class='k-textbox k-space-right'><input id='filter-search-" + field + "' placeholder='Search'><span class='k-icon k-i-zoom'></span></div>").insertAfter(elementToInsertAfter);
                        element = $("<div id='list-view-" + field + "' class='checkbox-ontainer'></div>").insertAfter(ele).kendoListView({
                            dataSource: dataSourceListView,
                            height: 135,
                            template: "<div class='list-Item'><input type='checkbox' value='#:Id#'/>#:Option#</div>"
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
                        ele = $("<div class='k-textbox k-space-right'><input id='filter-search-" + field + "' placeholder='Search'><span class='k-icon k-i-zoom'></span></div>").insertAfter(elementToInsertAfter);
                        element = $("<div id='list-view-" + field + "' class='checkbox-ontainer'></div>").insertAfter(ele).kendoListView({
                            dataSource: dataSourceListView,
                            height: 135,
                            template: "<div class='list-Item'><input type='checkbox' value='#:data#'/>#:data#</div>"
                        });
                        break;
                }
                $("#list-view-" + field).resizable({
                    alsoResize: filterContainer
                });
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
                    advancedMenu.bind("open", function (e) {
                        if (!inputFilterOne && !inputFilterTwo) {
                            var advancedOptionFrm = $('#advanced-menu-' + field).find('.k-filter-menu-container');
                            if (fieldType === 'object') {
                                var inputFirst = advancedOptionFrm.find("input:eq(0)");
                                var inputSecond = advancedOptionFrm.find("input:eq(1)");
                                inputFirst.removeClass('k-textbox').removeAttr("data-bind");
                                inputSecond.removeClass('k-textbox').removeAttr("data-bind");

                                ddFilterOne = $(inputFirst).kendoDropDownList({
                                    dataSource: dataSourceListView,
                                    dataTextField: "Option",
                                    dataValueField: "Id",
                                    change: function (e) {
                                        inputFilterOne = this.value();
                                    },
                                    height: 135,
                                }).data("kendoDropDownList");

                                ddFilterOne.setOptions({ optionLabel: "Select" });
                                ddFilterOne.refresh();
                                ddFilterOne.select(0);

                                ddFilterTwo = $(inputSecond).kendoDropDownList({
                                    dataSource: dataSourceListView,
                                    dataTextField: "Option",
                                    dataValueField: "Id",
                                    change: function (e) {
                                        inputFilterTwo = this.value();
                                    },
                                    height: 135,
                                }).data("kendoDropDownList");

                                ddFilterTwo.setOptions({ optionLabel: "Select" });
                                ddFilterTwo.refresh();
                                ddFilterTwo.select(0);

                                var firstValueDropDown = advancedOptionFrm.find("select:eq(0)");
                                var logicDropDown = advancedOptionFrm.find("select:eq(1)");
                                var secondValueDropDown = advancedOptionFrm.find("select:eq(2)");
                                logicDropDown.kendoDropDownList();
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

                container.find("[type='submit']").click(function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    if (element) {
                        var filter = dataSource.filter() || { logic: "and", filters: [] };
                        var fieldFilters = [];
                        if (fieldType != 'boolean') {
                            fieldFilters = $.map(element.find(":checkbox:checked"), function (input, index) {
                                return {
                                    field: fieldType == 'object' ? field : field,
                                    operator: 'eq',
                                    value: parseValue(fieldType, input.value)
                                };
                            });
                            // Get Field Filters
                            if (inputFilterOne || chkNullableFilter(filterOneValue)) {
                                fieldFilters.push({
                                    field: fieldType == 'object' ? field : field,
                                    operator: filterOneValue,
                                    value: parseValue(fieldType, inputFilterOne)
                                });
                            }
                            if (inputFilterTwo) {
                                fieldFilters.push({
                                    field: fieldType == 'object' ? field : field,
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
                    }
                });

                $('#filter-search-' + field).on('keyup', function (e) {
                    if (this.value) {
                        if (fieldType === 'object') {
                            var listView = $('#list-view-' + field).data('kendoListView');
                            listView.dataSource.filter({ field: "Option", operator: "contains", value: this.value });
                        }
                        else {
                            listView = $('#list-view-' + field).data('kendoListView');
                            listView.dataSource.filter({ operator: "contains", value: this.value });
                        }
                    }
                });

                var resizableAdvanceSearchEle = $('#advanced-menu-' + field).children(":first").children(":last");
                $(resizableAdvanceSearchEle).resizable();
            }


            var processScroll = true;


            $('#list-view-' + field).on('scroll', function (e) {
                if (processScroll && $(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight - 5) {
                    processScroll = false;
                    pageModel[field]++;
                    var listView = $('#list-view-' + field).data('kendoListView');
                    dataSourceListViewScroll.query({
                        page: pageModel[field],
                        pageSize: 150,
                        sort: { field: field, dir: "asc" }
                    }).then(function () {
                        if (dataSourceListViewScroll.data().length) {
                            dataSourceListViewScroll.data().forEach(x => {
                                listView.dataSource.data().push(x);
                            });
                            listView.dataSource.sync();
                            listView.refresh();
                            processScroll = true;
                        }
                        else {
                            processScroll = false;
                        }

                    });
                }
            });

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
                data[idField] = id;
                //var item = $.extend({}, data, { Author: {}, Language: {}, City: {}, Publisher: {}, Editor: {}, Translator: {} });
                grid.dataSource.add(data);
                formViewSelectedItem = grid.dataSource.data().find(x => x[idField] === data[idField]);
                selectedItemIndex = grid.dataSource.data().indexOf(formViewSelectedItem);
                selectGridRow();
                getFormView();
            }
            else {
                $.each(formViewSelectedItem, function (name, value) {
                    if (data.hasOwnProperty(name) && data[name] !== formViewSelectedItem[name]) {
                        formViewSelectedItem.set(name, data[name]);
                    }
                });
                $('.k-dirty').remove();
                hideLoading();
            }
        })
        .fail(function () {
        });
});

$(document).on("click", '#cancel-form-btn', function (e) {
    e.preventDefault();
    toggleFormView();
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
});

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
    }
}

function loadFormData() {
    var splitter = $("#myModal").data("kendoSplitter");
    var treeTable = getSelectTable();
    splitter.ajaxRequest(".k-pane:last", "/Grid/GetFormView", { type: treeTable, selectedItem: JSON.stringify(formViewSelectedItem) });
}

function selectGridRow() {
    grid.select(`tr:eq(${selectedItemIndex})`);
    grid.element.find(".k-grid-content").animate({
        scrollTop: grid.select().offset().top
    }, 400);
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
            $(formName + ' input:checkbox').attr('disabled', 'disabled');;
        }
        else {
            $(formName + " :input").removeAttr('disabled');
            $(formName + ' input:checkbox').removeAttr('checked');
        }
    }
}

function displayLoading(target) {
    var element = $(target);
    kendo.ui.progress(element, true);
}

function endLoading(target) {
    var element = $(target);
    kendo.ui.progress(element, false);
}

// End Form View Operations
