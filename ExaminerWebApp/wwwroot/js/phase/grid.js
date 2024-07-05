var kendoWindow;
var stepWindow;
$(function () {
    kendoWindow = $("#window").kendoWindow({
        width: "600px",
        title: "Phase",
        visible: false,
        modal: true
    }).data("kendoWindow");

    stepwindow = $("#stepwindow").kendoWindow({
        title: "Step",
        visible: false,
        modal: true
    }).data("kendoWindow");

    var grid = $("#grid").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    var page = options.data.page || 1;
                    var pageSize = options.data.pageSize || 10;

                    $.ajax({
                        url: "/Phase/GetAll",
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
                        phaseId: { editable: false },
                        ordinal: { editable: false },
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
        detailInit: PhaseRow,
        sortable: true,
        editable: false,
        toolbar: [
            {
                name: "create",
                text: "Add Phase",
                attributes: {
                    "class": "k-button k-primary"
                }
            },
            {
                template: kendo.template($("#custom-toolbar-template").html())
            }
        ],
        columns: [
            { field: "phaseId", title: "Phase ID", width: "125px", hidden: true },
            { field: "ordinal", title: "Ordinal", width: "125px", hidden: true },
            { field: "name", title: "Phase", width: "130px" },
            { field: "description", title: "Description", width: "130px" },
            {
                command: [
                    { text: "Add Steps", click: AddSteps },
                    { text: "Edit" },
                    { text: "Delete" }
                ],
                title: "Actions",
                width: "220px",
            }
        ],
        dataBound: function () {
            this.expandRow(this.tbody.find("tr.k-master-row").first());
            $('.k-grid-add').off("click");
            $('.k-grid-add').on("click", function () {
                $.ajax({
                    url: "/Phase/AddPhase",
                    type: 'GET',
                    success: function (result) {
                        kendoWindow.content(result);
                        kendoWindow.center().open();
                    },
                    error: function (error) {
                        console.log(error);
                        alert('error fetching details');
                    },
                });
            });
        },

    }).data("kendoGrid");

    function AddSteps(e) {
        var tr = $(e.target).closest("tr");
        var dataItem = $("#grid").data("kendoGrid").dataItem(tr);
        $.ajax({
            url: "/Phase/AddPhaseSteps",
            type: "GET",
         
            data: {
                phaseId: dataItem.phaseId,
            },
            success: function (result) {
                debugger;
                console.log(result);
                stepwindow.content(result);
                stepwindow.center().open();
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    //Steps Grid
    function PhaseRow(e) {
        var dataItem = e.data; // Use the e.data to get the current data item

        $("<div/>").appendTo(e.detailCell).kendoGrid({
            dataSource: {
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: "Step/GetAll",
                            type: "GET",
                            dataType: "json",
                            data: {
                                phaseId: dataItem.phaseId,
                            },
                            success: function (data) {
                                if (data && data.items && data.totalItems !== undefined) {
                                    options.success(data);
                                } else {
                                    options.error(new Error("Invalid data structure"));
                                }
                            },
                            error: function (error) {
                                console.log(error);
                                alert('Error fetching data.');
                            }
                        });
                    },
                },
                schema: {
                    total: "totalItems",
                    data: "items",
                    model: {
                        fields: {
                            stepId: { editable: false },
                            ordinal: { editable: false },
                            name: { type: "string" },
                            description: { type: "string" },
                        }
                    }
                },
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                pageSize: 10,
            },
            scrollable: false,
            sortable: false,
            pageable: false,
            editable: false,
            columns: [
                { field: "stepId", title: "Step ID", width: "125px", hidden: true },
                { field: "ordinal", title: "Ordinal", width: "125px", hidden: true },
                { field: "name", title: "Name", width: "130px" },
                { field: "description", title: "Description", width: "130px" },
            ],
        });
    }

    // Event handlers for custom toolbar buttons
    $(document).on("click", "#expand", function () {
        var grid = $("#grid").data("kendoGrid");
        $(".k-master-row").each(function (index) {
            grid.expandRow(this);
        });
    });

    $(document).on("click", "#collapse", function () {
        var grid = $("#grid").data("kendoGrid");
        $(".k-master-row").each(function (index) {
            grid.collapseRow(this);
        });
    });
});
