$(function () {
    var grid = $("#grid").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    var page = options.data.page || 1;
                    var pageSize = options.data.pageSize || 10;

                    $.ajax({
                        url: "/ApplicationTypeTemplate/GetAll",
                        type: "GET",
                        dataType: "json",
                        data: {
                            pageNumber: page,
                            pageSize: pageSize
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
        sortable: true,
        editable: false,
        toolbar: ["Create"],
        columns: [
            { field: "id", title: "", width: "125px", hidden: true },
            { field: "name", title: "Name", width: "130px" },
            { field: "description", title: "Description", width: "130px" },
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
                    url: "/ApplicationTypeTemplate/AddTemplate",
                    type: 'GET',
                    success: function (result) {
                        $('#displayModal').html(result);
                        $('#application-type-template').data("kendoWindow").open();
                    },
                    error: function (error) {
                        console.log(error);
                        alert('error fetching details');
                    },
                });
            });
        },
    }).data("kendoGrid");

    function EditEntry(e) {
        e.preventDefault();
        var tr = $(e.target).closest("tr");
        var dataItem = $("#grid").data("kendoGrid").dataItem(tr);

        $.ajax({
            url: "/Applicant/GetApplicant",
            type: 'GET',
            data: { Id: dataItem.id },
            success: function (result) {
                $('#displayModal').html(result);
                $('#application-type-template').data("kendoWindow").open();
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
                url: "/Applicant/DeleteApplicant",
                type: 'POST',
                data: { Id: dataItem.id },
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

    // Initialize Kendo Window
    $("#application-type-template").kendoWindow({
        title: "Ram's Ten Principles of Good Design",
        draggable: true,
        resizable: true,
        width: "600px",
        modal: true,
        visible: false,
        actions: ["Close"],
        close: onClose
    });

    function onClose() {
        $("#undo").show();
    }

    $("#undo").bind("click", function () {
        $("#application-type-template").data("kendoWindow").open();
        $("#undo").hide();
    });
});
