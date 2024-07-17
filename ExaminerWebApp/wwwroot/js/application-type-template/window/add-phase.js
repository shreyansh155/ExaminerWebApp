var phasegrid;
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

    function AddPhaseModel(data) {
        data = data || {};

        this.phaseid = ko.observable(data.phaseid || "").extend({ required: "Please select a phase" });
        this.ordinal = ko.observable(data.ordinal || "").extend({ required: "Please enter ordinal number" });

        this.submitForm = function () {
            this.phaseid.validate();
            this.ordinal.validate();

            if (this.isValid()) {
                var phasegrid = $("#phaseGrid").data("kendoGrid");
                var isValidGrid = true;

                var gridData = phasegrid.dataSource.view().map(function (item) {
                    var ordinal = phasegrid.tbody.find('tr[data-uid="' + item.uid + '"]').find('input[name="ordinal"]').data("kendoNumericTextBox").value();
                    var isChecked = phasegrid.tbody.find('tr[data-uid="' + item.uid + '"]').find('input[type="checkbox"]').is(":checked");

                    if (isChecked && !ordinal) {
                        isValidGrid = false;
                        alert("Ordinal is required for selected phases.");
                    }

                    return {
                        Id: item.id,
                        TemplatePhaseId: item.templatePhaseId,
                        Name: item.name,
                        Ordinal: ordinal,
                        IsInTemplatePhaseSteps: isChecked
                    };
                });

                if (!isValidGrid) return;

                var formdata = {
                    PhaseId: this.phaseid(),
                    Ordinal: this.ordinal(),
                    TemplateId: data.templateId,
                    GridData: gridData.map(item => ({
                        Id: item.Id,
                        TemplatePhaseId: item.templatePhaseId,
                        TemplatePhaseStepId: item.templatePhaseStepId,
                        Name: item.Name,
                        Ordinal: item.Ordinal,
                        IsInTemplatePhaseSteps: item.IsInTemplatePhaseSteps
                    }))
                };

                $.ajax({
                    url: '/ApplicationTypeTemplate/AddTemplatePhase',
                    type: 'POST',
                    contentType: "application/json",
                    data: JSON.stringify(formdata),
                    success: function (response) {
                        if (response.success) {
                            $("#refreshButton").trigger("click");
                            $('#add-phase').modal('hide');
                        } else {
                            var errorsHtml = '<ul>';
                            if (typeof (response.errors) === "string") {
                                errorsHtml += '<li>' + response.errors + '</li>';
                            } else {
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
        };

        this.isValid = function () {
            return !this.phaseid.hasError() && !this.ordinal.hasError();
        };
    }

    var viewModel = new AddPhaseModel(window.initialData);
    ko.applyBindings(viewModel, document.getElementById("addPhase"));

    function GetPhaseSteps(e) {
        var phaseId = e.target.value;

        // Initialize the grid if it does not exist
        if (!$("#phaseGrid").data("kendoGrid")) {
            initializeGrid(phaseId);
        } else {
            phasegrid = $("#phaseGrid").data("kendoGrid");
            phasegrid.setDataSource(new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: "/ApplicationTypeTemplate/GetPhaseSteps",
                            type: "GET",
                            dataType: "json",
                            data: {
                                templateId: window.initialData.templateId,
                                phaseId
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
                    update: function (options) {
                        $.ajax({
                            url: "/ApplicationTypeTemplate/UpdateOrdinal",
                            type: "POST",
                            dataType: "json",
                            data: {
                                templateId: window.initialData.templateId,
                                phaseId: options.data.id,
                                ordinal: options.data.ordinal
                            },
                            success: function (response) {
                                if (response.success) {
                                    $("#refreshButton").trigger("click");
                                }
                            },
                            error: function (error) {
                                console.log(error);
                                alert('Error updating data.');
                            }
                        });
                        options.success(options.data);
                    },
                    parameterMap: function (data, type) {
                        if (type === "read") {
                            return kendo.stringify(data);
                        }
                        return data;
                    }
                },
                schema: {
                    data: function (response) {
                        return response.map(function (item) {
                            return {
                                id: item.step.id,
                                name: item.step.name,
                                ordinal: item.step.ordinal,
                                isInTemplatePhaseSteps: item.isInTemplatePhaseSteps
                            };
                        });
                    },
                    total: function (response) {
                        return response.length;
                    },
                    model: {
                        id: "id",
                        fields: {
                            id: { type: "number" },
                            name: { type: "string" },
                            ordinal: { type: "number" },
                            isInTemplatePhaseSteps: { type: "boolean" }
                        }
                    }
                }
            }));
        }
    }

    function initializeGrid(phaseId) {
        $("#phaseGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: "/ApplicationTypeTemplate/GetPhaseSteps",
                            type: "GET",
                            dataType: "json",
                            data: {
                                templateId: window.initialData.templateId,
                                phaseId
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
                    update: function (options) {
                        $.ajax({
                            url: "/ApplicationTypeTemplate/UpdateOrdinal",
                            type: "POST",
                            dataType: "json",
                            data: {
                                templateId: window.initialData.templateId,
                                phaseId: options.data.id,
                                ordinal: options.data.ordinal
                            },
                            success: function (response) {
                                if (response.success) {
                                    $("#refreshButton").trigger("click");
                                }
                            },
                            error: function (error) {
                                console.log(error);
                                alert('Error updating data.');
                            }
                        });
                        options.success(options.data);
                    },
                    parameterMap: function (data, type) {
                        if (type === "read") {
                            return kendo.stringify(data);
                        }
                        return data;
                    }
                },
                schema: {
                    data: function (response) {
                        return response.map(function (item) {
                            return {
                                id: item.step.id,
                                name: item.step.name,
                                ordinal: item.step.ordinal,
                                isInTemplatePhaseSteps: item.isInTemplatePhaseSteps
                            };
                        });
                    },
                    total: function (response) {
                        return response.length;
                    },
                    model: {
                        id: "id",
                        fields: {
                            id: { type: "number" },
                            name: { type: "string", editable: false },
                            ordinal: { type: "number", validation: { min: 1, required: true } },
                            isInTemplatePhaseSteps: { type: "boolean" }
                        }
                    }
                }
            },
            pageable: false,
            sortable: false,
            scrollable: true,
            editable: "inline",
            height: 0,
            columns: [
                {
                    field: "isInTemplatePhaseSteps",
                    selectable: true,
                    template: '<input type="checkbox" #= isInTemplatePhaseSteps ? "checked=checked" : "" # disabled="disabled" />',
                    width: "40px"
                },
                { field: "name", title: "Step", width: "150px" },
                {
                    field: "ordinal",
                    title: "Ordinal",
                    width: "110px",
                    template: function (dataItem) {
                        return '<input class="k-input k-textbox" name="ordinal" type="number" value="' + dataItem.ordinal + '" min="1" required />';
                    }
                }
            ],
            dataBound: function (e) {
                var grid = e.sender;
                var rows = grid.tbody.find("tr");
                rows.each(function () {
                    var row = $(this);
                    var dataItem = grid.dataItem(row);
                    row.find('input[name="ordinal"]').kendoNumericTextBox({
                        format: "n0",
                        decimals: 0,
                        min: 1,
                    }).data("kendoNumericTextBox").value(dataItem.ordinal);

                    // Validation logic
                    var isChecked = row.find('input[type="checkbox"]').is(":checked");
                    var ordinalValue = row.find('input[name="ordinal"]').data("kendoNumericTextBox").value();
                    if (isChecked && !ordinalValue) {
                        row.find('input[name="ordinal"]').addClass("k-invalid");
                        $("<span class='k-invalid-msg'>Ordinal is required</span>").insertAfter(row.find('input[name="ordinal"]'));
                    }
                });
            }
        });
    }

    $("#phaseid").on("change", function (e) {
        GetPhaseSteps(e);
    });
}));