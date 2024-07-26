define(function (require) {
    'use strict';
    var ko = require('knockout'),
        ko_mapping = require('knockout.mapping');
    var EmailTemplate = function (options) {
        var self = this;
 
        options = options || {};
        options.id = options.id || null;
        options.name = options.name || "";
        options.description = options.description || "";
        options.template = options.template || "";

        self.emailTemplateVM = {
            model: {},
            hasId: ko.computed(() => !!options?.id),
            submitTemplate: function () {

                var formData = ko_mapping.toJS(ko.toJS(vm.model));

                var link = options && options.id ? '/EmailTemplate/Edit' : '/EmailTemplate/Create';

                $.ajax({
                    url: link,
                    type: 'POST',
                    data: formData,
                    success: function (response) {
                        if (response.success) {
                            window.location.replace('/EmailTemplate/Index');
                        }
                        else {
                            var errorsHtml = '<ul>';
                            if (typeof (response.errors) === "string") {
                                errorsHtml += '<li>' + response.errors + '</li>'
                            }
                            else {
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
                            }
                            else {
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
        }

        var vm = self.emailTemplateVM;
        self.init = function () {

            vm.model = ko_mapping.fromJS(options);

            $("#templateField").kendoEditor({ value: options.template || "" });

            ko.applyBindings(vm, document.getElementById('email-template-page'));
        };
    }
    return EmailTemplate;
});