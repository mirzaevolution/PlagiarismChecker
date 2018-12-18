﻿let Ajax = {
    Init: function () {
        this.LoadAssignments();
        this.LoadSubmittedAssignment();
    },
    
    LoadAssignments: function () {
        if ($.fn.DataTable.isDataTable("#AssignmentTable")) {
            $('#AssignmentTable').DataTable().clear().destroy();
        }
        var url = "/CoreAPI/GetSubjectsByTeacherRole";
        $("#AssignmentTable").DataTable({
            ajax: url,
            columns: [
                { data: "AssignmentName" },
                {
                    data: "Id",
                    render: function (data, type,row) {
                        if (type === 'display') {
                            data =  "<button class='btn btn-primary' onclick='Buttons.ShowEdit(\"" + data + "\",\"" + row.AssignmentName + "\")'>Edit</button>" +
                                "   <button class='btn btn-success' onclick='Buttons.ShowStudents(\"" + data + "\",\"" + row.AssignmentName + "\")'>Students</button>";
                        }
                        return data;
                    }
                }
            ],

            searching: true,
            search: true,
            paging: true,
            info: false,
            ordering: true,
            scrollY: "400px",
            scrollCollapse: true
        })
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
            MessageBox.Show(message);
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
            MessageBox.Show('Assignment name cannot be empty');
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
                        MessageBox.Show("Data has been updated successfully");
                    } else {
                        MessageBox.Show("An error occured when updating data");
                        console.log(response.Errors);
                    }

                },
                error: function () {
                    MessageBox.Show("An error occured while updating data in the server");
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
    ShowStudents: function (id, name) {
        window.location.href = "/ManageTeacher/AssignmentDetail/" + id + "?subjectName=" + name;
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
                    MessageBox.Show("Data has been removed successfully");
                } else {
                    MessageBox.Show("An error occured when removing data");
                    console.log(response.Errors);
                }

            },
            error: function () {
                MessageBox.Show("An error occured while removing data in the server");
            }
        })

    }
}
$(document).ready(function () {
    //Buttons.Init();
    Ajax.Init();
    Page.Init();
});