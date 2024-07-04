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
        this.submitForm = function () {
            this.name.validate();

            if (this.isValid()) {
                var formData = new FormData($("#templateForm")[0]);
                formData.append("Name", this.name());
                formData.append("Description", this.description());
                formData.append("Instruction", this.instruction());
                if (data.id) {
                    formData.append("Id", data.id);
                }

                var link = data && data.id ? '/ApplicationTypeTemplate/EditTemplate' : '/ApplicationTypeTemplate/AddTemplate';

                $.ajax({
                    url: link,
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
}))
