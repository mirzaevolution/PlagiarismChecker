let Ajax = {
    Init: function () {
        this.LoadAssignments();
    },
    LoadAssignments: function () {
        var url = "/CoreAPI/GetAllAssignments";
        $("#AssignmentTable").DataTable({
            ajax: url,
            columns: [
                { data: "AssignmentName" },
                {
                    data: "Id",
                    render: function (data, type) {
                        if (type === 'display') {
                            data = "<button class='btn btn-info' onclick='Buttons.ShowEdit(\"" + data + "\")'>Edit</button>";
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
        var id = $("#ID").val();
        var url = "/CoreAPI/GetStudentSubmittedAssignments/" + id;
        $("#SubmittedAssignmentTable").DataTable({
            ajax: url,
            columns: [
                { data: "AssignmentName" },
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
                }
            ],
            info: false,
            paging: false
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

                        alert("Assignment has been added to student list");
                    } else {
                        console.log(response.Errors);
                        alert("An error occured while adding assignment to list");
                    }
                },
                error: function () {
                    $("#ButtonAddNewAssignment").prop("disabled", true);
                    alert("An error occured while posting data to server");
                }
            })
        });
    },
    DeleteAssignment: function (id) {
        var userId = $("#ID").val();
        var assignmentId = id;
        $.ajax({
            url: "/CoreApi/DeleteAssignment",
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

                    alert("Assignment has been removed from student list");
                } else {
                    console.log(response.Errors);
                    alert("An error occured while removing assignment from the list");
                }
            },
            error: function () {
                $("#ButtonAddNewAssignment").prop("disabled", true);
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