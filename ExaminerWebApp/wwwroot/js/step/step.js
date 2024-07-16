$(function () {
    var phaseId = window.initialData.phaseId;

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
                optionLabel: "Select Step Type",
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
                    $.ajax({
                        url: "/Step/GetAll",
                        type: "GET",
                        dataType: "json",
                        data: { phaseId: phaseId },
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
                },
                destroy: {
                    url: function (data) {
                        return "/Step/Delete/" + data.id;
                    },
                    type: "POST",
                    dataType: "json",
                },
            },
            pageSize: 5,
            schema: {
                data: function (response) {
                    return response;
                },
                model: {
                    id: "id",
                    fields: {
                        id: { editable: false, nullable: true },
                        name: {
                            type: "string",
                            validation: {
                                required: { message: "Name is required." },
                                customValidation: function (input) {
                                    if (input.is("[name='name']") && input.val() == "") {
                                        input.attr("data-customValidation-msg", "Name is required.");
                                        return false;
                                    }
                                    return true;
                                }
                            },
                        },
                        typeId: {
                            type: "number",
                            validation: {
                                required: { message: "Step Type is required." },
                                customValidation: function (input) {
                                    if (input.is("[name='typeId']") && input.val() == 0) {
                                        input.attr("data-customValidation-msg", "Step Type is required.");
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
        },
        pageable: true,
        sortable: true,
        scrollable: true,
        editable: "inline",
        toolbar: [{ name: "create", text: "Add" }, { name: "search",text:"Search for..." }],
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
                    var errorsHtml = "<ul>";
                    if (typeof response.errors === "string") {
                        errorsHtml += "<li>" + response.errors + "</li>";
                    } else {
                        $.each(response.errors, function (key, value) {
                            errorsHtml += "<li>" + value + "</li>";
                        });
                    }
                    errorsHtml += "</ul>";
                    $("#errorBody").html(errorsHtml);
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
            PhaseId: phaseId,
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
                $("#refreshButton").trigger("click");
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

    $("#refreshButton").on("click", function () {
        grid.dataSource.read();
    });
});