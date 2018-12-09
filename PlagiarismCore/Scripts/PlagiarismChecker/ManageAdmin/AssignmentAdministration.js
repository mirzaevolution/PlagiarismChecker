let Ajax = {
    Init: function () {
        this.LoadAssignments();
        this.LoadSubmittedAssignment();
    },
    LoadAssignments: function () {
        if ($.fn.DataTable.isDataTable("#AssignmentTable")) {
            $('#AssignmentTable').DataTable().clear().destroy();
        }
        var url = "/CoreAPI/GetAllAssignments";
        $("#AssignmentTable").DataTable({
            ajax: url,
            columns: [
                { data: "AssignmentName" },
                {
                    data: "Id",
                    render: function (data, type,row) {
                        if (type === 'display') {
                            data = "<button class='btn btn-info' onclick='Buttons.ShowEdit(\"" + data + "\",\"" + row.AssignmentName + "\")'>Edit</button>";
                        }
                        return data;
                    }
                }
            ],
            searching: false,
            search: false,
            paging: false,
            info: false,
            ordering: false
        })
    },
    LoadSubmittedAssignment: function () {
        if ($.fn.DataTable.isDataTable("#SubmittedAssignmentTable")) {
            $('#SubmittedAssignmentTable').DataTable().clear().destroy();
        }
        var id = $("#ID").val();
        var url = "/CoreAPI/GetAllSubmittedAssignments";
        $("#SubmittedAssignmentTable").DataTable({
            ajax: url,
            columns: [
                { data: "StudentName" },
                { data: "StudentClass" },
                { data: "SubjectName" },
                { data: "Title" },
                { data: "Counter" },
                {
                    data: "UploadedFilePath",
                    render: function (data, type, row) {
                        if (type === 'display') {
                            data = "<a href='" + data + "' target='_blank'>Download File</a>";
                        }
                        return data;
                    } },
                { data: "PercentageInteger" },
                { data: "Description" },
                { data: "Status" },
                { data: "Score" },
                { data: "ScoreStatus" },
                {
                    data: "SubmissionDate",
                    render: function (data, type) {
                        if (type === 'display')
                            data = moment(data).format('LLL');
                        return data;
                    }
                },
                { data: "IsChecked" },

            ],
            info: false,
            paging: true,
            scrollX: true
        });

    }
}
let Page = {
    Init: function () {
        this.CheckMessage();
        //this.AssignmentTable();
    },
    CheckMessage: function () {
        var message = $("#Message").val();
        if (message && message !== "") {
            alert(message);
        }
    },
    AssignmentTable: function () {
        if ($.fn.DataTable.isDataTable("#AssignmentTable")) {
            $('#AssignmentTable').DataTable().clear().destroy();
        }
        var url = "/CoreAPI/GetStudentAssignments/" + $("#ID").val();
        $("#AssignmentTable").DataTable({
            ajax: url,
            columns: [
                { "data": "AssignmentName" },
                {
                    "data": "Id",
                    "render": function (data, type, row, meta) {
                        if (type === "display") {
                            data = "<button onclick='Buttons.EditAssignment(\"" + data + "\");' class='btn btn-danger btn-remove-assignment' data-id='" + data + "'>Delete</button>";
                        }
                        return data;
                    }
                }
            ],
            searching: false,
            search: false,
            paging: true,
            info: false,
            ordering: false,
            scrollY: "200px",
            scrollCollapse: true,
        });
    }
}
let Buttons = {
    SaveEdit: function () {
        var assignmentNameEdit = $("#AssignmentNameEdit").val();
        var assignmentId = $("#AssignmentIdHidden").val();

        if (assignmentNameEdit.trim() === '') {
            alert('Assignment name cannot be empty');
        } else {

            $.ajax({
                url: "/CoreApi/EditAssignment",
                method: "POST",
                data: {
                    Id: assignmentId,
                    AssignmentName: assignmentNameEdit,
                },
                beforeSend: function () {
                    $("#ButtonAddNewAssignment").prop("disabled", true);

                },
                success: function (response) {
                    $("#ModalEditAssignment").modal("hide");
                    Ajax.LoadAssignments();
                    if (response.Success) {
                        alert("Data has been updated successfully");
                    } else {
                        alert("An error occured when updating data");
                        console.log(response.Errors);
                    }

                },
                error: function () {
                    alert("An error occured while updating data in the server");
                }
            })
        }

    },
    ShowEdit: function (id, data) {
        $("#AssignmentIdHidden").val(id);
        $("#AssignmentNameEdit").val(data);
        $("#ModalEditAssignment").modal({
            backdrop: "static"
        });
    },
    RemoveAssignment: function () {
        var assignmentId = $("#AssignmentIdHidden").val();

        $.ajax({
            url: "/CoreApi/DeleteAssignment",
            method: "POST",
            data: {
                Id: assignmentId,
            },
            success: function (response) {
                $("#ModalEditAssignment").modal("hide");
                Ajax.LoadAssignments();
                if (response.Success) {
                    alert("Data has been removed successfully");
                } else {
                    alert("An error occured when removing data");
                    console.log(response.Errors);
                }

            },
            error: function () {
                alert("An error occured while removing data in the server");
            }
        })

    }
}
$(document).ready(function () {
    //Buttons.Init();
    Ajax.Init();
    Page.Init();
});