const emailtemplateGrid = document.getElementById('email-template-grid');
const emailTemplate = document.getElementById('email-template-page');
$(function () {
    if (emailtemplateGrid) {
        var grid = $("#grid").kendoGrid({
            dataSource: {
                transport: {
                    read: function (options) {
                        var gridData = {
                            Skip: options.data.skip,
                            Take: options.data.pageSize || 10,
                            Page: options.data.page || 1,
                            PageSize: options.data.pageSize || 10,
                            Sort: options.data.sort,
                            Filter: options.data.filter
                        };
                        var pager = JSON.stringify(gridData);
                        $.ajax({
                            url: "/EmailTemplate/GetAll",
                            type: "POST",
                            contentType: "application/json",
                            dataType: "json",
                            data: pager,
                            success: function (data) {
                                options.success(data);
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
                pageSize: 10,
                serverPaging: true,
                serverPaging: true,
                serverSorting: true,
                schema: {
                    total: "totalCount",
                    data: "items",
                    model: {
                        fields: {
                            id: { editable: false },
                            name: { type: "string" },
                            isDefault: { type: "boolean", editable: false },
                        }
                    }
                },
            },
            pageable: {
                pageSize: 10,
                pageSizes: [10, 15, 20]
            },
            sortable: true,
            editable: false,
            toolbar: [{ name: "create", text: "Add" }],
            columns: [

                { field: "id", title: "EmailTemplate ID", width: "125px", hidden: true },

                { field: "name", title: "Name" },

                {
                    field: "isDefault",
                    title: "Default",
                    width: 110,
                    template: '<input type="checkbox" #= isDefault ? \'checked="checked"\' : "" # disabled class="chkbx k-checkbox k-checkbox-md k-rounded-md" />',
                    attributes: { class: "k-text-center" }
                },
                {
                    command: [
                        { text: "Edit", click: EditEntry },
                        { text: "Delete", click: DeleteEntry }
                    ],
                    title: "Actions",
                    width: "220px",
                }
            ],
            dataBound: function (e) {
                $('.k-grid-add').off("click");
                $('.k-grid-add').on("click", function () {
                    $.ajax({
                        url: "/EmailTemplate/EmailTemplate",
                        type: 'GET',
                        success: function (response) {
                            if (response.success) {
                                window.location.href = "/EmailTemplate/Create"
                            }
                        },
                        error: function (error) {
                            console.log(error);
                            alert('error fetching details');
                        },
                    });
                });
            },


        }).data("kendoGrid");
        $("#grid .k-grid-content").on("change", "input.chkbx", function (e) {
            var grid = $("#grid").data("kendoGrid"),
                dataItem = grid.dataItem($(e.target).closest("tr"));

            dataItem.set("IsDefault", this.checked);
        });



        function EditEntry(e) {
            e.preventDefault();
            var tr = $(e.target).closest("tr");
            var dataItem = $("#grid").data("kendoGrid").dataItem(tr);

            $.ajax({
                url: "/EmailTemplate/EditTemplate",
                type: 'GET',
                data: {
                    id: dataItem.id
                },
                success: function (result) {
                    if (result.redirectUrl) {
                        window.location.href = result.redirectUrl;
                    }
                    else {
                        console.log('No redirect URL found in response.');
                    }
                },
                error: function (error) {
                    console.log(error);
                    alert('error fetching details');
                },
            });
        }
        function DeleteEntry(e) {
            e.preventDefault();

            var tr = $(e.target).closest("tr");
            var dataItem = $("#grid").data("kendoGrid").dataItem(tr);

            if (confirm("Are you sure you want to delete this entry?")) {
                $.ajax({
                    url: "/EmailTemplate/Delete",
                    type: 'POST',
                    data: {
                        id: dataItem.id
                    },
                    success: function (result) {
                        if (result.success) {
                            $("#refreshButton").trigger("click");
                            alert("EmailTemplate has been deleted successfully");
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
        }
        $("#refreshButton").on("click", function () {
            grid.dataSource.read();
        });
    }

    else if (emailTemplate) {
        EmailTemplateViewPage()
        function EmailTemplateViewPage() {
            function EmailTemplateModel(data) {

                data = data || {};

                this.name = ko.observable(data.name || "").extend({
                    required: "Please enter Email Template name",
                    maxLength: {
                        maxLength: 50,
                        overrideMessage: "Email Template name must be less than 50 letters."
                    }
                });
                this.description = ko.observable(data.description || "").extend({
                    required: "Please enter Description"
                });;
                this.template = ko.observable(data.template || "");
                this.id = ko.observable(data.id || null);
                this.isDefault = ko.observable(data.isDefault || false);

                this.hasId = ko.computed(() => !!this.id()); //to show and hide delete button

                this.submitTemplate = function () {

                    this.name.validate();
                    this.description.validate();

                    if (this.isValid()) {
                        var formData = new FormData($("#emailTemplateForm")[0]);
                        formData.append("Name", this.name());
                        formData.append("Description", this.description());
                        formData.append("Template", this.template());
                        formData.append("IsDefault", $("#isDefault").val());

                        if (data && data.id) {
                            formData.append("Id", data.id);
                        }

                        var link = data && data.id ? '/EmailTemplate/Edit' : '/EmailTemplate/Create';
                        $.ajax({
                            url: link,
                            type: 'POST',
                            data: formData,
                            processData: false,
                            contentType: false,
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
                    }
                }

                this.deleteTemplate = function () {
                    if (confirm("Are you sure you want to delete this template?")) {
                        $.ajax({
                            url: "/EmailTemplate/Delete",
                            type: 'POST',
                            data: {
                                id: data.id,
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
                }

                this.isValid = function () {
                    return !this.name.hasError() && !this.description.hasError();
                };
            }

            var viewModel = new EmailTemplateModel(window.initialData);

            ko.applyBindings(viewModel, document.getElementById("emailTemplateForm"));

            $("#back-btn").on("click", function () {
                window.location.replace("/EmailTemplate/Index")
            })
        }
    }
});