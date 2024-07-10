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
                var formdata = {
                    PhaseId: this.phaseid(),
                    Ordinal: this.ordinal(),
                    TemplateId: data.templateId
                };
                $.ajax({
                    url: '/ApplicationTypeTemplate/AddTemplatePhase',
                    type: 'POST',
                    contentType: "application/json",
                    data: JSON.stringify(formdata),
                    success: function (response) {
                        if (response.success) {
                            $("#refreshButton").trigger("click");
                        } else {
                            var errorsHtml = '<ul>';
                            if (typeof (response.errors) === "string") {
                                errorsHtml += '<li>' + response.errors + '</li>';
                            } else {
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
        };

        this.isValid = function () {
            return !this.phaseid.hasError();
        };
    }
    var viewModel = new AddPhaseModel(window.initialData);

    ko.applyBindings(viewModel, document.getElementById("addPhase"));
    function GetPhaseSteps(e) {
        var phaseId = e.target.value;
        phasegrid = $("#phaseGrid").kendoGrid({
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
                                alert('Error fetching data.');
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
                                id: item.phase.id,
                                ordinal: item.ordinal,
                                phase: item.phase.name,
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
                            ordinal: { type: "number" },
                            phase: { type: "string", editable: false },
                        },
                    },
                },
            },
            pageable: false,
            sortable: false,
            scrollable: true,
            editable: "inline",
            height: 0,
            columns: [
                { selectable: true, width: "50px" },
                { field: "phase", title: "Phase", width: "110px" },
                { field: "ordinal", title: "Ordinal", width: "110px" },
                {
                    command: ["edit"],
                    width: "100px"
                }
            ],
        }).data("kendoGrid");
        //function setGridHeightToRows(count) {
        //    var rowHeight = $("#grid .k-grid-content tr").first().outerHeight();
        //    var headerHeight = $("#grid .k-grid-header").outerHeight();
        //    var newHeight = (rowHeight * count) + headerHeight;
        //    $("#phaseGrid").height(newHeight);
        //    phasegrid.resize();
        //}
    }


    //setGridHeightToRows(3);
    $("#refreshButton").on("click", function () {
        phasegrid.dataSource.read();
    });
    $("#closeBtn").click(function () {
        $(this).closest("[data-role=window]").data("kendoWindow").close();
    });
    $("#phaseid").on("change", function (e) {
        GetPhaseSteps(e);
    })
}))
