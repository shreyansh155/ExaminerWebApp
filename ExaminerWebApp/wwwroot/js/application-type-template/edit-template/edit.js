var kendoWindow;
$((function () {

    kendoWindow = $("#window").kendoWindow({
        width: "600px",
        title: "Add Phase",
        visible: false,
        modal: true
    }).data("kendoWindow");
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
                            kendoWindow.close();
                        } else {
                            var errorsHtml = '<ul>';
                            if (typeof (response.errors) === "string") {
                                errorsHtml += '<li>' + response.errors + '</li>'
                            }
                            else {
                                $.each(response.errors, function (key, value) {
                                    errorsHtml += '<li>' + value + '</li>'; // Create list of errors
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
        this.isValid = function () {
            return !this.name.hasError();
        };
    }
    var viewModel = new ApplicationTypeTemplateModel(window.initialData);
    ko.applyBindings(viewModel, document.getElementById("templateForm"));

    var templateId = window.initialData.id;

    //Phase Grid
    var grid = $("#grid").kendoGrid({
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
                name: "expand",
                text: "Expand All",
                attributes: {
                    "class": "k-button k-primary"
                }
            },
            {
                name: "collapse",
                text: "Collapse All",
                attributes: {
                    "class": "k-button k-primary"
                }
            },

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
                        kendoWindow.content(result);
                        kendoWindow.center().open();
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
            { field: "steps", title: "Steps",width:"110px" }
        ]
    }).data("kendoGrid");
    $("#refreshButton").on("click", function () {
        grid.dataSource.read();
    });

    //Steps Grid    
    function PhaseRow(e) {
        var dataItem = e.data;

        $("<div/>").appendTo(e.detailCell).kendoGrid({
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
            sortable: false,
            pageable: false,
            editable: false,
            columns: [
                { field: "id", title: "Step Id", width: "125px", hidden: true },
                { field: "ordinal", title: "Ordinal", width: "125px" },
                { field: "step", title: "Step Name" },
                {
                    command: [

                        { text: "Edit" },
                        { text: "Delete" }
                    ],
                    title: "Actions",
                    width: "220px",
                }
            ],
        });
    }
    // Event handlers for custom toolbar buttons
    $(document).on("click", "#expand", function () {
        var grid = $("#grid").data("kendoGrid");
        console.log("clicked 123");
        $(".k-master-row").each(function (index) {
            grid.expandRow(this);
        });
    });
    $("#expand").on("click", function () {
        console.log("expanded");
    })

    $(document).on("click", "#collapse", function () {
        console.log("abcd");
        var grid = $("#grid").data("kendoGrid");
        $(".k-master-row").each(function (index) {
            grid.collapseRow(this);
        });
    });
}))