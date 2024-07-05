var kendoWindow;
$((function () {
  
    ko.extenders.required = function (target, overrideMessage) {
        target.hasError = ko.observable(false);
        target.validationMessage = ko.observable();
        target.validationTriggered = ko.observable(false); // Track if validation should be triggered

        function validate(newValue) {
            if (target.validationTriggered()) {
                target.hasError(!newValue);
                target.validationMessage(!newValue ? overrideMessage || "This field is required" : "");
            }
        }

        target.subscribe(validate);

        target.validate = function () {
            target.validationTriggered(true);
            validate(target());
        };

        return target;
    };
    function ApplicationTypeTemplateModel(data) {

        data = data || {};

        this.name = ko.observable(data.name || "").extend({ required: "Please enter application name" });
        this.description = ko.observable(data.description || "");
        this.instruction = ko.observable(data.instruction || "");
        this.submitTemplate = function () {
            this.name.validate();

            if (this.isValid()) {
                var formData = new FormData($("#templateForm")[0]);
                formData.append("Name", this.name());
                formData.append("Description", this.description());
                formData.append("Instruction", this.instruction());
                formData.append("Id", data.id);


                $.ajax({
                    url: '/ApplicationTypeTemplate/EditTemplate',
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        if (response.success) {
                            $("#refreshButton").trigger("click");
                            kendoWindow.close();
                        } else {
                            var errorsHtml = '<ul>';
                            if (typeof (response.errors) === "string") {
                                errorsHtml += '<li>' + response.errors + '</li>'
                            }
                            else {
                                $.each(response.errors, function (key, value) {
                                    errorsHtml += '<li>' + value + '</li>'; // Create list of errors
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

    //Phase Grid
    //var element = $("#grid").kendoGrid({
    //    dataSource: {
    //        transport: {
    //            read: function (options) {
    //                var page = options.data.page || 1;
    //                var pageSize = options.data.pageSize || 10;

    //                $.ajax({
    //                    url: "/Phase/GetAll",
    //                    type: "GET",
    //                    dataType: "json",
    //                    data: {
    //                        pageNumber: page,
    //                        pageSize: pageSize
    //                    },
    //                    success: function (data) {

    //                        options.success(data);
    //                    },
    //                    error: function (error) {
    //                        console.log(error);
    //                        alert('Error fetching data.');
    //                    }
    //                });
    //            },
    //            parameterMap: function (data, type) {
    //                if (type === "read") {
    //                    return kendo.stringify(data);
    //                }
    //                return data;
    //            }
    //        },
    //        pageSize: 10,
    //        serverPaging: true,
    //        schema: {
    //            total: "totalItems",
    //            data: "items",
    //            model: {
    //                fields: {
    //                    ordinal: { editable: false },
    //                    phase: { type: "string" },
    //                    steps: { type: "string" },
    //                }
    //            }
    //        },
    //    },
    //    pageable: {
    //        pageSize: 10,
    //        pageSizes: [10, 15, 20]
    //    },
    //    sortable: true,
    //    editable: false,
    //    toolbar: [
    //        {
    //            name: "create",
    //            text: "Add Phase",
    //            attributes: {
    //                "class": "k-button k-primary"
    //            }
    //        },
    //        {
    //            template: kendo.template($("#custom-toolbar-template").html())
    //        }
    //    ],
    //    columns: [

    //        { field: "ordinal", title: "Ordinal", width: "125px", hidden: true },

    //        { field: "phase", title: "Phase", width: "130px" },

    //        { field: "steps", title: "Steps", width: "130px" },
    //        {
    //            command: [
    //                { text: "Add Steps" },
    //                { text: "Edit" },
    //                { text: "Delete" }
    //            ],
    //            title: "Actions",
    //            width: "220px",
    //        }
    //    ],
    //    dataBound: function () {
    //        this.expandRow(this.tbody.find("tr.k-master-row").first());
    //        $('.k-grid-add').off("click");
    //        $('.k-grid-add').on("click", function () {
    //            $.ajax({
    //                url: "/Phase/AddPhase",
    //                type: 'GET',
    //                success: function (result) {
    //                    kendoWindow.content(result);
    //                    kendoWindow.center().open();
    //                },
    //                error: function (error) {
    //                    console.log(error);
    //                    alert('error fetching details');
    //                },
    //            });
    //        });
    //    },
    //    columns: [
    //        {
    //            field: "ordinal",
    //            title: "Ordinal",
    //            width: "110px"
    //        },
    //        {
    //            field: "phase",
    //            title: "Phase",
    //            width: "110px"
    //        },
    //        {
    //            field: "steps",
    //            title: "Steps",
    //            width: "110px"
    //        }
    //    ]
    //}).data("kendoGrid");


    ////Steps Grid
    //function detailInit(e) {
    //    $("<div/>").appendTo(e.detailCell).kendoGrid({
    //        dataSource: {
    //            type: "odata",
    //            transport: {
    //                read: "Steps/GetAll"
    //            },
    //            serverPaging: true,
    //            serverSorting: true,
    //            serverFiltering: true,
    //            pageSize: 10,
    //            //filter: { field: "EmployeeID", operator: "eq", value: e.data.EmployeeID }
    //        },
    //        scrollable: false,
    //        sortable: true,
    //        pageable: true,
    //        columns: [
    //            { field: "ordinal", width: "110px" },
    //            { field: "stepname", title: "Ship Country", width: "110px" },
    //            { field: "actions", title: "Ship Name", width: "300px" }
    //        ]
    //    });
    //}

    //// Event handlers for custom toolbar buttons
    //$(document).on("click", "#expand", function () {
    //    var grid = $("#grid").data("kendoGrid");
    //    $(".k-master-row").each(function (index) {
    //        grid.expandRow(this);
    //    });
    //});

    //$(document).on("click", "#collapse", function () {
    //    var grid = $("#grid").data("kendoGrid");
    //    $(".k-master-row").each(function (index) {
    //        grid.collapseRow(this);
    //    });
    //});
}))