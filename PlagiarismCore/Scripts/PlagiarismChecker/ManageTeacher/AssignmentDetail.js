let Consts = {
    GetPageID: function () {
        return $("#UserID").val();
    },
    GetSubjectID: function () {
        return $("#ID").val();
    }
}
let Ajax = {
    Init: function () {
        this.LoadClasses1();
        this.LoadClasses2();
        this.LoadClasses3();
    },
    LoadClasses1: function () {
        var url = "/CoreApi/GetTeacherClasses?teacherId=" + Consts.GetPageID();
        $("#SelectClasses1").html("");
        $.ajax({
            url: url,
            beforeSend: function () {
                console.log("Retrieving data from " + url);
            },
            success: function (response) {
                if (response && response.data) {
        
                    $.each(response.data, (idx, val) => {
                        var options = "<option value='" + val.Id + "'>" + val.ClassName + "</option>";
                        $("#SelectClasses1").append(options);
                    });
                    if (response.data.length > 0) {
                        $("#SelectClasses1").val(response.data[0].Id).change();
                    }
                }
            },
            error: function () {
                console.log("An error occured while retrieving data from " + url);
                MessageBox.Show("An error occured while retrieving data from " + url);
            }
        })
    },
    LoadClasses2: function () {
        var url = "/CoreApi/GetTeacherClasses?teacherId=" + Consts.GetPageID();
        $("#SelectClasses2").html("");
        $.ajax({
            url: url,
            beforeSend: function () {
                console.log("Retrieving data from " + url);
            },
            success: function (response) {
                if (response && response.data) {
                    $.each(response.data, (idx, val) => {
                        var options = "<option value='" + val.Id + "'>" + val.ClassName + "</option>";
                        $("#SelectClasses2").append(options);
                    });
                    if (response.data.length > 0) {
                        $("#SelectClasses2").val(response.data[0].Id).change();
                    }
                }
            },
            error: function () {
                console.log("An error occured while retrieving data from " + url);
                MessageBox.Show("An error occured while retrieving data from " + url);
            }
        })
    },
    LoadClasses3: function () {
        var url = "/CoreApi/GetTeacherClasses?teacherId=" + Consts.GetPageID();
        $("#SelectClasses3").html("");
        $.ajax({
            url: url,
            beforeSend: function () {
                console.log("Retrieving data from " + url);
            },
            success: function (response) {
                if (response && response.data) {
                    $.each(response.data, (idx, val) => {
                        var options = "<option value='" + val.Id + "'>" + val.ClassName + "</option>";
                        $("#SelectClasses3").append(options);
                    });
                    if (response.data.length > 0) {
                        $("#SelectClasses3").val(response.data[0].Id).change();
                    }
                }
            },
            error: function () {
                console.log("An error occured while retrieving data from " + url);
                MessageBox.Show("An error occured while retrieving data from " + url);
            }
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
    GetAssignedStudents: function () {
        var url = "/CoreAPI/GetSubjectAssignedStudents?classId=" + $("#SelectClasses1").val() + "&subjectId=" + Consts.GetSubjectID();
        if ($.fn.DataTable.isDataTable("#TableAssignedStudents")) {
            $('#TableAssignedStudents').DataTable().clear().destroy();
        }
        $("#TableAssignedStudents").DataTable({
            ajax: url,
            columns: [
                { data: "FullName" },
                { data: "AssignedClass" },
                {
                    data: "ID",
                    "render": function (data, type, row, meta) {
                        if (type === "display") {
                            data = "<button onclick='Buttons.RemoveStudent(\"" + data + "\");' class='btn btn-sm btn-danger btn-remove-assignment' data-id='" + data + "'>Delete</button>";
                        }
                        return data;
                    }
                }
            ]
        })
    },
    GetUnassignedStudents: function () {
        var url = "/CoreAPI/GetSubjectUnassignedStudents?classId=" + $("#SelectClasses2").val() + "&subjectId=" + Consts.GetSubjectID();
        if ($.fn.DataTable.isDataTable("#TableUnAssignedStudents")) {
            $('#TableUnAssignedStudents').DataTable().clear().destroy();
        }
        $("#TableUnAssignedStudents").DataTable({
            ajax: url,
            columns: [
                {
                    data: "ID",
                    "render": function (data, type, row, meta) {
                        if (type === "display") {
                            data = "<input class='check-student' data-id='" + data + "' type='checkbox'/>";
                        }
                        return data;
                    }
                },
                { data: "FullName" },
                { data: "AssignedClass" },
            ]
        })
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
    },
    GetSubmittedAssignments: function () {
        if ($.fn.DataTable.isDataTable("#TableSubmittedAssignment")) {
            $('#TableSubmittedAssignment').DataTable().clear().destroy();
        }
        var id = $("#ID").val();
        var url = "/CoreAPI/GetSubmittedAssignmentsByClass?classId=" + $("#SelectClasses3").val() + "&subjectId=" + Consts.GetSubjectID();
        $("#TableSubmittedAssignment").DataTable({
            ajax: url,
            columns: [
                {
                    data: "StudentName",
                    render: function (data, type, row) {
                        if (type === 'display') {
                            var link = "/ManageAdmin/StudentDetail/" + row.StudentId;
                            data = "<a href='" + link + "' target='_blank'>" + data + "</a>";
                        }
                        return data;
                    }
                },
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
                    }
                },
                { data: "PercentageInteger" },
                { data: "Description" },
                { data: "Status" },
                { data: "Score" },
                { data: "ScoreStatus" },
                { data: "Note" },
                {
                    data: "SubmissionDate",
                    render: function (data, type) {
                        if (type === 'display')
                            data = moment(data).format('LLL');
                        return data;
                    }
                },
                { data: "IsChecked" }

            ],
            columnDefs: [
                {
                    render: function (data, type, full, meta) {
                        return "<div class='text-wrap width-250'>" + data + "</div>";
                    },
                    targets: 11
                }
            ],
            info: false,
            paging: true,
            scrollX: true
        });
    }
}
let Buttons = {
    Init: function () {
        this.ButtonAddToSubject();
    },
    RemoveStudent: function (id) {
        var userId = id;
        var subjectId = Consts.GetSubjectID();
        var url = "/CoreAPI/DeleteUserAssignment";

        $.ajax({
            url: url,
            data: {
                userId: userId,
                assignmentId: subjectId
            },
            method: 'POST',
            beforeSend: function () {
                console.log("Posting data to " + url);
            },
            success: function (response) {
                if (response.Success) {
                    Page.GetAssignedStudents();
                    Page.GetUnassignedStudents();
                    MessageBox.Show("Student has been removed from current Subject");
                } else {
                    if (response.Errors) {
                        var error = "";
                        $.each(response.Errors, (idx, val) => {
                            error += val + "\n";
                        });
                        if (error === '') {
                            error = "An error occured while deleting data to server";
                        }
                        MessageBox.Show(error);
                    }
                }
            },
            error: function () {
                MessageBox.Show("Network error while requesting POST Request");
            }
        })
    },
    ButtonAddToSubject: function () {
        $("#ButtonAddToSubject").click(function () {
            var ids = [];
            $.each($(".check-student:checked"), (idx, val) => {
                ids.push($(val).attr("data-id"));
            });
            var classId = $("#SelectClasses2").val();
            var subjectId = Consts.GetSubjectID();
            var url = "/CoreAPI/PostStudentsToSubject/";
            $.ajax({
                url: url,
                data: {
                    ids: ids,
                    classId: classId,
                    subjectId: subjectId
                },
                method: 'POST',
                beforeSend: function () {
                    console.log("Posting data to " + url);
                },
                success: function (response) {
                    if (response.Success) {
                        Page.GetAssignedStudents();
                        Page.GetUnassignedStudents();
                        MessageBox.Show("Student(s) have been added to current Subject");

                    } else {
                        MessageBox.Show("An error occured while posting data to server");
                    }

                },
                error: function () {
                    MessageBox.Show("Network error!");
                }
            });
        });
    }

}
$(document).ready(function () {
    Buttons.Init();
    Ajax.Init();
    Page.Init();
});