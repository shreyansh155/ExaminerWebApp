define(function (require) {
    'use strict';
 
    var EmailTemplateGrid = function () {
        var self = this;
        self.emailTemplateGrid = {
            model: {},
            loadGrid: function () {
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
                                    url: "/EmailTemplate/GetAll",
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
                            data: "items",
                            model: {
                                fields: {
                                    id: { editable: false },
                                    name: { type: "string" },
                                    isDefault: { type: "boolean", editable: false },
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
                    toolbar: [{ name: "create", text: "Add" }],
                    columns: [

                        { field: "id", title: "EmailTemplate ID", width: "125px", hidden: true },

                        { field: "name", title: "Name" },

                        {
                            field: "isDefault",
                            title: "Default",
                            width: 110,
                            template: '<input type="checkbox" #= isDefault ? \'checked="checked"\' : "" # disabled class="chkbx k-checkbox k-checkbox-md k-rounded-md" />',
                            attributes: { class: "k-text-center" }
                        },
                        {
                            command: [
                                { text: "Edit", click: self.editEntry.bind(self) },
                                { text: "Delete", click: self.deleteEntry.bind(self) }
                            ],
                            title: "Actions",
                            width: "220px",
                        }
                    ],
                    dataBound: function (e) {
                        $('.k-grid-add').off("click");
                        $('.k-grid-add').on("click", function () {
                            $.ajax({
                                url: "/EmailTemplate/EmailTemplate",
                                type: 'GET',
                                success: function (response) {
                                    if (response.success) {
                                        window.location.href = "/EmailTemplate/Create"
                                    }
                                },
                                error: function (error) {
                                    console.log(error);
                                    alert('error fetching details');
                                },
                            });
                        });
                    },


                }).data("kendoGrid");
                $("#grid .k-grid-content").on("change", "input.chkbx", function (e) {
                    var grid = $("#grid").data("kendoGrid"),
                        dataItem = grid.dataItem($(e.target).closest("tr"));

                    dataItem.set("IsDefault", this.checked);
                });
            },
        }
        self.editEntry = function (e) {
            e.preventDefault();
            var tr = $(e.target).closest("tr");
            var dataItem = $("#grid").data("kendoGrid").dataItem(tr);

            $.ajax({
                url: "/EmailTemplate/EditTemplate",
                type: 'GET',
                data: {
                    id: dataItem.id
                },
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
                    alert('error fetching details');
                },
            });
        }
        self.deleteEntry = function (e) {
            e.preventDefault();

            var tr = $(e.target).closest("tr");
            var dataItem = $("#grid").data("kendoGrid").dataItem(tr);

            if (confirm("Are you sure you want to delete this entry?")) {
                $.ajax({
                    url: "/EmailTemplate/Delete",
                    type: 'POST',
                    data: {
                        id: dataItem.id
                    },
                    success: function (result) {
                        if (result.success) {
                            alert("EmailTemplate has been deleted successfully");
                            $("#grid").data("kendoGrid").dataSource.read();
                        }
                        else {
                            alert(result.errors);
                        }
                    },
                    error: function (error) {
                        console.log(error);
                        alert('error deleting details');
                    },
                });
            }
        }
        self.init = function () {
            self.emailTemplateGrid.loadGrid();
        }
    }
    return EmailTemplateGrid;
});