define(function (require) {
    'use strict';
    var ko = require('knockout'),
        ko_validation = require('knockout_validation'),
        ko_mapping = require('knockout.mapping');

    var EmailTemplate = function (options) {
        var self = this;

        options = options || {};

        self.emailTemplateVM = {
            model: {},
            hasId: ko.computed(() => !!options?.id),
            submitTemplate: function () {

                // function to validate fields
                var errors = ko_validation.group(vm.model);
                if (errors().length > 0) {
                    errors.showAllMessages();
                    return;
                }
                var formData = ko_mapping.toJS(ko.toJS(vm.model));
                var link = options && options.id ? '/EmailTemplate/Edit' : '/EmailTemplate/Create';

                $.ajax({
                    url: link,
                    type: 'POST',
                    data: formData,
                    success: function (response) {
                        if (response.success) {
                            window.location.replace('/EmailTemplate/Index');
                        } else {
                            DisplayErrorMessages();
                            var errorsHtml = '<ul>';
                            if (typeof (response.errors) === "string") {
                                errorsHtml += '<li>' + response.errors + '</li>'
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
            },

            deleteTemplate: function () {
                if (confirm("Are you sure you want to delete this template?")) {
                    $.ajax({
                        url: "/EmailTemplate/Delete",
                        type: 'POST',
                        data: {
                            id: options.id,
                        },
                        success: function (result) {
                            if (result.success) {
                                alert("Email Template has been deleted successfully");
                                window.location.replace('/EmailTemplate/Index');
                                $("#refreshButton").trigger("click");
                            } else {
                                alert(result.errors);
                            }
                        },
                        error: function (error) {
                            console.log(error);
                            alert('error deleting details');
                        },
                    });
                }
            },

            backBtn: function () {
                window.location.replace("/EmailTemplate/Index")
            }
        };

        var vm = self.emailTemplateVM;

        self.init = function () {
            vm.model = ko_mapping.fromJS(options);

            vm.model.name.extend({
                required: { message: "Name is required" },
                maxLength: {
                    message: "Name must be less than 50 letters",
                    params: 50
                },
                minLength: {
                    message: "Name must be more than 3 letters",
                    params: 3
                }
            });
            vm.model.description.extend({
                required: { message: "Description is required" },
                maxLength: {
                    message: "Description must be less than 500 letters",
                    params: 500
                }
            });
            vm.model.template.extend({
                maxLength: {
                    message: "Template must be less than 500 letters",
                    params: 500
                }
            });

            $("#templateField").kendoEditor({ value: options.template || "" });

            ko_validation.init({
                registerExtenders: true,
                messagesOnModified: true,
                insertMessages: false,
                parseInputAttributes: true,
                messageTemplate: null,
                decorateInputElement: true,
                errorElementClass: 'err',
                errorsAsTitle: false
            }, true);

            ko.applyBindings(vm, document.getElementById('email-template-page'));
        };
    };

    return EmailTemplate;
});