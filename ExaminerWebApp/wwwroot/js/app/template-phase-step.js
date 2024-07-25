$(function () {
    EditTemplateViewPage();

    //EDIT TEMPLATE PAGE MAIN JS FILE //edit-template/edit.js
    function EditTemplateViewPage() {
        var stepGrid;
        var grid;
        function ApplicationTypeTemplateModel(data) {

            data = data || {};

            this.name = ko.observable(data.name || "").extend({ required: "Please enter application template name" });
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
                        url: '/TemplatePhaseStep/EditTemplate',
                        type: 'POST',
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: function (response) {
                            if (response.success) {
                                $("#refreshButton").trigger("click");
                                location.reload();
                                $('#add-phase').modal('hide');
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
            this.deleteTemplate = function () {
                if (confirm("Are you sure you want to delete this template?")) {

                    $.ajax({
                        url: "/ApplicationTypeTemplate/DeleteTemplate",
                        type: 'POST',
                        data: {
                            id: data.id,
                        },
                        success: function (result) {
                            alert("Application Type Template has been deleted successfully");
                            window.location.replace('/ApplicationTypeTemplate/Index');
                            $("#refreshButton").trigger("click");
                        },
                        error: function (error) {
                            console.log(error);
                            alert('error deleting details');
                        },
                    });
                }
            }

            this.isValid = function () {
                return !this.name.hasError();
            };
        }

        var viewModel = new ApplicationTypeTemplateModel(window.initialData);

        ko.applyBindings(viewModel, document.getElementById("templateForm"));

        var templateId = window.initialData.id;

        //Phase Grid
        grid = $("#grid").kendoGrid({
            dataSource: {
                transport: {
                    read: function (options) {

                        var gridData = {
                            Page: options.data.page || 1,
                            PageSize: options.data.pageSize || 10,
                            Sort: options.data.sort,
                            Filter: options.data.filter
                        };
                        var pager = JSON.stringify(gridData);
                        $.ajax({
                            url: "/TemplatePhaseStep/GetPhase?templateId=" + templateId,
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
                serverSorting: true,
                schema: {
                    total: "totalCount",
                    data: function (response) {
                        return response.items.map(function (item) {
                            return {
                                id: item.id,
                                templatePhaseId: item.templatePhaseId,
                                ordinal: item.ordinal,
                                phaseName: item.phaseName,
                                phaseId: item.phaseId,
                                steps: item.stepCount,
                            };
                        });
                    },

                    model: {
                        fields: {
                            id: { type: "number" },
                            ordinal: { type: "number" },
                            phaseName: { type: "string" },
                            steps: { type: "number" },
                        },
                    },
                },
            },
            detailInit: PhaseRow,
            pageable: {
                pageSize: 10,
                pageSizes: [10, 15, 20]
            },
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
            dataBound: function () {
                var grid = this;
                grid.tbody.find("tr").each(function () {
                    grid.expandRow($(this));
                });

                $('.k-grid-add').off("click");
                $('.k-grid-add').on("click", function () {
                    $.ajax({
                        url: "/TemplatePhaseStep/OpenTemplatePhase",
                        type: 'GET',
                        data: {
                            templateId: templateId
                        },
                        success: function (result) {
                            $('#displayModal').html(result);
                            $('#add-phase').modal('show');
                            AddPhaseViewModal();
                        },
                        error: function (error) {
                            console.log(error);
                            alert('error fetching details');
                        },
                    });
                });
            },
            columns: [
                { field: "ordinal", title: "Ordinal", width: "110px" },
                { field: "phaseName", title: "Phase" },
                { field: "steps", title: "Steps", width: "200px" },
                {
                    command: [
                        {
                            text: "Add Steps",
                            click: AddStep
                        },
                        {
                            name: "editPhase",
                            text: "",
                            click: EditPhase,
                            iconClass: ".k-i-pencil",
                            attributes: {
                                "class": "k-button k-primary"
                            },
                        },
                        {
                            name: "deletePhase",
                            text: "",
                            click: DeletePhase,
                            iconClass: ".k-i-trash",
                            attributes: {
                                "class": "k-button k-primary"
                            },
                        }
                    ],
                    title: "Actions",
                    width: "300px",
                },
            ]
        }).data("kendoGrid");

        $("#refreshButton").on("click", function () {
            grid.dataSource.read();
        });

        //Steps Grid    
        function PhaseRow(e) {
            var dataItem = e.data;
            stepGrid = $("<div class='innerGrid' />").appendTo(e.detailCell).kendoGrid({
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                url: "TemplatePhaseStep/GetPhaseStep",
                                type: "GET",
                                dataType: "json",
                                data: {
                                    phaseId: dataItem.phaseId,
                                    templateId: templateId
                                },
                                success: function (data) {
                                    options.success(data);
                                },
                                error: function (error) {
                                    alert('Error fetching data abcd.');
                                }
                            });
                        },
                    },
                    schema: {
                        data: function (response) {
                            return response.map(function (item) {
                                return {
                                    id: item.templatePhaseStepId,
                                    ordinal: item.ordinal,
                                    step: item.step.name,

                                };
                            });

                        },
                        total: function (response) {
                            return response.length;
                        },
                        model: {
                            fields: {
                                id: { type: "number" },
                                ordinal: { type: "number" },
                                step: { type: "string" },
                            },
                        },
                    },
                    pageSize: 10,
                },
                scrollable: true,
                sortable: false,
                pageable: false,
                editable: false,
                columns: [
                    { field: "id", title: "Step Id", width: "125px", hidden: true },
                    { field: "ordinal", title: "Ordinal", width: "125px" },
                    { field: "step", title: "Step Name" },
                    {
                        command: [

                            {
                                name: "editStep",
                                text: " ",
                                click: EditStep,
                                iconClass: ".k-i-pencil",
                                attributes: {
                                    "class": "k-button k-primary"
                                },
                            },
                            {
                                name: "deleteStep",
                                text: " ",
                                click: DeleteStep,
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
            });
        }

        // Event handlers for custom command buttons
        function AddStep(e) {
            var tr = $(e.target).closest("tr");
            var dataItem = $("#grid").data("kendoGrid").dataItem(tr);
            $.ajax({
                url: "/TemplatePhaseStep/AddTemplatePhaseStep",
                type: "GET",
                data: {
                    templatePhaseId: dataItem.id,
                },
                success: function (result) {
                    $('#displayModal').html(result);
                    $('#add-edit-step').modal('show');
                    AddEditStepViewModal();
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }

        function EditStep(e) {
            var tr = $(e.target).closest("tr");
            var dataItem = stepGrid.data("kendoGrid").dataItem(tr);

            $.ajax({
                url: "/TemplatePhaseStep/EditStep",
                type: "GET",
                data: {
                    id: dataItem.id, //temp phase step id
                },
                success: function (result) {
                    $('#displayModal').html(result);
                    $('#add-edit-step').modal('show');
                    AddEditStepViewModal();
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }

        function DeleteStep(e) {
            var tr = $(e.target).closest("tr");
            var dataItem = stepGrid.data("kendoGrid").dataItem(tr);
            if (confirm("Are you sure you want to delete this entry?")) {
                $.ajax({
                    url: "/TemplatePhaseStep/DeleteStep",
                    type: "GET",
                    data: {
                        id: dataItem.id, //temp phase step id
                    },
                    success: function (result) {
                        if (result && result.success) {
                            $("#refreshButton").trigger("click");
                            alert("Step has been deleted successfully");
                        }
                        else {
                            alert(`An error occured: ${result.errors}`);
                        }
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
        }

        function EditPhase(e) {
            var tr = $(e.target).closest("tr");
            var dataItem = $("#grid").data("kendoGrid").dataItem(tr);

            console.log(dataItem);

            $.ajax({
                url: "/TemplatePhaseStep/EditPhase",
                type: "GET",
                data: {
                    templatePhaseId: dataItem.id,
                    ordinal: dataItem.ordinal,
                    phaseName: dataItem.phaseName
                },
                success: function (result) {
                    $('#displayModal').html(result);
                    $('#edit-phase').modal('show');
                    EditPhaseModal();
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }

        function DeletePhase(e) {
            var tr = $(e.target).closest("tr");
            var dataItem = $("#grid").data("kendoGrid").dataItem(tr);

            if (confirm("Are you sure you want to delete this entry?")) {
                $.ajax({
                    url: "/TemplatePhaseStep/DeleteTemplatePhase",
                    type: 'POST',
                    data: {
                        templatePhaseId: dataItem.templatePhaseId,
                    },
                    success: function (result) {
                        $("#refreshButton").trigger("click");
                        alert("Phase has been deleted successfully");
                    },
                    error: function (error) {
                        console.log(error);
                        alert('error deleting details');
                    },
                });
            }
        }

        // Event handlers for custom toolbar buttons
        $(document).on("click", "#expand", function () {
            var grid = $("#grid").data("kendoGrid");
            $(".k-master-row").each(function (index) {
                grid.expandRow(this);
            });
        });

        $('#add-edit-step').on('hidden.bs.modal', function () {
            var element = $('#add-edit-step-form')[0];
            ko.cleanNode(element);
        });

        $("#back-btn").on("click", function () {
            window.location.replace("/ApplicationTypeTemplate/Index")
        })

        $(document).on("click", "#collapse", function () {
            var grid = $("#grid").data("kendoGrid");
            $(".k-master-row").each(function (index) {
                grid.collapseRow(this);
            });
        });

        function AddEditStepViewModal() {

            function AddEditStepModel(data) {
                data = data || {};

                this.stepid = ko.observable(data.stepId || null).extend({ required: "Please select step" });
                this.steptypeid = ko.observable(data.stepTypeId || "");
                this.ordinal = ko.observable(data.ordinal || "").extend({ required: "Please enter ordinal number" });
                this.stepinstruction = ko.observable(data.step?.instruction || "");

                this.formSubmit = function () {

                    this.stepid.validate();
                    this.ordinal.validate();

                    if (this.isValid()) {

                        var formdata = {
                            StepId: this.stepid(),
                            Ordinal: this.ordinal(),
                            Instruction: this.stepinstruction(),
                            TemplatePhaseId: data.templatePhaseId
                        };
                        if (data && data.id) {
                            formdata.Id = data.id;
                        }
                        var link = data && data.stepId ? '/TemplatePhaseStep/EditTemplatePhaseStep' : '/TemplatePhaseStep/AddTemplatePhaseStep'
                        $.ajax({
                            url: link,
                            type: 'POST',
                            contentType: "application/json",
                            data: JSON.stringify(formdata),
                            success: function (response) {
                                if (response.success) {
                                    $("#add-edit-step").modal('hide');
                                    $("#refreshButton").trigger("click");
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
                    return !this.stepid.hasError() && !this.ordinal.hasError();
                };
            }

            var viewModel = new AddEditStepModel(window.initialData);
            ko.applyBindings(viewModel, document.getElementById("add-edit-step-form"));

            var ordinalNumericTextBox = $("#ordinal").data("kendoNumericTextBox");

            ordinalNumericTextBox.value(viewModel.ordinal());

            viewModel.ordinal.subscribe(function (newValue) {
                ordinalNumericTextBox.value(newValue);
            });

            //step name binding
            $("#stepname").data("kendoDropDownList").value(viewModel.stepid());

            viewModel.stepid.subscribe(function (newValue) {
                $("#stepname").data("kendoDropDownList").value(newValue);
            });

            //Step type binding
            $("#steptypeid").data("kendoDropDownList").value(viewModel.steptypeid());

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

            viewModel.steptypeid.subscribe(function (newValue) {
                $("#steptypeid").data("kendoDropDownList").value(newValue);
            });

            //ordinal binding
            var ordinalNumericTextBox = $("#ordinal").data("kendoNumericTextBox");

            ordinalNumericTextBox.value(viewModel.ordinal());

            viewModel.ordinal.subscribe(function (newValue) {
                ordinalNumericTextBox.value(newValue);
            });
        }
    }

    //PHASE WiTH ORDINAL ADDITION MODAL IN EDIT TEMPLATE VIEW // window/add-phase.js
    function AddPhaseViewModal() {
        function AddPhaseModel(data) {
            data = data || {};
            this.phaseid = ko.observable(data.phaseid || "").extend({ required: "Please select a Phase" });
            this.ordinal = ko.observable(data.ordinal || null).extend({ required: "Please enter Ordinal Number" });

            this.submitForm = function () {
                this.phaseid.validate();
                this.ordinal.validate();

                if (this.isValid()) {
                    var phasegrid = $("#phaseGrid").data("kendoGrid");
                    var isValidGrid = true;

                    var gridData = phasegrid.dataSource
                        .view()
                        .map(function (item) {
                            var ordinal = phasegrid.tbody.find('tr[data-uid="' + item.uid + '"]').find('input[name="ordinal"]').data("kendoNumericTextBox").value();
                            var isChecked = phasegrid.tbody.find('tr[data-uid="' + item.uid + '"]').find('input[type="checkbox"]').is(":checked");

                            if (isChecked && !ordinal) {
                                isValidGrid = false;
                                var ordinalBox = phasegrid.tbody.find('tr[data-uid="' + item.uid + '"]').find('input[name="ordinal"]');
                                ordinalBox.addClass("k-invalid");
                                $("<span class='k-input k-textbox k-input-solid k-input-md k-rounded-md k-invalid'>Ordinal is required</span>").insertAfter(ordinalBox);
                            }

                            return {
                                Id: item.id,
                                TemplatePhaseId: item.templatePhaseId,
                                Name: item.name,
                                Ordinal: ordinal,
                                IsInTemplatePhaseSteps: isChecked
                            };
                        });

                    if (!isValidGrid) return;

                    var formdata = {
                        PhaseId: this.phaseid(),
                        Ordinal: this.ordinal(),
                        TemplateId: data.templateId,
                        StepCount: gridData.length,
                        TemplatePhaseSteps: gridData
                            .filter(item => item.IsInTemplatePhaseSteps)
                            .map(item => ({
                                Id: item.Id,
                                TemplatePhaseId: item.templatePhaseId,
                                TemplatePhaseStepId: item.templatePhaseStepId,
                                Name: item.Name,
                                Ordinal: item.Ordinal,
                                IsInTemplatePhaseSteps: item.IsInTemplatePhaseSteps
                            }))
                    };
                    $.ajax({
                        url: '/TemplatePhaseStep/AddTemplatePhase',
                        type: 'POST',
                        contentType: "application/json",
                        data: JSON.stringify(formdata),
                        success: function (response) {
                            if (response.success) {
                                $("#refreshButton").trigger("click");
                                $('#add-phase').modal('hide');
                            } else {
                                var errorsHtml = '<ul>';
                                if (typeof (response.errors) === "string") {
                                    errorsHtml += '<li>' + response.errors + '</li>';
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
                }
            };

            this.isValid = function () {
                return !this.phaseid.hasError() && !this.ordinal.hasError();
            };
        }

        var viewModel = new AddPhaseModel(window.initialData);

        ko.applyBindings(viewModel, document.getElementById("addPhase"));

        var ordinalNumericTextBox = $("#ordinal").data("kendoNumericTextBox");

        ordinalNumericTextBox.value(viewModel.ordinal());

        viewModel.ordinal.subscribe(function (newValue) {
            ordinalNumericTextBox.value(newValue);
        });

        function GetPhaseSteps(e) {
            var phaseId = e.target.value;

            // Initialize the grid if it does not exist
            if (!$("#phaseGrid").data("kendoGrid")) {
                initializeGrid(phaseId);
            }
            else {
                phasegrid = $("#phaseGrid").data("kendoGrid");
                phasegrid.setDataSource(new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            $.ajax({
                                url: "/TemplatePhaseStep/GetPhaseSteps",
                                type: "GET",
                                dataType: "json",
                                data: {
                                    templateId: window.initialData.templateId,
                                    phaseId
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
                        update: function (options) {
                            $.ajax({
                                url: "/TemplatePhaseStep/UpdateOrdinal",
                                type: "POST",
                                dataType: "json",
                                data: {
                                    templateId: window.initialData.templateId,
                                    phaseId: options.data.id,
                                    ordinal: options.data.ordinal
                                },
                                success: function (response) {
                                    if (response.success) {
                                        $("#refreshButton").trigger("click");
                                    }
                                },
                                error: function (error) {
                                    console.log(error);
                                    alert('Error updating data.');
                                }
                            });
                            options.success(options.data);
                        },
                        parameterMap: function (data, type) {
                            if (type === "read") {
                                return kendo.stringify(data);
                            }
                            return data;
                        }
                    },
                    schema: {
                        data: function (response) {
                            return response.map(function (item) {
                                return {
                                    id: item.step.id,
                                    name: item.step.name,
                                    ordinal: item.ordinal,
                                    isInTemplatePhaseSteps: item.isInTemplatePhaseSteps
                                };
                            });
                        },
                        total: function (response) {
                            return response.length;
                        },
                        model: {
                            id: "id",
                            fields: {
                                id: { type: "number" },
                                name: { type: "string" },
                                ordinal: { type: "number" },
                                isInTemplatePhaseSteps: { type: "boolean" }
                            }
                        }
                    }
                }));
            }
        }

        function initializeGrid(phaseId) {
            $("#phaseGrid").kendoGrid({
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                url: "/TemplatePhaseStep/GetPhaseSteps",
                                type: "GET",
                                dataType: "json",
                                data: {
                                    templateId: window.initialData.templateId,
                                    phaseId
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
                        update: function (options) {
                            $.ajax({
                                url: "/TemplatePhaseStep/UpdateOrdinal",
                                type: "POST",
                                dataType: "json",
                                data: {
                                    templateId: window.initialData.templateId,
                                    phaseId: options.data.id,
                                    ordinal: options.data.ordinal
                                },
                                success: function (response) {
                                    if (response.success) {
                                        $("#refreshButton").trigger("click");
                                    }
                                },
                                error: function (error) {
                                    console.log(error);
                                    alert('Error updating data.');
                                }
                            });
                            options.success(options.data);
                        },
                        parameterMap: function (data, type) {
                            if (type === "read") {
                                return kendo.stringify(data);
                            }
                            return data;
                        }
                    },
                    schema: {
                        data: function (response) {
                            return response.map(function (item) {
                                return {
                                    id: item.step.id,
                                    name: item.step.name,
                                    ordinal: item.ordinal,
                                    isInTemplatePhaseSteps: item.isInTemplatePhaseSteps
                                };
                            });
                        },
                        total: function (response) {
                            return response.length;
                        },
                        model: {
                            id: "id",
                            fields: {
                                id: { type: "number" },
                                name: { type: "string", editable: false },
                                ordinal: { type: "number", validation: { min: 1, required: true } },
                                isInTemplatePhaseSteps: { type: "boolean" }
                            }
                        }
                    }
                },
                pageable: false,
                sortable: false,
                scrollable: true,
                editable: "inline",
                height: 0,
                columns: [
                    {
                        field: "isInTemplatePhaseSteps",
                        selectable: true,
                        template: '<input type="checkbox" #= isInTemplatePhaseSteps ? "checked=checked" : "" # disabled="disabled" />',
                        width: "40px"
                    },
                    { field: "name", title: "Step", width: "150px" },
                    {
                        field: "ordinal",
                        title: "Ordinal",
                        width: "110px",
                        template: function (dataItem) {
                            return '<input class="k-input k-textbox" name="ordinal" type="number" value="' + dataItem.ordinal + '" min="1" required />';
                        }
                    }
                ],
                dataBound: function (e) {
                    var grid = e.sender;
                    var rows = grid.tbody.find("tr");
                    rows.each(function () {
                        var row = $(this);
                        var dataItem = grid.dataItem(row);
                        row.find('input[name="ordinal"]').kendoNumericTextBox({
                            format: "n0",
                            decimals: 0,
                            min: 1,
                        }).data("kendoNumericTextBox").value(dataItem.ordinal);

                        // Validation logic
                        var isChecked = row.find('input[type="checkbox"]').is(":checked");
                        var ordinalValue = row.find('input[name="ordinal"]').data("kendoNumericTextBox").value();
                        if (isChecked && !ordinalValue) {
                            row.find('input[name="ordinal"]').addClass("k-invalid");
                            $("<span class='k-invalid-msg'>Ordinal is required</span>").insertAfter(row.find('input[name="ordinal"]'));
                        }
                    });
                }
            });
        }

        $("#phaseid").on("change", function (e) {
            GetPhaseSteps(e);
        });
    }

    //EDIT PHASE ORDINAL MODAL IN EDIT TEMPLATE VIEW // window/edit-phase.js
    function EditPhaseModal() {
        function EditPhaseModel(data) {
            data = data || {};

            this.phasename = ko.observable(data.phaseName || "");
            this.id = data.id;
            this.ordinal = ko.observable(data.ordinal || "").extend({ required: "Please enter ordinal number" });
            this.isValid = function () {
                return !this.ordinal.hasError();
            };
            this.submitForm = function () {
                this.ordinal.validate();
                if (this.isValid()) {
                    var formdata = {
                        Id: this.id,
                        Ordinal: this.ordinal(),
                    };

                    $.ajax({
                        url: '/TemplatePhaseStep/EditTemplatePhase',
                        type: 'POST',
                        contentType: "application/json",
                        data: JSON.stringify(formdata),
                        success: function (response) {
                            if (response.success) {
                                $("#refreshButton").trigger("click");
                                $('#edit-phase').modal('hide');
                            } else {
                                var errorsHtml = '<ul>';
                                if (typeof (response.errors) === "string") {
                                    errorsHtml += '<li>' + response.errors + '</li>';
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
                }
                this.isValid = function () {
                    return !this.ordinal.hasError();
                };
            }
        }

        var viewModel = new EditPhaseModel(window.initialData);
        ko.applyBindings(viewModel, document.getElementById("editPhase"));
    }
})