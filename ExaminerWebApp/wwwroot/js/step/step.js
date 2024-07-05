$(function () {

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
    function PhaseViewModel(data) {

        data = data || {};

        this.phasename = ko.observable(data.phasename || "").extend({ required: "Please enter application name" });
        this.phasedescription = ko.observable(data.phasedescription || "");
        this.submitForm = function () {
            this.phasename.validate();

            if (this.isValid()) {
                var formData = new FormData($("#phaseForm")[0]);
                formData.append("Name", this.phasename());
                formData.append("Description", this.phasedescription());
                if (data.id) {
                    formData.append("Id", data.id);
                }

                var link = data && data.id ? '/Phase/EditPhase' : '/Phase/CreatePhase';

                $.ajax({
                    url: link,
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
            return !this.phasename.hasError();
        };
    }
    var viewModel = new PhaseViewModel(window.initialData);
    ko.applyBindings(viewModel, document.getElementById("phaseForm"));

    var grid = $("#stepGrid").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax({
                        url: "/Step/GetAll",
                        type: "GET",
                        dataType: "json",
                        data: {

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
 
    $("#refreshButton").on("click", function () {
        grid.dataSource.read();
    });
});
