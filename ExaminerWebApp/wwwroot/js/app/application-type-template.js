$(function () {
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
                        url: "/ApplicationTypeTemplate/GetAll",
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
            },
            pageSize: 10,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            schema: {
                total: "totalCount",
                data: "items",
                model: {
                    fields: {
                        id: { editable: false },
                        name: { type: "string" },
                        description: { type: "string" },
                    }
                }
            },
        },
        toolbar: [
            {
                name: "Create",
                text: "Add Template"
            },
            {
                name: "search",
                text: "Search for..."
            }
        ],
        pageable: {
            pageSize: 10,
            pageSizes: [10, 15, 20]
        },
        sortable: true,
        editable: false,
        columns: [
            { field: "id", title: "", hidden: true },
            { field: "name", title: "Name", width: "300px" },
            { field: "description", title: "Description", },
            {
                command: [
                    {
                        text: "",
                        name:"editTemplate",
                        click: EditEntry,
                        iconClass: ".k-i-pencil",
                        className: "k-button-solid-primary"
                    },
                    {
                        text: "",
                        name:"deleteTemplate",
                        click: DeleteEntry,
                        iconClass: ".k-i-trash",
                        className: "k-button-solid-error"
                    }
                ],
                title: "Actions",
                width: "220px",
            }
        ],
        dataBound: function (e) {

            // Bind click event to the add button
            $('.k-grid-add').off("click").on("click", function () {
                $.ajax({
                    url: "/ApplicationTypeTemplate/ShowTemplateModal",
                    type: 'GET',
                    success: function (result) {
                        $('#displayModal').html(result);
                        $('#templateModal').modal('show');
                        AddTemplateViewModal();
                    },
                    error: function () {
                        alert("An error occurred while loading the content.");
                    }
                });
            });
        }
    }).data("kendoGrid");

    function EditEntry(e) {
        e.preventDefault();
        var tr = $(e.target).closest("tr");
        var dataItem = $("#grid").data("kendoGrid").dataItem(tr);

        $.ajax({
            url: "/ApplicationTypeTemplate/GetApplicationTemplate",
            type: 'GET',
            data: { id: dataItem.id },
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
                alert('Error fetching details.');
            },
        });
    }


    function DeleteEntry(e) {
        e.preventDefault();
        var tr = $(e.target).closest("tr");
        var dataItem = $("#grid").data("kendoGrid").dataItem(tr);
        if (confirm("Are you sure you want to delete this entry?")) {
            $.ajax({
                url: "/ApplicationTypeTemplate/DeleteTemplate",
                type: 'POST',
                data: { id: dataItem.id },
                success: function (result) {
                    $("#refreshButton").trigger("click");
                    alert("Applicant has been deleted successfully");
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

    //TEMPLATE ADDITION MODAL IN INDEX (TEMPLATE GRID VIEW) // window/window.js
    function AddTemplateViewModal() {
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
                                $('#templateModal').modal('hide');
                            } else {
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
            this.isValid = function () {
                return !this.name.hasError();
            };
        }
        var viewModel = new ApplicationTypeTemplateModel(window.initialData);
        ko.applyBindings(viewModel, document.getElementById("templateForm"));
    }
});