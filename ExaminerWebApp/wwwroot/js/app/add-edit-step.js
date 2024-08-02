﻿define(function (require) {
    'use strict';
    var ko = require('knockout'),
        ko_validation = require('knockout_validation'),
        ko_mapping = require('knockout.mapping');

    var AddEditStep = function (options) {
        var self = this;

        self.stepVM = {
            model: {},
            stepTypeId: ko.observable(options.step?.stepTypeId || null),
            showSecondModal: ko.observable(false),
            isIdNotNull: ko.computed(function () {
                return options.id !== null;
            }),

            attachmentModal: function () {
                $.ajax({
                    url: "/TemplatePhaseStep/OpenAttachmentModal",
                    type: "GET",
                    data: { tpsId: options.id },
                    success: function (response) {
                        $("#display-attachment-modal").html(response);
                        $("#attachment-modal").modal('show');
                    },
                    error: function (error) {
                        console.log(error);
                    }
                })
            },

            formSubmit: function () {

                var errors = ko_validation.group(vm.model);
                if (errors().length > 0) {
                    errors.showAllMessages();
                    return;
                }
                var formData = ko_mapping.toJS(ko.toJS(vm.model));
                var link = options && options.id ? '/TemplatePhaseStep/EditTemplatePhaseStep' : '/TemplatePhaseStep/AddTemplatePhaseStep';

                $.ajax({
                    url: link,
                    type: 'POST',
                    data: formData,
                    success: function (response) {
                        if (response.success) {
                            $("#refreshButton").trigger("click");
                            $('#add-edit-step').modal('hide');
                        }
                        else {
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

            attachmentGrid: function () {
                var grid = $("#attachmentsgrid").kendoGrid({
                    dataSource: {
                        transport: {
                            read: function (opt) {
                                var gridData = {
                                    Skip: opt.data.skip,
                                    Take: opt.data.pageSize || 3,
                                    Page: opt.data.page || 1,
                                    PageSize: opt.data.pageSize || 3,
                                    Sort: opt.data.sort,
                                    Filter: opt.data.filter
                                };
                                var pager = JSON.stringify(gridData);
                                $.ajax({
                                    url: "/TemplatePhaseStep/GetTemplatePhaseStepAttachment?tpsId=" + options.id,
                                    type: "POST",
                                    contentType: "application/json",
                                    dataType: "json",
                                    data: pager,
                                    success: function (data) {
                                        opt.success(data);
                                    },
                                    error: function (error) {
                                        console.log(error);
                                        alert('Error fetching data.');
                                    }
                                });
                            },
                            parameterMap: function (data, type) {
                                if (type === "read") {
                                    return kendo.stringify(data);
                                }
                                return data;
                            }
                        },
                        pageSize: 3,
                        serverPaging: true,
                        serverSorting: true,
                        schema: {
                            total: "totalCount",
                            data: function (response) {
                                return response.items.map(function (item) {
                                    return {
                                        id: item.id,
                                        title: item.title,
                                        attachmentTypeId: item.attachmentTypeId,
                                        fileType: item.attachmentType.name,
                                        ordinal: item.ordinal,
                                        filePath: item.filePath
                                    };
                                });
                            },
                            model: {
                                id: "id",
                                fields: {
                                    id: { editable: false },
                                    title: { type: "string" },
                                    attachmentType: { type: "string" },
                                    ordinal: { type: "number", validation: { min: 1 } },
                                }
                            }
                        },
                    },
                    pageable: true,
                    sortable: true,
                    editable: "inline",
                    columns: [

                        { field: "id", hidden: true },

                        { field: "title", title: "Title" },

                        { field: "attachmentTypeId", title: "Attachment Type", editor: documentTypeDropDown, template: "#= fileType #" },

                        { field: "ordinal", title: "Ordinal", width: 125 },
                        {
                            command: [
                                { name: "edit", text: { edit: "", update: "", cancel: "" }, iconClass: ".k-i-pencil", className: "k-button-solid-base", },
                                { name: "download", text: "", iconClass: ".k-i-download", click: DownloadFile },
                                { name: "destroy", text: "", iconClass: ".k-i-trash", className: "k-button-solid-error", }],
                            title: "Actions",
                            width: 150,
                        }
                    ],
                    save: function (e) {
                        e.preventDefault();
                        UpdateAttachment(e.model);
                    },
                    remove: function (e) {
                        DeleteAttachment(e.model);
                    },

                }).data("kendoGrid");
                function DownloadFile(e) {
                    var tr = $(e.target).closest("tr");
                    var dataItem = $("#attachmentsgrid").data("kendoGrid").dataItem(tr);

                    // Construct the file path
                    var filePath = `/UploadedFiles/TPSAttachments/${vm.model.id()}/${dataItem.id}/${encodeURIComponent(dataItem.filePath)}`;

                    // Create an anchor element
                    var link = document.createElement('a');
                    link.href = filePath;

                    // Set the download attribute to the encoded file name
                    link.download = dataItem.title;

                    // Append the anchor to the body, trigger a click, and then remove it
                    document.body.appendChild(link);
                    link.click();
                    document.body.removeChild(link);
                }

                function UpdateAttachment(e) {
                    var formData = {
                        Id: e.id,
                        TemplatePhaseStepId: vm.model.id(),
                        Title: e.title,
                        AttachmentTypeId: e.attachmentTypeId,
                        Ordinal: e.ordinal,
                    };
                    $.ajax({
                        url: "/TemplatePhaseStep/EditTemplatePhaseStepAttachment",
                        type: "POST",
                        contentType: "application/json",
                        dataType: "json",
                        data: JSON.stringify(formData),
                        success: function (response) {
                            if (response.success) {
                                grid.dataSource.read();
                            }
                        },
                        error: function (error) {
                            console.log(error);
                            alert('Error fetching data.');
                        }
                    });
                }

                function DeleteAttachment(e) {
                    $.ajax({
                        url: "/TemplatePhaseStep/DeleteTemplatePhaseStepAttachment",
                        type: "POST",
                        data: {
                            id: e.id
                        },
                        success: function (response) {
                            if (response.success) {
                                grid.dataSource.read();
                            }
                        },
                        error: function (error) {
                            console.log(error);
                            alert('Error fetching data.');
                        }
                    });
                }
            },

            documentProofGrid: function () {
                var grid = $("#documentproofgrid").kendoGrid({
                    dataSource: {
                        transport: {
                            read: function (opt) {
                                var gridData = {
                                    Skip: opt.data.skip,
                                    Take: opt.data.pageSize || 3,
                                    Page: opt.data.page || 1,
                                    PageSize: opt.data.pageSize || 3,
                                    Sort: opt.data.sort,
                                    Filter: opt.data.filter
                                };

                                var pager = JSON.stringify(gridData);

                                $.ajax({
                                    url: "/TemplatePhaseStep/GetTemplatePhaseStepDocumentProof?tpsId=" + options.id,
                                    type: "POST",
                                    contentType: "application/json",
                                    dataType: "json",
                                    data: pager,
                                    success: function (data) {
                                        opt.success(data);
                                    },
                                    error: function (error) {
                                        console.log(error);
                                        alert('Error fetching data.');
                                    }
                                });
                            },
                            parameterMap: function (data, type) {
                                if (type === "read") {
                                    return kendo.stringify(data);
                                }
                                return data;
                            }
                        },
                        page: 1,
                        pageSize: 3,
                        serverPaging: true,
                        serverSorting: true,
                        serverFiltering: true,
                        schema: {
                            total: "totalCount",
                            data: function (response) {
                                return response.items.map(function (item) {
                                    return {
                                        id: item.id,
                                        title: item.title,
                                        description: item.description,
                                        documentFileType: item.documentFileType,
                                        fileType: item.documentFileTypeNavigation.name,
                                        isRequired: item.isRequired,
                                        ordinal: item.ordinal,
                                    };
                                });
                            },
                            model: {
                                id: "id",
                                fields: {
                                    id: { editable: false },
                                    title: {
                                        type: "string",
                                        validation: {
                                            required: { message: "Title is required." },
                                            titleValidation: function (input) {
                                                if (input.is("[title='title']") && input.val() == "") {
                                                    input.attr("data-titleValidation-msg", "Title is required.");
                                                    return false;
                                                }

                                                return true;
                                            },
                                            titleMaxlength: function (input) {
                                                if (input.is("[name='title']") && input.val().length > 50) {
                                                    input.attr("data-titleMaxlength-msg", "Max length is 50 characters");
                                                    return false;
                                                }
                                                return true;
                                            }
                                        },
                                    },
                                    description: {
                                        type: "string",
                                        validation: {
                                            descriptionMaxlength: function (input) {
                                                if (input.is("[name='description']") && input.val().length > 250) {
                                                    input.attr("data-descriptionMaxlength-msg", "Max length is 250 characters");
                                                    return false;
                                                }
                                                return true;
                                            }
                                        }
                                    },
                                    documentFileType: {
                                        type: "number",
                                        validation: {
                                            required: { message: "File Type is required." },
                                            documentFileTypeValidation: function (input) {
                                                if (input.is("[name='documentFileType']") && input.val() == 0) {
                                                    input.attr("data-documentFileTypeValidation-msg", "File Type is required.");
                                                    return false;
                                                }
                                                return true;
                                            }
                                        },
                                    },
                                    fileType: { type: "string" },
                                    isRequired: { type: "boolean" },
                                    ordinal: {
                                        type: "number",
                                        validation: {
                                            required: { message: "Ordinal is required." },
                                            min: 1,
                                            ordinalValidation: function (input) {
                                                if (input.is("[name='ordinal']") && input.val() == 0) {
                                                    input.attr("data-ordinalValidation-msg", "Ordinal is required.");
                                                    return false;
                                                }
                                                return true;
                                            }
                                        }
                                    },
                                }
                            }
                        },
                    },
                    pageable: true,
                    sortable: true,
                    editable: "inline",
                    toolbar: [{ name: "create", text: "Add" }],
                    columns: [
                        { field: "id", hidden: true },
                        { field: "title", title: "Title", width: 150 },
                        { field: "description", title: "Description", width: 275, editor: textBox },
                        { field: "documentFileType", title: "File Type", editor: documentTypeDropDown, template: "#= fileType #", width: 200 },
                        { field: "isRequired", title: "Required", template: "#= isRequired === true ? 'Yes' : 'No' #", editor: requiredCheckbox, width: 125 },
                        { field: "ordinal", title: "Ordinal", width: 100 },
                        {
                            command: [
                                { name: "edit", text: { edit: "", update: "", cancel: "" }, iconClass: ".k-i-pencil", className: "k-button-solid-base", },
                                { name: "destroy", text: "", iconClass: ".k-i-trash", className: "k-button-solid-error", }],
                            title: "Actions",
                            width: 150,
                        }
                    ],
                    save: function (e) {
                        e.preventDefault();
                        if (!e.model.id) {
                            CreateDocumentProof(e.model);
                        } else {
                            UpdateDocumentProof(e.model);
                        }
                    },
                    remove: function (e) {
                        DeleteDocumentProof(e.model);
                    },
                }).data("kendoGrid");

                function requiredCheckbox(container, options) {
                    $('<input type="checkbox" name="isRequired" data-bind="checked: isRequired" />')
                        .appendTo(container)
                        .kendoCheckBox({
                            checked: options.model.isRequired
                        });
                }

                function CreateDocumentProof(e) {
                    var formData = {
                        TemplatePhaseStepId: vm.model.id(),
                        Title: e.title,
                        Description: e.description,
                        DocumentFileType: e.documentFileType,
                        IsRequired: e.isRequired,
                        Ordinal: e.ordinal,
                    };

                    $.ajax({
                        url: "/TemplatePhaseStep/CreateTemplatePhaseStepDocumentProof",
                        type: "POST",
                        contentType: "application/json",
                        dataType: "json",
                        data: JSON.stringify(formData),
                        success: function (response) {
                            if (response.success) {
                                grid.dataSource.read();
                            }
                        },
                        error: function (error) {
                            console.log(error);
                            alert('Error fetching data.');
                        }
                    });
                }

                function UpdateDocumentProof(e) {
                    var formData = {
                        Id: e.id,
                        TemplatePhaseStepId: vm.model.id(),
                        Title: e.title,
                        Description: e.description,
                        DocumentFileType: e.documentFileType,
                        IsRequired: e.isRequired,
                        Ordinal: e.ordinal,
                    };
                    $.ajax({
                        url: "/TemplatePhaseStep/EditTemplatePhaseStepDocumentProof",
                        type: "POST",
                        contentType: "application/json",
                        dataType: "json",
                        data: JSON.stringify(formData),
                        success: function (response) {
                            if (response.success) {
                                grid.dataSource.read();
                            }
                        },
                        error: function (error) {
                            console.log(error);
                            alert('Error fetching data.');
                        }
                    });
                }

                function DeleteDocumentProof(e) {
                    $.ajax({
                        url: "/TemplatePhaseStep/DeleteTemplatePhaseStepDocumentProof",
                        type: "POST",
                        data: {
                            id: e.id
                        },
                        success: function (response) {
                            grid.dataSource.read();
                        },
                        error: function (error) {
                            console.log(error);
                            alert('Error fetching data.');
                        }
                    });
                }
            },

        };

        function documentTypeDropDown(container, options) {
            $('<input required name="' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    autoBind: true,
                    optionLabel: {
                        name: " Select File Type",
                        id: 0
                    },
                    dataTextField: "name",
                    dataValueField: "id",
                    dataSource: documentTypeDataSource,
                    value: options.model[options.field],
                });
        }

        function textBox(container, options) {
            $('<textarea rows="1" name="' + options.field + '" class="k-textbox form-control" data-bind="value:' + options.field + '"></textarea>')
                .appendTo(container);
        }

        var documentTypeDataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: "/TemplatePhaseStep/GetDocumentTypeList",
                    type: "GET",
                    dataType: "json",
                },
            },
        });

        var vm = self.stepVM;

        self.init = function () {
            vm.model = ko_mapping.fromJS(options);

            vm.model.ordinal.extend({
                required: { message: "Ordinal is required" },
                minLength: {
                    message: "Ordinal must be greater than 0",
                    params: 1
                }
            });
            vm.model.stepId.extend({
                required: { message: "Please select a step" },
            });

            vm.model.instruction.extend({
                maxLength: {
                    message: "instruction must be less than 500 letters",
                    params: 500
                }
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

            var ordinalNumericTextBox = $("#ordinal").data("kendoNumericTextBox");
            ordinalNumericTextBox.value(vm.model.ordinal());

            vm.model.ordinal.subscribe(function (newValue) {
                ordinalNumericTextBox.value(newValue);
            });

            $("#stepname").data("kendoDropDownList").value(vm.model.stepId());
            vm.model.stepId.subscribe(function (newValue) {
                $("#stepname").data("kendoDropDownList").value(newValue);
            });

            var stepIdDropdown = $("#steptypeid").data("kendoDropDownList");
            stepIdDropdown.value(vm.stepTypeId());

            vm.stepTypeId.subscribe(function () {
                stepIdDropdown.value(vm.stepTypeId());
            });

            if (vm.model.id() != null) {
                vm.attachmentGrid();
                vm.documentProofGrid();
            }

            // Initialize Kendo Editor
            var instructionElement = $("#instruction");

            if (instructionElement.length) {
                instructionElement.kendoEditor({ value: vm.model.instruction() || "" });

                // Ensure the Kendo Editor instance is available
                var editor = instructionElement.data("kendoEditor");

                if (editor) {
                    // Subscribe to changes in the Knockout observable and update the Kendo Editor
                    vm.model.instruction.subscribe(function (newValue) {
                        if (editor.value() !== newValue) {
                            editor.value() === newValue;
                        }
                    });

                    // Update Knockout observable when the Kendo Editor content changes
                    editor.bind("change", function () {
                        var newValue = editor.value();
                        if (vm.model.instruction() !== newValue) {
                            vm.model.instruction(newValue);
                        }
                    });
                } else {
                    console.error("Kendo Editor instance not found.");
                }
            } else {
                console.error("#instruction element not found.");
            }

            ko.applyBindings(vm, document.getElementById('add-edit-step'));
        };
    };
    return AddEditStep;
})

$("#stepname").on("change", function (e) {
    var newStepId = e.target.value;
    $.ajax({
        url: "/TemplatePhaseStep/GetStepTypeId",
        type: "GET",
        data: { stepId: newStepId },
        success: function (response) {
            if (response && response.stepTypeId) {
                $("#steptypeid").data("kendoDropDownList").value(response.stepTypeId);
            }
        },
        error: function () {
            console.error("Failed to fetch step type ID.");
        }
    });
})