$(function () {
    var phaseId = window.initialData.phaseId;

    var stepTypeDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "/Step/StepTypeList",
                type: "GET",
                dataType: "json"
            }
        }
    });

    function textBox(container, options) {
        $('<textarea rows="1" name="' + options.field + '" class="k-textbox form-control" data-bind="value:' + options.field + '"></textarea>')
            .appendTo(container);
    }

    function stepTypeDropDownEditor(container, options) {
        $('<input required name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                optionLabel: "Select Step Type",
                dataTextField: "name",
                dataValueField: "id",
                dataSource: stepTypeDataSource,
                value: options.model[options.field]
            });
    }

    function kendoEditor(container, options) {
        $('<textarea name="' + options.field + '"></textarea>')
            .appendTo(container)
            .kendoEditor({
                tools: [
                    "bold",
                    "italic",
                    "underline",
                    "strikethrough",
                    "justifyLeft",
                    "justifyCenter",
                    "justifyRight",
                    "justifyFull",
                    "insertUnorderedList",
                    "insertOrderedList",
                    "indent",
                    "outdent",
                    "createLink",
                    "unlink",
                    "insertImage",
                    "insertFile",
                    "subscript",
                    "superscript",
                    "createTable",
                    "addRowAbove",
                    "addRowBelow",
                    "addColumnLeft",
                    "addColumnRight",
                    "deleteRow",
                    "deleteColumn",
                    "viewHtml"
                ]
            });
    }

    var grid = $("#stepGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: "/Step/GetAll",
                    type: "GET",
                    dataType: "json",
                    data: {
                        phaseId: phaseId
                    }
                },
                update: {
                    url: function (data) {
                        return "/Step/Update/" + data.id;
                    },
                    type: "POST",
                    dataType: "json"
                },
                destroy: {
                    url: function (data) {
                        return "/Step/Delete/" + data.id;
                    },
                    type: "POST",
                    dataType: "json"
                },
            },
            pageSize: 10,
            schema: {
                model: {
                    id: "id",
                    fields: {
                        id: { editable: false, nullable: true },
                        name: {
                            type: "string",
                            validation: {
                                required: true,
                                nameValidation: function (input) {
                                    if (input.is("[name='name']") && input.val() == "") {
                                        input.attr("data-nameValidation-msg", "Name is required.");
                                        return false;
                                    }
                                    return true;
                                }
                            }
                        },
                        description: { type: "string" },
                        instruction: { type: "string" },
                        typeId: {
                            type: "number",
                            validation: {
                                required: true,
                                typeIdValidation: function (input) {
                                    if (input.is("[name='typeId']") && input.val() == "") {
                                        input.attr("data-typeIdValidation-msg", "Step Type is required.");
                                        return false;
                                    }
                                    return true;
                                }
                            }
                        },
                        stepType: { type: "string" }
                    }
                }
            }
        },
        width: "1200px",
        margin: "50px",
        pageable: true,
        sortable: false,
        editable: "inline",
        toolbar: [{ name: "create", text: "Add Steps" }],
        columns: [
            { field: "id", title: "Step Id", width: "150px", hidden: true },
            { field: "name", title: "Name", width: "150px" },
            { field: "description", title: "Description", editor: textBox },
            {
                field: "instruction", title: "Instruction", editor: kendoEditor,
                template: function (dataItem) {
                    return '<div class="instruction-cell" data-instruction="' + kendo.htmlEncode(dataItem.instruction) + '">' + dataItem.instruction + '</div>';
                }
            },
            {
                field: "typeId",
                title: "Step Type",
                width: "200px",
                editor: stepTypeDropDownEditor,
                template: "#= stepType #",
            },
            {
                command: ["edit", "destroy"],
                title: "Actions",
                width: "220px"
            },
        ],
        save: function (e) {
            if (e.model.isNew()) {
                e.preventDefault();
                CreateStep(e.model);
                console.log("Creating new record");
            } else {
                EditStep(e.model);
                console.log("Updating record with id: " + e.model.id);
            }
        },

        dataBound: function () {
            $('.k-grid-add').off("click").on("click", function () {
                grid.addRow();
            });
            $("#grid").on("click", ".k-grid-myDelete", function (e) {
                e.preventDefault();

                var command = $(this);
                var cell = command.closest("td");

            });
        }
    }).data("kendoGrid");

    $("#grid").kendoValidator({
        rules: {
            typeIdValidation: function (input) {
                if (input.is("[name='typeId']")) {
                    return input.val() != "";
                }
                return true;
            },
            nameValidation: function (input) {
                if (input.is("[name='name']")) {
                    return input.val() != "";
                }
                return true;
            }
        },
        messages: {
            typeIdValidation: "Step Type is required.",
            nameValidation: "Name is required."
        }
    });

    function CreateStep(data) {
        var validator = $("#grid").data("kendoValidator");
        if (!validator.validate()) {
            return;
        }
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
                    kendoWindow.close();
                } else {
                    var errorsHtml = '<ul>';
                    if (typeof (response.errors) === "string") {
                        errorsHtml += '<li>' + response.errors + '</li>'
                    }
                    else {
                        $.each(response.errors, function (key, value) {
                            errorsHtml += '<li>' + value + '</li>';  
                        });
                    }
                    errorsHtml += '</ul>';
                    $('#errorBody').html(errorsHtml);
                }
            },
            error: function (error) {
                console.log(error);
                alert('Error fetching data.');
            }
        });
    }

    function EditStep(data) {
        var formData = {
            PhaseId: phaseId,
            Name: data.name,
            Description: data.description,
            Instruction: data.instruction,
            TypeId: data.typeId,
            Id: data.id
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
                alert('Error fetching data.');
            }
        });
    }

    $("#refreshButton").on("click", function () {
        grid.dataSource.read();
    });

    $("#stepGrid").kendoTooltip({
        filter: ".instruction-cell",
        content: function (e) {
            var dataItem = $("#stepGrid").data("kendoGrid").dataItem(e.target.closest("tr"));
            $("#tooltipEditor").val(dataItem.instruction);
            $("#tooltipEditor").kendoEditor({
                tools: [
                    "bold",
                    "italic",
                    "underline",
                    "strikethrough",
                    "justifyLeft",
                    "justifyCenter",
                    "justifyRight",
                    "justifyFull",
                    "insertUnorderedList",
                    "insertOrderedList",
                    "indent",
                    "outdent",
                    "createLink",
                    "unlink",
                    "insertImage",
                    "insertFile",
                    "subscript",
                    "superscript",
                    "createTable",
                    "addRowAbove",
                    "addRowBelow",
                    "addColumnLeft",
                    "addColumnRight",
                    "deleteRow",
                    "deleteColumn",
                    "viewHtml"
                ]
            });
            return $("#tooltipTemplate").html();
        },
        width: 300,
        position: "top"
    });
});