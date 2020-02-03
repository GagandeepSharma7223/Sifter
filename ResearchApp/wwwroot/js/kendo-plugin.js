// MultiSelectBox, Kendo Plugin
// -----------------------------------------------------------
(function ($) {
    var kendo = window.kendo,
        ui = kendo.ui,
        MultiSelect = ui.MultiSelect,
        CHANGE = "change",
        BLUR = "blur",
        PASTE = "paste";

    var MultiSelectBox = ui.DropDownList.extend({
        init: function (element, options) {
            var me = this;
            // setup template to include a checkbox
            options.template = kendo.template(
                kendo.format('<input type="checkbox" name="{0}" value="#= {1} #" />&nbsp;<label for="{0}">#= {2} #</label>',
                    element.id + "_option_" + options.dataValueField,
                    options.dataValueField,
                    options.dataTextField
                )
            );
            // create drop down UI
            ui.DropDownList.fn.init.call(me, element, options);
            // setup change trigger when popup closes
            me.popup.bind('close', function () {
                var values = me.ul.find(":checked")
                    .map(function () { return this.value; }).toArray();
                // check for array inequality
                if (values < me.selectedIndexes || values > me.selectedIndexes) {
                    me._setText();
                    me._setValues();
                    me.trigger('change', {});
                }
            });
        },

        options: {
            name: "MultiSelectBox"
        },

        selectedIndexes: [],

        _accessor: function (vals, idx) { // for view model changes
            var me = this;
            if (vals === undefined) {
                return me.selectedIndexes;
            }
        },

        value: function (vals) {
            var me = this;
            if (vals === undefined) { // for view model changes
                return me._accessor();
            } else { // for loading from view model
                var checkboxes = me.ul.find("input[type='checkbox']");
                if (vals.length > 0) {
                    // convert to array of strings
                    var valArray = $(vals.toJSON())
                        .map(function () { return this + ''; })
                        .toArray();
                    checkboxes.each(function () {
                        this.checked = $.inArray(this.value, valArray) !== -1;
                    });
                    me._setText();
                    me._setValues();
                }
            }
        },

        _select: function (li) { }, // kills highlighting behavior
        _blur: function () { }, // kills popup-close-on-click behavior

        _setText: function () { // set text based on selections
            var me = this;
            var text = me.ul.find(":checked")
                .map(function () { return $(this).siblings("label").text(); })
                .toArray();
            me.text(text.toString().replace(/,/g, ', '));
        },
        _setValues: function () { // set selectedIndexes based on selection
            var me = this;
            var values = me.ul.find(":checked")
                .map(function () { return this.value; })
                .toArray();
            me.selectedIndexes = values;
        }

    });

    ui.plugin(MultiSelectBox);

    var ParsingMultiSelect = MultiSelect.extend({
        init: function (element, options) {
            var that = this;

            // Base call to MultiSelect to initialize ParsingMultiSelect
            MultiSelect.fn.init.call(that, element, options);

            that.input.on(PASTE, that, that._paste);
            //that.input.on(BLUR, that, that._blur);
            that.element.on("select", that._select);
            that.element.on("close", that._close);
            that.bind("dataBound", that._dataBound);
        },
        options: {
            // Assigns the name as it should appear off the kendo namespace (i.e. kendo.ui.ParsingMultiSelect).
            // The jQuery plugin would be jQuery.fn.kendoParsingMultiSelect
            name: "ParsingMultiSelect"
        },
        events: [
            PASTE,
            BLUR
        ],
        _paste: function (e) {
            var parsingMultiSelect = e.data,
                dataValueField = parsingMultiSelect.options.dataValueField,
                parsingMultiSelectDataSource = parsingMultiSelect.dataSource,
                pastedData = e.originalEvent.clipboardData.getData('text'),
                validValues = [];

            //e.sender._close();

            parsingMultiSelectDataSource.fetch().done(function () {
                var dataSourceData = parsingMultiSelectDataSource.data();
                var dataSourceValues = [];

                if (dataSourceData instanceof kendo.data.ObservableArray) {
                    dataSourceValues = dataSourceData.map(function (arrayElement, index) {
                        return arrayElement[dataValueField];
                    });
                }

                // Convert all commas to a single space then reduce all whitespace (and consecutive)
                // characters in the pasted input to a single space to get ready for splitting
                pastedData = pastedData.replace(/,/g, "").replace(/\s\s+/g, " ").trim();

                pastedValues = pastedData.split(" ");

                // Loop through the values pasted in.  If any exist in the dataSource, push them to
                // the array of valid values.
                $.each(pastedValues, function (index, value) {
                    if (dataSourceValues.indexOf(value) != -1) {
                        validValues.push(value);
                    }
                });
                parsingMultiSelect.value(e.data.value().concat(validValues));
                parsingMultiSelect._change();
                parsingMultiSelect.tagList.children().remove();
                //parsingMultiSelect.close();
            });

            e.preventDefault();
        },
        _select: function (e) {
            // code to render the tag list in a div goes here
            var that = this;
            that.trigger("select", e);
            //that.tagList.children().remove();
            //that.open();
            return MultiSelect.prototype._select.call(that, e);
        },
        _close: function (e) {
            var that = this;
            that.trigger("open", e);
            that.tagList.children().remove();
            that.open();
        },
        _dataBound: function (e) {
            var that = this;
            that.trigger("open", e);
            that.open();
        },
        _blur: function (e) {
            var multiselect = e.data;
            setTimeout(function () {
                multiselect.open();
            });
            //multiselect.toggle(true);
            e.preventDefault();
        }
    });

    // Add the ParsingMultiSelect to Kendo UI
    ui.plugin(ParsingMultiSelect);

    var CustomGrid = ui.Grid.extend({
        options: {
            name: 'CustomGrid'
        },
        _dataSource: function () {
            kendo.ui.Grid.fn._dataSource.call(this);
            this.dataSource.attachedGrid = this;
        }
    });

    ui.plugin(CustomGrid);
})(jQuery);

