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

    var patternsRegex = {
        passwordRegex: /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/,
    };

    // Custom extender for comparing passwords
    ko.extenders.equal = function (target, compareTo) {
        target.hasError = ko.observable(false);
        target.validationMessage = ko.observable();
        target.validationTriggered = ko.observable(false);

        function validate(newValue) {
            if (target.validationTriggered()) {
                target.hasError(newValue !== compareTo());
                target.validationMessage(newValue !== compareTo() ? "Passwords do not match" : "");
            }
        }

        target.subscribe(validate);

        target.validate = function () {
            target.validationTriggered(true);
            validate(target());
        };

        return target;
    };

    // View model for the modal form
    function ResetPassword() {
        var self = this;

        // Define observables for each form field with validation
        self.password = ko.observable().extend({
            required: "Please enter password",
            pattern: {
                message: "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (#?!@$%^&*-)",
                params: patternsRegex.passwordRegex
            }
        });
        self.confirmPassword = ko.observable().extend({
            required: "Please confirm your password",
            equal: self.password
        });

        // Validate all observables
        self.validateAll = function () {
            self.password.validate();
            self.confirmPassword.validate();
        };

        // Helper function to check if all fields are valid
        self.isValid = function () {
            return !self.password.hasError() && !self.confirmPassword.hasError();
        };
    }

    // Initialize Knockout.js bindings
    var resetPasswordViewModel = new ResetPassword();
    ko.applyBindings(resetPasswordViewModel, $(".reset-password-box")[0]);

    // Attach the submit event handler to the form
    $("#resetPasswordForm").submit(function (event) {
        resetPasswordViewModel.validateAll();

        // Check if the form is valid before submitting
        if (!resetPasswordViewModel.isValid()) {
            event.preventDefault(); // Prevent the form from being submitted

            // Scroll to the first invalid field
            var firstInvalidField = $(".has-error").first();
            if (firstInvalidField.length) {
                $("html, body").animate({
                    scrollTop: firstInvalidField.offset().top - 100
                }, 500);
                firstInvalidField.find("input, select, textarea").trigger("focus");
            }
        }
    });
});
