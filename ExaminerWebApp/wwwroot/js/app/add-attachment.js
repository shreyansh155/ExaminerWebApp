define(function (require) {
    'use strict';
    var ko = require('knockout'),
        ko_validation = require('knockout_validation'),
        ko_mapping = require('knockout.mapping');

    ko_validation.rules['notEqual'] = {
        validator: function (val, otherVal) {
            return val !== otherVal;
        },
        message: 'File type is required'
    };

    ko_validation.registerExtenders();
    var AddAttachment = function (options) {
        var self = this;
        self.attachmentVM = {
            model: {},
            submitForm: function () {
                var errors = ko_validation.group(vm.model);

                if (errors().length > 0) {
                    errors.showAllMessages();
                    return;
                }

                var formData = new FormData($("#add-attachment-form")[0]);
                formData.append("Title", vm.model.title());
                formData.append("AttachmentTypeId", vm.model.attachmentTypeId());
                formData.append("TemplatePhaseStepId", vm.model.templatePhaseStepId());

                var files = $("input[name=formFile]")[0].files;
                if (files.length > 0) {
                    formData.append("AttachmentFile", files[0]);
                }
                $.ajax({
                    url: "/TemplatePhaseStep/CreateAttachment",
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        if (response.success) {
                            $("#attachmentsgrid").data("kendoGrid").dataSource.read();
                            $('#attachment-modal').modal('hide');
                        } else {
                            console.log(response.errors);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert("An error occurred: " + error);
                    }
                });
            }
        }
        var vm = self.attachmentVM;
        self.init = function () {
            vm.model = ko_mapping.fromJS(options);

            vm.model.title.extend({
                required: { message: "Title is required" },
                maxLength: {
                    message: "Title must be less than 50 characters",
                    params: 50
                }
            });

            vm.model.attachmentTypeId.extend({
                notEqual: 0
            });

            vm.model.attachmentFile.extend({
                required: { message: "File is required" },
            });

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

            ko.applyBindings(vm, document.getElementById('attachment-modal'));
        }
    }
    return AddAttachment;
});