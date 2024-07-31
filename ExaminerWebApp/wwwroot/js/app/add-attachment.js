define(function (require) {
    'use strict';
    var ko = require('knockout'),
        ko_validation = require('knockout_validation'),
        ko_mapping = require('knockout.mapping');
    var AddAttachment = function (options) {
        var self = this;
        console.log(options);
        self.attachmentVM = {
            model: {},
            submitForm: function () {
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
                            alert(response.message || "An error occurred while uploading the file.");
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
