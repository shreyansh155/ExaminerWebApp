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
    function PhaseViewModel(data) {

        data = data || {};

        this.phasename = ko.observable(data.name || "").extend({ required: "Please enter phase name" });
        this.phasedescription = ko.observable(data.description || "");

        this.submitForm = function () {
            this.phasename.validate();

            if (this.isValid()) {
                var formData = new FormData($("#phaseForm")[0]);
                formData.append("Name", this.phasename());
                formData.append("Description", this.phasedescription());
                if (data.phaseId) {
                    formData.append("PhaseId", data.phaseId);
                }

                var link = data && data.phaseId ? '/Phase/EditPhase' : '/Phase/CreatePhase';

                $.ajax({
                    url: link,
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        if (response.success) {
                            $("#refreshButton").trigger("click");
                            $('#create-phase').modal('hide');
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

    var viewModel = new PhaseViewModel(window.initialData);
    ko.applyBindings(viewModel, document.getElementById("phaseForm"));
}))