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
$("#refreshButton").on("click", function () {
    $("#errorBody").empty();
});