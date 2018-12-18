let Ajax = {
    Init: function () {
        this.LoadClasses();
        this.LoadAssignments();
        this.LoadSubmittedAssignment();
    },
    LoadClasses: function () {
        var url = "/CoreAPI/GetClasses";
        $.ajax({
            url: url,
            beforeSend: function () {
                console.log(".....Requesting data from " + url);
            },
            success: function (response) {
                console.log(".....Data successfully retrieved from " + url);
                $("#ClassID").empty();
                if (response.length > 0) {
                    $.each(response, (index, value) => {
                        $("#ClassID").append("<option value='" + value.ID + "'>" + value.Text + "</option>");
                    });
                    $("#ClassID").val($("#ClassIDHidden").val());
                }

            },
            error: function () {
                console.log(".....Data failed to receive from " + url);
            }
        });
    },
    LoadAssignments: function () {
        var userId = $("#ID").val();
        var url = "/CoreAPI/GetAssignments/" + userId;
        $.ajax({
            url: url,
            beforeSend: function () {
                console.log(".....Requesting data from " + url);
            },
            success: function (response) {
                console.log(".....Data successfully retrieved from " + url);
                $("#AssignmentSelect").empty();
                if (response.length > 0) {
                    
                    $.each(response, (index, value) => {
                        $("#AssignmentSelect").append("<option id='" + value.Id + "' value='" + value.Id + "'>" + value.AssignmentName + "</option>");
                    });

                }

            },
            error: function () {
                console.log(".....Data failed to receive from " + url);
            }
        });
    },
    LoadSubmittedAssignment: function () {
        var id = $("#ID").val();
        var url = "/CoreAPI/GetStudentSubmittedAssignments/" + id;
        if ($.fn.DataTable.isDataTable("#SubmittedAssignmentTable")) {
            $('#SubmittedAssignmentTable').DataTable().clear().destroy();
        }
        $("#SubmittedAssignmentTable").DataTable({
            ajax: url,
            columns: [
                { data: "AssignmentName" },
                { data: "Title" },
                { data: "Description" },
                { data: "PercentageInteger" },
                { data: "Status" },
                {
                    data: "SubmissionDate",
                    render: function (data, type) {
                        if (type === 'display') 
                            data = moment(data).format('LLL');
                        return data;
                    }
                },
                {
                    data: "UploadedFilePath",
                    render: function (data, type, row) {
                        if (type === 'display') {
                            data = "<a href='" + data + "' target='_blank'>Download File</a>";
                        }
                        return data;
                    }
                },
                { data: "Score" },
                { data: "ScoreStatus" },
                { data: "Teacher" },
                { data: "Note" },
                {
                    data: "Id",
                    render: function (data, type, row) {
                        if (type === 'display' && row.IsChecked !== 'In Review') {
                            data = '<button class="btn btn-primary" onclick="Buttons.ShowEditScore(\'' + data + '\',\'' + row.Score + '\')"> Edit Score</button>';
                        }
                        else {
                            data = "";
                        }
                        return data;
                    }
                }

            ],
            columnDefs: [
                    {
                        render: function (data, type, full, meta) {
                            return "<div class='text-wrap width-250'>" + data + "</div>";
                        },
                        targets: 9
                    }
            ],
            scrollX: true,
            info: false,
            paging: false
        });

    }
}
let Page = {
    Init: function () {
        this.CheckMessage();
        this.AssignmentTable();
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
                            data = "<button onclick='Buttons.DeleteAssignment(\"" + data + "\");' class='btn btn-danger btn-remove-assignment' data-id='" + data + "'>Delete</button>";
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
        });
    }
}
let Buttons = {
    Init: function () {
        $("#ButtonDelete").click(function () {
            $("#FormDelete").submit();
        });
        $("#ButtonAddNewAssignment").click(function () {
            var userId = $("#ID").val();
            var assignmentId = $("#AssignmentSelect").val();
            $.ajax({
                url: "/CoreApi/PostStudentAssignment",
                method: "POST",
                data: {
                    userId: userId,
                    assignmentId: assignmentId
                },
                beforeSend: function () {
                    $("#ButtonAddNewAssignment").prop("disabled", true);

                },
                success: function (response) {
                    $("#ButtonAddNewAssignment").prop("disabled", false);

                    if (response.Success) {
                        Ajax.LoadAssignments();
                        Page.AssignmentTable();

                        MessageBox.Show("Assignment has been added to student list");
                    } else {
                        console.log(response.Errors);
                        MessageBox.Show("An error occured while adding assignment to list");
                    }
                },
                error: function () {
                    $("#ButtonAddNewAssignment").prop("disabled", true);
                    MessageBox.Show("An error occured while posting data to server");
                }
            })
        });
    },
    DeleteAssignment: function (id) {
        var userId = $("#ID").val();
        var assignmentId = id;
        $.ajax({
            url: "/CoreApi/DeleteUserAssignment",
            method: "POST",
            data: {
                userId: userId,
                assignmentId: assignmentId
            },
            beforeSend: function () {
                $("#ButtonAddNewAssignment").prop("disabled", true);

            },
            success: function (response) {
                $("#ButtonAddNewAssignment").prop("disabled", false);

                if (response.Success) {
                    Ajax.LoadAssignments();

                    Page.AssignmentTable();
                    
                    MessageBox.Show("Assignment has been removed from student list");
                } else {
                    console.log(response.Errors);
                    MessageBox.Show("An error occured while removing assignment from the list");
                }
            },
            error: function () {
                $("#ButtonAddNewAssignment").prop("disabled", true);
                MessageBox.Show("An error occured while removing data in the server");
            }
        })
    },
    SaveEdit: function () {
        var AssignmentScoreEdit = $("#AssignmentScoreEdit").val();
        var assignmentId = $("#AssignmentIdHidden").val();

        if (AssignmentScoreEdit.trim() === '') {
            MessageBox.Show('Score cannot be empty');
        } else {

            $.ajax({
                url: "/CoreApi/EditStudentAssignmentScore",
                method: "POST",
                data: {
                    assignmentId: assignmentId,
                    score: AssignmentScoreEdit,
                },
                success: function (response) {
                    $("#ModalEditScore").modal("hide");
                    Ajax.LoadSubmittedAssignment();
                    if (response.Success) {
                        MessageBox.Show("Score has been updated successfully");
                    } else {
                        MessageBox.Show("An error occured when updating score");
                        console.log(response.Errors);
                    }

                },
                error: function () {
                    MessageBox.Show("An error occured while updating data in the server");
                }
            })
        }

    },
    ShowEditScore: function (id,data) {
        $("#AssignmentIdHidden").val(id);
        $("#AssignmentScoreEdit").val(data);
        $("#ModalEditScore").modal({
            backdrop: "static"
        });
    }
}
$(document).ready(function () {
    Buttons.Init();
    Page.Init();
    Ajax.Init();
});