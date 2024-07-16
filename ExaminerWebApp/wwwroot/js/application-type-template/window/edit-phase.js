$((function () {
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
    };

    function EditPhaseModel(data) {
        data = data || {};

        this.phasename = ko.observable(data.phaseName || "");
        this.templatephaseid = data.templatePhaseId;
        this.ordinal = ko.observable(data.ordinal || "").extend({ required: "Please enter ordinal number" });

        this.isValid = function () {
            return !this.ordinal.hasError();
        };
        this.submitForm = function () {
            this.ordinal.validate();
            if (this.isValid()) {
                var formdata = {
                    TemplatePhaseId: this.templatephaseid,
                    Ordinal: this.ordinal(),
                };

                $.ajax({
                    url: '/ApplicationTypeTemplate/EditTemplatePhase',
                    type: 'POST',
                    contentType: "application/json",
                    data: JSON.stringify(formdata),
                    success: function (response) {
                        if (response.success) {
                            $("#refreshButton").trigger("click");
                            $('#edit-phase').modal('hide');
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
            this.isValid = function () {
                return !this.ordinal.hasError();
            };
        }
    }

    var viewModel = new EditPhaseModel(window.initialData);
    ko.applyBindings(viewModel, document.getElementById("editPhase"));

}))