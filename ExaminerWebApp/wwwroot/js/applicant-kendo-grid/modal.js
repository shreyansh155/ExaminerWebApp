$(function () {
    // Custom Knockout.js extender for required fields
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

    // Custom Knockout.js extender for pattern validation
    ko.extenders.pattern = function (target, options) {
        target.hasError = ko.observable(false);
        target.validationMessage = ko.observable();
        target.validationTriggered = ko.observable(false); // Track if validation should be triggered

        function validate(newValue) {
            if (target.validationTriggered()) {
                target.hasError(!options.params.test(newValue));
                target.validationMessage(!options.params.test(newValue) ? options.message : "");
            }
        }

        target.subscribe(validate);

        target.validate = function () {
            target.validationTriggered(true);
            validate(target());
        };

        return target;
    };

    function formatDate(dateString) {
        if (dateString) {
            var date = new Date(dateString);
            return kendo.toString(date, "dd-MM-yyyy");

        }
        return "";
    }

    var patternsRegex = {
        phone: /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/,
        email: /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/,
    };

    // View model for the modal form
    function ApplicantViewModel(data) {
        data = data || {};

        // Define observables for each form field with validation
        this.settingid = ko.observable(data.settingid || "").extend({ required: "Please select Application Type" });
        this.firstName = ko.observable(data.firstname || "").extend({
            required: "First name is required",
            noNumbers: true
        });
        this.lastName = ko.observable(data.lastname || "").extend({
            required: "Last name is required",
            noNumbers: true
        });
        this.dateOfBirth = ko.observable(formatDate(data.dateofbirth) || "").extend({ required: "Date of birth is required" });
        this.phoneNumber = ko.observable(data.phone || null).extend({
            required: "Phone number is required",
            pattern: {
                params: patternsRegex.phone,
                message: "Entered phone format is not valid."
            }
        });
        this.email = ko.observable(data.email || "").extend({
            required: "Email is required",
            pattern: {
                params: patternsRegex.email,
                message: "Entered email format is not valid",
            }
        });

        // this.applicanttypeid = ko.observable(data.applicanttypeid || "").extend({ required: "Please select application type" });

        this.formFile = ko.observable(null);

        this.viewFile = function () {
            if (data.filepath === null || data.filepath === "") {
                alert("No file found!");
                return;
            }
            var filePath = `UploadedFiles/${data.filepath}`;
            window.open(filePath, '_blank');
        }

        this.submitForm = function () {
            // Validate all observables
            this.firstName.validate();
            this.lastName.validate();
            this.dateOfBirth.validate();
            this.phoneNumber.validate();
            this.email.validate();
            this.settingid.validate();



            if (this.isValid()) {
                var formData = new FormData($("#addApplicant")[0]);
                formData.append("Firstname", this.firstName());
                formData.append("Lastname", this.lastName());
                formData.append("Dateofbirth", this.dateOfBirth());
                formData.append("Phone", this.phoneNumber());
                formData.append("Email", this.email());
                formData.append("Settingid", this.settingid());

                if (this.formFile()) {
                    formData.append("FormFile", this.formFile());
                }
                if (data.id) {
                    formData.append("Id", data.id);
                }

                var link = data && data.id ? '/Applicant/EditApplicant' : '/Applicant/AddApplicant';

                $.ajax({
                    url: link,
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        if (response.success) {
                            $("#refreshButton").trigger("click");
                            $('#applicationForm').modal('hide');
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
            } else {
                // Scroll to the first invalid field
                var firstInvalidField = $(".has-error").first();
                if (firstInvalidField.length) {
                    $("html, body").animate({
                        scrollTop: firstInvalidField.offset().top - 100
                    }, 500);
                    firstInvalidField.find("input, select, textarea").trigger("focus");
                }
            }
        };

        String.prototype.camelCase = function () {
            return this.charAt(0).toLowerCase() + this.slice(1);
        };

        // Helper function to check if all fields are valid
        this.isValid = function () {
            return !this.firstName.hasError() && !this.lastName.hasError() && !this.dateOfBirth.hasError()
                && !this.phoneNumber.hasError() && !this.email.hasError() && !this.settingid.hasError();
        };
    }

    // Initialize Knockout.js bindings when the modal is shown
    $('#applicationForm').on('shown.bs.modal', function () {
        var viewModel = new ApplicantViewModel(window.initialData);
        ko.applyBindings(viewModel, $("#addApplicant")[0]);
    });
});