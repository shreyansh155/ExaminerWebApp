$(function () {
    var grid = $("#grid").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    var page = options.data.page || 1;
                    var pageSize = options.data.pageSize || 10;

                    $.ajax({
                        url: "/Examiner/GetAll",
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
                        firstname: { type: "string" },
                        lastname: { type: "string" },
                        dateofbirth: { type: "date" },
                        phone: { type: "string" },
                        email: { type: "string" },
                        examinerTypeName: { type: "string" },
                        status: { type: "string" }
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

            { field: "id", title: "Examiner ID", width: "125px", hidden: true },

            { field: "firstname", title: "First Name", width: "130px" },

            { field: "lastname", title: "Last Name", width: "130px" },

            { field: "dateofbirth", title: "Date Of Birth", width: "130px", format: "{0:dd/MM/yyyy}" },

            { field: "phone", title: "Phone Number", width: "150px" },

            { field: "email", title: "Email", width: "250px" },

            { field: "examinerTypeName", title: "Examiner Type", width: "200px" },

            { field: "status", title: "Status", width: "100px" },
            {
                command: [
                    { text: "View", click: OpenFile },
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
                    url: "/Examiner/ExaminerForm",
                    type: 'GET',
                    success: function (result) {
                        $('#displayModal').html(result);
                        $('#examiner-form').modal('show');
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

        //open modal to edit the row data
        $.ajax({
            url: "/Examiner/GetExaminer",
            type: 'GET',
            data: {
                Id: dataItem.id
            },
            success: function (result) {
                $('#displayModal').html(result);
                $('#examiner-form').modal('show');
            },
            error: function (error) {
                console.log(error);
                alert('error fetching details');
            },
        });
    }

    function OpenFile(e) {
        var tr = $(e.target).closest("tr");
        var dataItem = $("#grid").data("kendoGrid").dataItem(tr);

        if (dataItem.filepath === null || dataItem.filepath === "") {
            alert("No file found!");
            return;
        }
        var filePath = `UploadedFiles/${dataItem.filepath}`;
        window.open(filePath, '_blank');
    }

    function DeleteEntry(e) {
        e.preventDefault();

        var tr = $(e.target).closest("tr");
        var dataItem = $("#grid").data("kendoGrid").dataItem(tr);

        if (confirm("Are you sure you want to delete this entry?")) {
            $.ajax({
                url: "/Examiner/DeleteExaminer",
                type: 'POST',
                data: {
                    Id: dataItem.id
                },
                success: function (result) {
                    if (result) {
                        $("#refreshButton").trigger("click");
                        alert("Examiner has been deleted successfully");
                    }
                    else {
                        alert("Examiner cannot be deleted, some error occured,", result.error);
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
});
