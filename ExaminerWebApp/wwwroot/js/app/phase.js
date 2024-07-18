$(function () {
    function textBox(container, options) {
        $('<textarea rows="3" id="description" name="description" data-bind="value:' + options.field + '"></textarea>')
            .appendTo(container);
    }
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
                template: kendo.template($("#custom-toolbar-template").html())
            }
        ],
        columns: [
            { field: "phaseId", title: "Phase ID", width: "125px", hidden: true },
            { field: "ordinal", title: "Ordinal", width: "125px", hidden: true },
            { field: "name", title: "Phase", width: "300px" },
            { field: "description", title: "Description", editor: textBox },
            {
                command: [
                    { text: "Add Steps", click: AddSteps },
                    { text: "Edit", click: EditPhase },
                    { text: "Delete", click: DeletePhase }
                ],
                title: "Actions",
                width: "300px",
            }
        ],
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
                $('#displayModal').html(result);
                $('#create-phase').modal('show');
                CreatePhaseModal();
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    function EditPhase(e) {
        var tr = $(e.target).closest("tr");
        var dataItem = $("#grid").data("kendoGrid").dataItem(tr);
        $.ajax({
            url: "/Phase/Edit",
            type: "GET",
            data: {
                id: dataItem.phaseId,
            },
            success: function (result) {
                $('#displayModal').html(result);
                $('#create-phase').modal('show');
                CreatePhaseModal();
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    function DeletePhase(e) {
        var tr = $(e.target).closest("tr");
        var dataItem = $("#grid").data("kendoGrid").dataItem(tr);
        if (confirm("Are you sure?")) {
            $.ajax({
                url: "/Phase/Delete",
                type: "POST",
                data: {
                    id: dataItem.phaseId,
                },
                success: function (result) {
                    $("#refreshButton").trigger("click");
                    alert("Phase has been deleted successfully");
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    }
    //Steps Grid
    function PhaseRow(e) {
        var dataItem = e.data;

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
                                options.success(data);
                            },
                            error: function (error) {
                                console.log(error);
                                alert('Error fetching data.');
                            }
                        });
                    },
                },
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            id: { editable: false, nullable: true },
                            name: { type: "string" },
                            description: { type: "string" },
                            instruction: { type: "string", encoded: false },
                            typeId: { type: "number" },
                            stepType: { type: "string" }
                        }
                    }
                },
                pageSize: 10,
            },
            scrollable: true,
            sortable: true,
            pageable: false,
            editable: false,
            columns: [
                { field: "id", title: "Step Id", width: "125px", hidden: true },
                { field: "name", title: "Step Name", width: "130px" },
                { field: "description", title: "Description", width: "130px" },
                {
                    field: "instruction",
                    title: "Instruction",
                    width: "130px",
                    encoded: false,
                    template: function (dataItem) {
                        return dataItem.instruction;
                    },
                },
                { field: "stepType", title: "Step Type", width: "150px" },
            ],
        });
    }

    // Event handlers for custom toolbar buttons
    $(document).on("click", "#add", function () {
        $.ajax({
            url: "/Phase/AddPhase",
            type: 'GET',
            success: function (result) {
                $('#displayModal').html(result);
                $('#create-phase').modal('show');
                CreatePhaseModal();
            },
            error: function (error) {
                console.log(error);
                alert('error fetching details');
            },
        });
    });

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
    $("#refreshButton").on("click", function () {
        grid.dataSource.read();
    });

    //modal binding
    function CreatePhaseModal() {
        ko.extenders.required = function (target, overrideMessage) {
            target.hasError = ko.observable(false);
            target.validationMessage = ko.observable();
            target.validationTriggered = ko.observable(false);

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

        function PhaseViewModel(data) {

            data = data || {};

            this.phasename = ko.observable(data.name || "").extend({ required: "Please enter Phase name" });
            this.phasedescription = ko.observable(data.description || "");

            this.submitForm = function () {
                this.phasename.validate();

                if (this.isValid()) {
                    var formData = new FormData($("#phaseForm")[0]);
                    formData.append("Name", this.phasename());
                    formData.append("Description", this.phasedescription());
                    if (data.phaseId) {
                        formData.append("PhaseId", data.phaseId);
                    }

                    var link = data && data.phaseId ? '/Phase/EditPhase' : '/Phase/CreatePhase';

                    $.ajax({
                        url: link,
                        type: 'POST',
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: function (response) {
                            if (response.success) {
                                $("#refreshButton").trigger("click");
                                $('#create-phase').modal('hide');
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
                return !this.phasename.hasError();
            };
        }

        var viewModel = new PhaseViewModel(window.initialData);

        ko.applyBindings(viewModel, document.getElementById("phaseForm"));
    }
});
