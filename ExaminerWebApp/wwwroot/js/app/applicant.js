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
                        url: "/Applicant/GetAll",
                        type: "POST",
                        contentType: "application/json",
                        dataType: "json",
                        data: pager,
                        success: function (data) {
                            console.log(data);
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
                data: function (response) {
                    return response.items.map(function (item) {
                        return {
                            id: item.id,
                            firstname: item.firstName,
                            lastname: item.lastName,
                            dateofbirth: item.dateOfBirth,
                            phone: item.phone,
                            email: item.email,
                            applicantTypeName: item.applicantType,
                            filepath: item.filePath,
                        };
                    });
                },
                model: {
                    fields: {
                        id: { editable: false },
                        firstname: { type: "string" },
                        lastname: { type: "string" },
                        dateofbirth: { type: "date" },
                        phone: { type: "string" },
                        email: { type: "string" },
                        applicantTypeName: { type: "string" }
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

            { field: "id", title: "Application ID", width: "125px", hidden: true },

            { field: "firstname", title: "First Name", width: "130px" },

            { field: "lastname", title: "Last Name", width: "130px" },

            { field: "dateofbirth", title: "Date Of Birth", width: "130px", format: "{0:dd/MM/yyyy}" },

            { field: "phone", title: "Phone Number", width: "150px" },

            { field: "email", title: "Email", width: "250px" },

            { field: "applicantTypeName", title: "Application Type", width: "200px" },
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
                    url: "/Applicant/ApplicantForm",
                    type: 'GET',
                    success: function (result) {
                        $('#displayModal').html(result);
                        $('#applicationForm').modal('show');
                        ApplicantModal();
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
            url: "/Applicant/GetApplicant",
            type: 'GET',
            data: {
                Id: dataItem.id
            },
            success: function (result) {
                $('#displayModal').html(result);
                $('#applicationForm').modal('show');
                ApplicantModal();
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
        var filePath = `/UploadedFiles/${dataItem.filepath}`;
        window.open(filePath, '_blank');
    }

    function DeleteEntry(e) {
        e.preventDefault();
        var tr = $(e.target).closest("tr");
        var dataItem = $("#grid").data("kendoGrid").dataItem(tr);
        if (confirm("Are you sure you want to delete this entry?")) {
            $.ajax({
                url: "/Applicant/DeleteApplicant",
                type: 'POST',
                data: {
                    id: dataItem.id
                },
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

    function ApplicantModal() {
        function ApplicantViewModel(data) {
            data = data || {};

            // Define observables for each form field with validation
            this.settingid = ko.observable(data.settingid || "").extend({ required: "Please select application type" });
            this.firstName = ko.observable(data.firstname || "").extend({
                required: "First name is required",
                noNumbers: true
            });
            this.lastName = ko.observable(data.lastname || "").extend({
                required: "Last name is required",
                noNumbers: true
            });
            this.dateOfBirth = ko.observable(formatDate(data.dateofbirth) || "").extend({ required: "Date of birth is required" });
            this.phoneNumber = ko.observable(data.phone || null).extend({
                required: "Phone number is required",
                pattern: {
                    params: patternsRegex.phone,
                    message: "Entered phone format is not valid."
                }
            });
            this.email = ko.observable(data.email || "").extend({
                required: "Email is required",
                pattern: {
                    params: patternsRegex.email,
                    message: "Entered email format is not valid",
                }
            });

            // this.applicanttypeid = ko.observable(data.applicanttypeid || "").extend({ required: "Please select application type" });

            this.formFile = ko.observable(null);

            this.viewFile = function () {
                if (data.filepath === null || data.filepath === "") {
                    alert("No file found!");
                    return;
                }
                var filePath = `UploadedFiles/${data.filepath}`;
                window.open(filePath, '_blank');
            }

            this.submitForm = function () {
                // Validate all observables
                this.firstName.validate();
                this.lastName.validate();
                this.dateOfBirth.validate();
                this.phoneNumber.validate();
                this.email.validate();
                this.settingid.validate();

                if (this.isValid()) {
                    var formData = new FormData($("#addApplicant")[0]);
                    formData.append("Firstname", this.firstName());
                    formData.append("Lastname", this.lastName());
                    formData.append("Dateofbirth", this.dateOfBirth());
                    formData.append("Phone", this.phoneNumber());
                    formData.append("Email", this.email());
                    formData.append("Settingid", this.settingid());

                    if (this.formFile()) {
                        formData.append("FormFile", this.formFile());
                    }
                    if (data.id) {
                        formData.append("Id", data.id);
                    }
                    console.log(formData);
                    var link = data && data.id ? '/Applicant/EditApplicant' : '/Applicant/AddApplicant';

                    $.ajax({
                        url: link,
                        type: 'POST',
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: function (response) {
                            if (response.success) {
                                $("#refreshButton").trigger("click");
                                $('#applicationForm').modal('hide');
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
                } else {
                    // Scroll to the first invalid field
                    var firstInvalidField = $(".has-error").first();
                    if (firstInvalidField.length) {
                        $("html, body").animate({
                            scrollTop: firstInvalidField.offset().top - 100
                        }, 500);
                        firstInvalidField.find("input, select, textarea").trigger("focus");
                    }
                }
            };

            String.prototype.camelCase = function () {
                return this.charAt(0).toLowerCase() + this.slice(1);
            };

            // Helper function to check if all fields are valid
            this.isValid = function () {
                return !this.firstName.hasError() && !this.lastName.hasError() && !this.dateOfBirth.hasError()
                    && !this.phoneNumber.hasError() && !this.email.hasError() && !this.settingid.hasError();
            };
        }

        $('#applicationForm').on('shown.bs.modal', function () {
            var viewModel = new ApplicantViewModel(window.initialData);
            ko.applyBindings(viewModel, $("#addApplicant")[0]);
        });
    }
});