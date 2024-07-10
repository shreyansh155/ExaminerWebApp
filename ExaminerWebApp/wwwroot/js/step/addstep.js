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
    function AddStepModel(data) {

        data = data || {};

        this.steptypeid = ko.observable(data.phasename || "").extend({ required: "Please select phase type" });
        this.phaseordinal = ko.observable();
        this.submitForm = function () {
            this.phasename.validate();

            if (this.isValid()) {
                var formData = new FormData($("#phaseForm")[0]);
                formData.append("Name", this.phasename());
                formData.append("Description", this.phasedescription());


                $.ajax({
                    url: '/Step/CreateStep',
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
            return !this.phasename.hasError();
        };
    }
    var viewModel = new AddStepModel(window.initialData);
    ko.applyBindings(viewModel, document.getElementById("addStep"));

    var phaseId = window.initialData.phaseId;

    var grid = $("#ordinalGrid").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax({
                        url: "/Step/GetAllSteps",
                        type: "GET",
                        dataType: "json",
                        data: {
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
                total: "totalItems",
                data: "items",
                model: {
                    fields: {
                        checkbox: { editable: false },
                        stepname: { type: "string" },
                        ordinal: { type: "string" },
                    }
                }
            },
        },
        pageable: false,
        sortable: false,
        editable: "inline",
        columns: [
            { "field": "checkbox", title: "", "template": "<input type=\"checkbox\" />" },

            { field: "stepname", title: "Name", width: "130px" },

            { field: "ordinal", title: "Ordinal", width: "130px" },
        ],
        dataBound: function (e) {
            $('.k-grid-add').off("click");
            $('.k-grid-add').on("click", function () {
                $.ajax({
                    url: "/Step/Create",
                    type: 'GET',
                    success: function (result) {
                        stepPhaseWindow.content(result);
                        stepPhaseWindow.center().open();
                    },
                    error: function (error) {
                        console.log(error);
                        alert('error fetching details');
                    },
                });
            });
        },

    }).data("kendoGrid");
}))
