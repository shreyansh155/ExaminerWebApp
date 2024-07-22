$(function () {
    var phaseId = window.initialData.id;

    var stepTypeDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "/Step/StepTypeList",
                type: "GET",
                dataType: "json",
            },
        },
    });

    function textBox(container, options) {
        $('<textarea rows="1" name="' + options.field + '" class="k-textbox form-control" data-bind="value:' + options.field + '"></textarea>')
            .appendTo(container);
    }

    function stepTypeDropDownEditor(container, options) {
        $('<input required name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: true,
                optionLabel: {
                    name: "Select Step Type",
                    id: 0
                },
                dataTextField: "name",
                dataValueField: "id",
                dataSource: stepTypeDataSource,
                value: options.model[options.field],
            });
    }


    function kendoEditor(container, options) {
        $('<textarea name="' + options.field + '"></textarea>')
            .appendTo(container)
            .kendoEditor({
                tools: [
                    "bold", "italic", "underline", "strikethrough",
                    "justifyLeft", "justifyCenter", "justifyRight", "justifyFull",
                    "insertUnorderedList", "insertOrderedList", "indent", "outdent",
                    "createLink", "unlink", "insertImage", "insertFile",
                    "subscript", "superscript", "createTable",
                    "addRowAbove", "addRowBelow", "addColumnLeft", "addColumnRight",
                    "deleteRow", "deleteColumn", "viewHtml"
                ],
            });
    }

    var grid = $("#stepGrid").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    var gridData = {
                        Skip: options.data.skip,
                        Take: options.data.pageSize || 10,
                        Page: options.data.page || 1,
                        PageSize: options.data.pageSize || 10,
                        Sort: options.data.sort,
                        Filter: options.data.filter
                    };
                    var pager = JSON.stringify(gridData);
                    $.ajax({
                        url: "/Step/GetAll?phaseId=" + phaseId,
                        type: "POST",
                        contentType: "application/json",
                        dataType: "json",
                        data: pager,
                        success: function (data) {
                            options.success(data);
                        },
                        error: function (error) {
                            console.error(error);
                            alert("Error fetching data.");
                        },
                    });
                },
                update: {
                    url: function (data) {
                        return "/Step/Update/" + data.id;
                    },
                    type: "POST",
                    dataType: "json",
                    complete: function (e) {
                        if (e.responseJSON.success) {
                            dataSource.read();
                        } else {
                            displayErrors(e.responseJSON.errors);
                        }
                    }
                },

                destroy: {
                    url: function (data) {
                        return "/Step/Delete/" + data.id;
                    },
                    type: "POST",
                    dataType: "json",
                },
            },
            page: 1,
            pageSize: 10,
            pageable: {
                pageSize: 10,
                pageSizes: [10, 15, 20]
            },
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            schema: {
                total: "totalCount",
                data: "items",
                model: {
                    id: "id",
                    fields: {
                        id: { editable: false, nullable: true },
                        typeId: {
                            type: "number",
                            validation: {
                                required: { message: "Step Type is required." },
                                typeIdValidation: function (input) {
                                    if (input.is("[name='typeId']") && input.val() == 0) {
                                        input.attr("data-typeIdValidation-msg", "Step Type is required.");
                                        return false;
                                    }
                                    return true;
                                }
                            }
                        },
                        name: {
                            type: "string",
                            validation: {
                                required: { message: "Name is required." },
                                nameValidation: function (input) {
                                    if (input.is("[name='name']") && input.val() == "") {
                                        input.attr("data-nameValidation-msg", "Name is required.");
                                        return false;
                                    }
                                    return true;
                                }
                            },
                        },
                        description: { type: "string" },
                        instruction: { type: "string" },
                        stepType: { type: "string" },
                    },
                },
            },
            filter: function (e) {
                console.log("Filter applied: ", e.filter);
            }
        },
        pageable: {
            pageSize: 5,
            pageSizes: [5, 10, 15]
        },
        sortable: true,
        scrollable: true,
        editable: "inline",
        toolbar: [
            {
                name: "create",
                text: "Add",
            },
        ],
        columns: [
            { field: "id", title: "Step Id", width: "150px", hidden: true },
            { field: "name", title: "Name", width: "150px" },
            { field: "description", title: "Description", editor: textBox },
            {
                field: "instruction", title: "Instruction", editor: kendoEditor,
                template: function (dataItem) {
                    return (
                        '<div class="instruction-cell" data-instruction="' +
                        kendo.htmlEncode(dataItem.instruction) +
                        '">' +
                        dataItem.instruction +
                        "</div>"
                    );
                },
            },
            { field: "typeId", title: "Step Type", width: "200px", editor: stepTypeDropDownEditor, template: "#= stepType #" },
            { command: ["edit", "destroy"], title: "Actions", width: "220px" },
        ],
        save: function (e) {

            if (e.model.isNew()) {
                e.preventDefault();
                CreateStep(e.model);
            } else {
                e.preventDefault();
                EditStep(e.model);
            }
        },
        remove: function (e) {
            DeleteStep(e.model);
        },
        edit: function (e) {
            if (e.container.find("textarea[name='instruction']").length > 0) {
                e.container.find("textarea[name='instruction']").focus(function () {
                    var textarea = $(this);
                    textarea.kendoEditor({
                        tools: [
                            "bold", "italic", "underline", "strikethrough",
                            "justifyLeft", "justifyCenter", "justifyRight", "justifyFull",
                            "insertUnorderedList", "insertOrderedList", "indent", "outdent",
                            "createLink", "unlink", "insertImage", "insertFile",
                            "subscript", "superscript", "createTable",
                            "addRowAbove", "addRowBelow", "addColumnLeft", "addColumnRight",
                            "deleteRow", "deleteColumn", "viewHtml"
                        ],
                    });
                });
            }
        },
        dataBound: function () {
            $(".k-grid-add").off("click").on("click", function () {
                grid.addRow();
            });
        },
    }).data("kendoGrid");

    $("#searchBox").on("input", function () {
        $("#grid").data("kendoGrid").dataSource.read();
    });

    function CreateStep(data) {

        var formData = {
            PhaseId: phaseId,
            Name: data.name,
            Description: data.description,
            Instruction: data.instruction,
            TypeId: data.typeId,
        };
        $.ajax({
            url: "/Step/CreateStep",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    $("#refreshButton").trigger("click");
                } else {
                    displayErrors(response.errors);
                }
            },
            error: function (error) {
                console.log(error);
                alert("Error fetching data.");
            },
        });
    }

    function EditStep(data) {
        var formData = {
            PhaseId: data.phaseId,
            Name: data.name,
            Description: data.description,
            Instruction: data.instruction,
            TypeId: data.typeId,
            Id: data.id,
        };
        $.ajax({
            url: "/Step/EditStep",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    $("#refreshButton").trigger("click");
                } else {
                    displayErrors(response.errors);
                }
            },
            error: function (error) {
                console.log(error);
                alert("Error fetching data.");
            },
        });
    }

    function DeleteStep(data) {
        $.ajax({
            url: "/Step/Delete",
            type: "POST",
            data: { id: data.id },
            success: function (result) {
                $("#refreshButton").trigger("click");
                alert("Step has been deleted successfully");
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    function displayErrors(errors) {
        var errorsHtml = "<ul>";
        if (typeof errors === "string") {
            errorsHtml += "<li>" + errors + "</li>";
        } else {
            $.each(errors, function (key, value) {
                errorsHtml += "<li>" + value + "</li>";
            });
        }
        errorsHtml += "</ul>";
        $("#errorBody").html(errorsHtml);
    }

    $("#refreshButton").on("click", function () {
        grid.dataSource.read();
        $("#errorBody").empty();
    });

    $("#search").on("input", function () {
        var value = $(this).val();
        grid.dataSource.filter({
            logic: "or",
            filters: [
                { field: "name", operator: "contains", value: value },
                { field: "description", operator: "contains", value: value },
                { field: "instruction", operator: "contains", value: value }
            ]
        });
    });
});