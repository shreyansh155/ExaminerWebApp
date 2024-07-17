var stepGrid;
var grid;
$((function () {
    ko.extenders.required = function (target, overrideMessage) {
        target.hasError = ko.observable(false);
        target.validationMessage = ko.observable();
        target.validationTriggered = ko.observable(false); // Track if validation should be triggered

        function validate(newValue) {
            if (target.validationTriggered()) {
                target.hasError(!newValue);
                target.validationMessage(!newValue ? overrideMessage || "This field is required" : "");
            }
        }

        target.subscribe(validate);

        target.validate = function () {
            target.validationTriggered(true);
            validate(target());
        };

        return target;
    };
    function ApplicationTypeTemplateModel(data) {

        data = data || {};

        this.name = ko.observable(data.name || "").extend({ required: "Please enter application name" });
        this.description = ko.observable(data.description || "");
        this.instruction = ko.observable(data.instruction || "");

        this.submitTemplate = function () {
            this.name.validate();

            if (this.isValid()) {
                var formData = new FormData($("#templateForm")[0]);
                formData.append("Name", this.name());
                formData.append("Description", this.description());
                formData.append("Instruction", this.instruction());
                formData.append("Id", data.id);

                $.ajax({
                    url: '/ApplicationTypeTemplate/EditTemplate',
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        if (response.success) {
                            $("#refreshButton").trigger("click");
                            $('#add-phase').modal('hide');
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
                    }
                });
            }
        }

        this.deleteTemplate = function () {
            if (confirm("Are you sure you want to delete this template?")) {
                $.ajax({
                    url: "/ApplicationTypeTemplate/DeleteTemplate",
                    type: 'POST',
                    data: {
                        id: data.id,
                    },
                    success: function (result) {
                        $("#refreshButton").trigger("click");
                        alert("Template has been deleted successfully");
                        window.location.replace('/ApplicationTypeTemplate/Index');
                    },
                    error: function (error) {
                        console.log(error);
                        alert('error deleting details');
                    },
                });
            }
        }
        this.isValid = function () {
            return !this.name.hasError();
        };
    }

    var viewModel = new ApplicationTypeTemplateModel(window.initialData);

    ko.applyBindings(viewModel, document.getElementById("templateForm"));

    var templateId = window.initialData.id;

    //Phase Grid
    grid = $("#grid").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax({
                        url: "/ApplicationTypeTemplate/GetPhase",
                        type: "GET",
                        dataType: "json",
                        data: {
                            templateId: templateId,
                        },
                        success: function (data) {
                            options.success(data);
                        },
                        error: function (error) {
                            console.log(error);
                            alert('Error fetching data.');
                        }
                    });
                },
                parameterMap: function (data, type) {
                    if (type === "read") {
                        return kendo.stringify(data);
                    }
                    return data;
                }
            },
            pageSize: 10,
            serverPaging: true,
            schema: {
                data: function (response) {
                    return response.map(function (item) {
                        return {
                            id: item.phase.id,
                            templatePhaseId: item.templatePhaseId,
                            ordinal: item.ordinal,
                            phase: item.phase.name,
                            steps: item.stepCount,
                        };
                    });
                },
                total: function (response) {
                    return response.length;
                },
                model: {
                    fields: {
                        id: { type: "number" },
                        ordinal: { type: "number" },
                        phase: { type: "string" },
                        steps: { type: "number" },
                    },
                },
            },
        },
        detailInit: PhaseRow,
        pageable: {
            pageSize: 10,
            pageSizes: [10, 15, 20]
        },
        sortable: true,
        editable: false,
        toolbar: [
            {
                name: "create",
                text: "Add Phase",
                attributes: {
                    "class": "k-button k-primary"
                }
            },
            {
                template: kendo.template($("#custom-toolbar-template").html())
            }
        ],

        dataBound: function () {
            this.expandRow(this.tbody.find("tr.k-master-row").first());
            $('.k-grid-add').off("click");
            $('.k-grid-add').on("click", function () {
                $.ajax({
                    url: "/ApplicationTypeTemplate/OpenTemplatePhase",
                    type: 'GET',
                    data: {
                        templateId: templateId
                    },
                    success: function (result) {
                        $('#displayModal').html(result);
                        $('#add-phase').modal('show');
                    },
                    error: function (error) {
                        console.log(error);
                        alert('error fetching details');
                    },
                });
            });
        },
        columns: [
            { field: "ordinal", title: "Ordinal", width: "110px" },
            { field: "phase", title: "Phase" },
            { field: "steps", title: "Steps", width: "200px" },
            {
                command: [

                    { text: "Add Steps", click: AddStep },
                    {
                        name: "editPhase",
                        text: "",
                        iconClass: ".k-i-pencil",
                        click: EditPhase,
                        attributes: {
                            "class": "k-button k-primary"
                        },
                    },
                    {
                        name: "deletePhase",
                        text: "",
                        click: DeletePhase,
                        iconClass: ".k-i-trash",
                        attributes: {
                            "class": "k-button k-primary"
                        },
                    }
                ],
                title: "Actions",
                width: "300px",
            },
        ]
    }).data("kendoGrid");

    $("#refreshButton").on("click", function () {
        grid.dataSource.read();
    });

    //Steps Grid    
    function PhaseRow(e) {
        var dataItem = e.data;
        stepGrid = $("<div/>").appendTo(e.detailCell).kendoGrid({
            dataSource: {
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: "ApplicationTypeTemplate/GetPhaseStep",
                            type: "GET",
                            dataType: "json",
                            data: {
                                phaseId: dataItem.id,
                                templateId: templateId
                            },
                            success: function (data) {
                                options.success(data);
                            },
                            error: function (error) {
                                alert('Error fetching data.');
                            }
                        });
                    },
                },
                schema: {
                    data: function (response) {
                        return response.map(function (item) {
                            return {
                                id: item.templatePhaseStepId,
                                ordinal: item.ordinal,
                                step: item.step.name,

                            };
                        });

                    },
                    total: function (response) {
                        return response.length;
                    },
                    model: {
                        fields: {
                            id: { type: "number" },
                            ordinal: { type: "number" },
                            step: { type: "string" },
                        },
                    },
                },
                pageSize: 10,
            },
            scrollable: true,
            sortable: true,
            pageable: false,
            editable: false,
            columns: [
                { field: "id", title: "Step Id", width: "125px", hidden: true },
                { field: "ordinal", title: "Ordinal", width: "125px" },
                { field: "step", title: "Step Name" },
                {
                    command: [

                        { text: "Edit", click: EditStep },
                        { text: "Delete", click: DeleteStep }
                    ],
                    title: "Actions",
                    width: "220px",
                }
            ],
        });
    }

    // Event handlers for custom command buttons
    function AddStep(e) {
        var tr = $(e.target).closest("tr");
        var dataItem = $("#grid").data("kendoGrid").dataItem(tr);
        $.ajax({
            url: "/ApplicationTypeTemplate/AddTemplatePhaseStep",
            type: "GET",
            data: {
                templatePhaseId: dataItem.templatePhaseId,
            },
            success: function (result) {
                $('#displayModal').html(result);
                $('#add-edit-step').modal('show');
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    function EditStep(e) {
        var tr = $(e.target).closest("tr");
        var dataItem = stepGrid.data("kendoGrid").dataItem(tr);

        $.ajax({
            url: "/ApplicationTypeTemplate/EditStep",
            type: "GET",
            data: {
                id: dataItem.id, //temp phase step id
            },
            success: function (result) {
                $('#displayModal').html(result);
                $('#add-edit-step').modal('show');
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    function DeleteStep(e) {
        var tr = $(e.target).closest("tr");
        var dataItem = stepGrid.data("kendoGrid").dataItem(tr);
        if (confirm("Are you sure you want to delete this entry?")) {
            $.ajax({
                url: "/ApplicationTypeTemplate/DeleteStep",
                type: "GET",
                data: {
                    id: dataItem.id, //temp phase step id
                },
                success: function (result) {
                    if (result && result.success) {

                        $("#refreshButton").trigger("click");
                        alert("Phase has been deleted successfully");
                    }

                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    }
    function EditPhase(e) {
        var tr = $(e.target).closest("tr");
        var dataItem = $("#grid").data("kendoGrid").dataItem(tr);

        $.ajax({
            url: "/ApplicationTypeTemplate/EditPhase",
            type: "GET",
            data: {
                templatePhaseId: dataItem.templatePhaseId,
                ordinal: dataItem.ordinal,
                phaseName: dataItem.phase
            },
            success: function (result) {
                $('#displayModal').html(result);
                $('#edit-phase').modal('show');
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    function DeletePhase(e) {
        var tr = $(e.target).closest("tr");
        var dataItem = $("#grid").data("kendoGrid").dataItem(tr);

        if (confirm("Are you sure you want to delete this entry?")) {
            $.ajax({
                url: "/ApplicationTypeTemplate/DeleteTemplatePhase",
                type: 'POST',
                data: {
                    templatePhaseId: dataItem.templatePhaseId,
                },
                success: function (result) {
                    $("#refreshButton").trigger("click");
                    alert("Phase has been deleted successfully");
                },
                error: function (error) {
                    console.log(error);
                    alert('error deleting details');
                },
            });
        }
    }

    // Event handlers for custom toolbar buttons
    $(document).on("click", "#expand", function () {
        var grid = $("#grid").data("kendoGrid");
        $(".k-master-row").each(function (index) {
            grid.expandRow(this);
        });
    });

    $('#add-edit-step').on('hidden.bs.modal', function () {
        var element = $('#add-edit-step-form')[0];
        ko.cleanNode(element);
    });

    $("#back-btn").on("click", function () {
        window.location.replace("/ApplicationTypeTemplate/Index")
    })

    $(document).on("click", "#collapse", function () {
        var grid = $("#grid").data("kendoGrid");
        $(".k-master-row").each(function (index) {
            grid.collapseRow(this);
        });
    });
}))