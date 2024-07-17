$(function () {
    var grid = $("#grid").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    var page = options.data.page || 1;
                    var pageSize = options.data.pageSize || 10;

                    var searchValue = $(".k-searchbox.k-input");
                    console.log(searchValue);

                    $.ajax({
                        url: "/ApplicationTypeTemplate/GetAll",
                        type: "GET",
                        dataType: "json",
                        data: {
                            pageNumber: page,
                            pageSize: pageSize,
                            search: $("#searchBox").val()
                        },
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
            serverFiltering: true,
            serverSorting: true,
            schema: {
                total: "totalItems",
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
        pageable: {
            pageSize: 10,
            pageSizes: [10, 15, 20]
        },
        editable: false,
        toolbar: [
            {
                name: "Create",
                text: "Add Template"
            },
            {
                template: kendo.template($("#searchGrid").html())
            }
        ],
        columns: [
            { field: "id", title: "", width: "125px", hidden: true },
            { field: "name", title: "Name", width: "130px" },
            { field: "description", title: "Description", width: "130px" },
            {
                command: [
                    {
                        text: "Edit",
                        click: EditEntry,
                        iconClass: ".k-i-pencil",
                        attributes: {
                            "class": "k-button k-primary"
                        },
                    },
                    {
                        text: "Delete",
                        click: DeleteEntry,
                        iconClass: ".k-i-trash",
                        attributes: {
                            "class": "k-button k-primary"
                        },
                    }
                ],
                title: "Actions",
                width: "220px",
            }
        ],
        dataBound: function (e) {
            $('.k-grid-add').off("click");
            $('.k-grid-add').on("click", function () {
                $.ajax({
                    url: "/ApplicationTypeTemplate/ShowTemplateModal",
                    type: 'GET',
                    success: function (result) {
                        $('#displayModal').html(result);
                        $('#templateModal').modal('show');
                    },
                    error: function () {
                        alert("An error occurred while loading the content.");
                    }
                });
            });
        },
    }).data("kendoGrid");

    $("#searchBox").keyup(function () {
        grid.dataSource.read();
    });
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
                } else {
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
});

$("#clearButton").on("click", function () {
    $("#searchBox").val("");
    $("#refreshButton").trigger("click");
})