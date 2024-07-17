$(function () {
    ko.extenders.required = function (target, overrideMessage) {
        target.hasError = ko.observable(false);
        target.validationMessage = ko.observable();
        target.validationTriggered = ko.observable(false);

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
    }

    function AddEditStepModel(data) {
        data = data || {};

        this.stepid = ko.observable(data.stepId || null).extend({ required: "Please select step" });
        this.steptypeid = ko.observable(data.stepTypeId || "");
        this.ordinal = ko.observable(data.ordinal || "").extend({ required: "Please enter ordinal number" });
        this.stepinstruction = ko.observable(data.instruction || "");

        this.formSubmit = function () {
            this.stepid.validate();
            this.ordinal.validate();

            if (this.isValid()) {

                var formdata = {
                    StepId: this.stepid(),
                    Ordinal: this.ordinal(),
                    Id: data.id,
                    Instruction: this.stepinstruction(),
                    TemplatePhaseId: data.templatePhaseId
                };

                $.ajax({
                    url: '/ApplicationTypeTemplate/EditTemplatePhaseStep',
                    type: 'POST',
                    contentType: "application/json",
                    data: JSON.stringify(formdata),
                    success: function (response) {
                        if (response.success) {
                            $("#add-edit-step").modal('hide');
                            $("#refreshButton").trigger("click");
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
            return !this.stepid.hasError() && !this.ordinal.hasError();
        };
    }

    var viewModel = new AddEditStepModel(window.initialData);
    ko.applyBindings(viewModel, document.getElementById("add-edit-step-form"));

    //step name binding
    $("#stepname").data("kendoDropDownList").value(viewModel.stepid());

    viewModel.stepid.subscribe(function (newValue) {
        $("#stepname").data("kendoDropDownList").value(newValue);
    });

    //Step type binding
    $("#steptypeid").data("kendoDropDownList").value(viewModel.steptypeid());

    $("#stepname").on("change", function (e) {
        var newStepId = e.target.value;
        console.log(newStepId);
        $.ajax({
            url: "/ApplicationTypeTemplate/GetStepTypeId",
            type: "GET",
            data: { stepId: newStepId },
            success: function (response) {
                if (response && response.stepTypeId) {
                    $("#steptypeid").data("kendoDropDownList").value(response.stepTypeId);
                }
            },
            error: function () {
                console.error("Failed to fetch step type ID.");
            }
        });
    })
    viewModel.steptypeid.subscribe(function (newValue) {
        $("#steptypeid").data("kendoDropDownList").value(newValue);
    }); 

    //ordinal binding
    var ordinalNumericTextBox = $("#ordinal").data("kendoNumericTextBox");
    ordinalNumericTextBox.value(viewModel.ordinal());

    viewModel.ordinal.subscribe(function (newValue) {
        ordinalNumericTextBox.value(newValue);
    });
});