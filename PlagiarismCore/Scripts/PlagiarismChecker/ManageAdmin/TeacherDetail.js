let Consts = {
    GetPageId: function () {
        return $("#Id").val();
    }
}
let Ajax = {
    Init: function () {
        this.LoadAvailableClasses();
        this.LoadCurrentClasses();
        this.LoadAvailableSubjects();
        this.LoadCurrentSubjects();
    },
    LoadAvailableClasses: function () {
        var url = "/CoreAPI/GetAvailableTeacherClasses/?teacherId=" + Consts.GetPageId();
        $("ClassSelect").html("");
        $.ajax({
            url: url,
            beforeSend: function () {
                console.log("Retrieving data from " + url);
            },
            success: function (response) {
                if (response && response.data) {
                    $.each(response.data, (idx, val) => {
                        var option = "<option value='" + val.Id + "'>" + val.ClassName + "</option>";
                        $("#ClassSelect").append(option);
                    });
                }
            },
            error: function () {
                console.log("An error occured while getting data from " + url);
                MessageBox.Show("An error occured while getting data from " + url);
            }
        });
    },
    LoadCurrentClasses: function () {
        var url = "/CoreAPI/GetTeacherClasses/?teacherId=" + Consts.GetPageId();
        if ($.fn.DataTable.isDataTable("#ClassTable")) {
            $('#ClassTable').DataTable().clear().destroy();
        }
        $("#ClassTable").DataTable({
            ajax: url,
            columns: [
                { "data": "ClassName" },
                {
                    "data": "Id",
                    "render": function (data, type, row, meta) {
                        if (type === "display") {
                            data = "<button onclick='Buttons.DeleteClass(\"" + data + "\");' class='btn btn-danger btn-remove-assignment' data-id='" + data + "'>Delete</button>";
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
    },
    LoadAvailableSubjects: function () {
        var url = "/CoreAPI/GetAvailableTeacherSubjects/?teacherId=" + Consts.GetPageId();
        $("#SubjectSelect").html("");
        $.ajax({
            url: url,
            beforeSend: function () {
                console.log("Retrieving data from " + url);
            },
            success: function (response) {
                if (response && response.data) {
                    $.each(response.data, (idx, val) => {
                        var option = "<option value='" + val.Id + "'>" + val.AssignmentName + "</option>";
                        $("#SubjectSelect").append(option);
                    });
                }
            },
            error: function () {
                console.log("An error occured while getting data from " + url);
                MessageBox.Show("An error occured while getting data from " + url);
            }
        });
    },
    LoadCurrentSubjects: function () {
        var url = "/CoreAPI/GetTeacherSubjects/?teacherId=" + Consts.GetPageId();
        if ($.fn.DataTable.isDataTable("#SubjectTable")) {
            $('#SubjectTable').DataTable().clear().destroy();
        }
        $("#SubjectTable").DataTable({
            ajax: url,
            columns: [
                { "data": "AssignmentName" },
                {
                    "data": "Id",
                    "render": function (data, type, row, meta) {
                        if (type === "display") {
                            data = "<button onclick='Buttons.DeleteSubject(\"" + data + "\");' class='btn btn-danger btn-remove-assignment' data-id='" + data + "'>Delete</button>";
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

let Page = {
    Init: function () {
        this.CheckMessage();
    },
    CheckMessage: function () {
        var message = $("#Message").val();
        if (message && message !== "") {
            MessageBox.Show(message);
        }
    },
}

let Buttons = {
    Init: function () {
        $("#ButtonDelete").click(function () {
            $("#FormDelete").submit();
        });
        this.ButtonAddNewClass();
        this.ButtonAddNewSubject();
    },
    ButtonAddNewClass: function () {
        $("#ButtonAddNewClass").click(function () {
            var selected = $("#ClassSelect").val();
            if (selected && selected !=='') {
                var teacherId = Consts.GetPageId();
                var classId = selected;
                var url = "/CoreAPI/PostTeacherClass";
                $.ajax({
                    url: url,
                    data: {
                        userId: teacherId,
                        classId: classId
                    },
                    method:"POST",
                    beforeSend: function () {
                        $("#ButtonAddNewClass").prop("disabled", true);
                        console.log("Posting data to " + url);
                    },
                    success: function (response) {

                        if (response) {
                            if (response.Success) {
                                Ajax.LoadAvailableClasses();
                                Ajax.LoadCurrentClasses();
                                MessageBox.Show("A new class has been assigned successfully");
                            } else {
                                var error = "";
                                $.each(response.Errors, (index, value) => {
                                    error += value += "\n";
                                });
                                if (error === '') {
                                    error = "An error occured while posting data to " + url;
                                }
                                MessageBox.Show(error);
                            }
                        }
                        $("#ButtonAddNewClass").prop("disabled", false);

                    },
                    error: function () {
                        $("#ButtonAddNewClass").prop("disabled", false);

                        console.log("An error occured while posting data to " + url);
                        MessageBox.Show("An error occured while posting data to " + url);

                    }
                })
            } else {
                MessageBox.Show("Please select Available Class first!");
            }
        });
    },
    ButtonAddNewSubject: function () {
        $("#ButtonAddNewSubject").click(function () {
            var subjectId = $("#SubjectSelect").val();
            var classId = $("#ClassSelect").val();

            if (subjectId && subjectId !== '' && classId && classId !=='') {
                var teacherId = Consts.GetPageId();
                var url = "/CoreAPI/PostTeacherSubject";
                $.ajax({
                    url: url,
                    data: {
                        userId: teacherId,
                        classId: classId,
                        subjectId: subjectId
                    },
                    method: "POST",
                    beforeSend: function () {
                        $("#ButtonAddNewSubject").prop("disabled", true);
                        console.log("Posting data to " + url);
                    },
                    success: function (response) {

                        if (response) {
                            if (response.Success) {
                                Ajax.LoadAvailableSubjects();
                                Ajax.LoadCurrentSubjects();
                                MessageBox.Show("A new subject has been assigned successfully");
                            } else {
                                var error = "";
                                $.each(response.Errors, (index, value) => {
                                    error += value += "\n";
                                });
                                if (error === '') {
                                    error = "An error occured while posting data to " + url;
                                }
                                MessageBox.Show(error);
                            }
                        }
                        $("#ButtonAddNewSubject").prop("disabled", false);

                    },
                    error: function () {
                        $("#ButtonAddNewSubject").prop("disabled", false);

                        console.log("An error occured while posting data to " + url);
                        MessageBox.Show("An error occured while posting data to " + url);

                    }
                })
            } else {
                MessageBox.Show("Please select Available Subject or Class first!");
            }
        });
    },
    DeleteClass: function (id) {
        var userId = Consts.GetPageId();
        var classId = id;
        $.ajax({
            url: "/CoreApi/DeleteTeacherClass",
            method: "POST",
            data: {
                userId: userId,
                classId: classId
            },

            beforeSend: function () {
                $("#ButtonAddNewClass").prop("disabled", true);

            },
            success: function (response) {
                $("#ButtonAddNewClass").prop("disabled", false);

                if (response.Success) {
                    Ajax.LoadAvailableClasses();
                    Ajax.LoadCurrentClasses();

                    MessageBox.Show("Class has been removed from teacher list");
                } else {
                    var error = "";
                    $.each(response.Errors, (index, value) => {
                        error += value += "\n";
                    });
                    if (error === '') {
                        error = "An error occured while posting data to " + url;
                    }
                    MessageBox.Show(error);
                }
            },
            error: function () {
                $("#ButtonAddNewClass").prop("disabled", true);
                MessageBox.Show("An error occured while removing data in the server");
            }
        })
    },
    DeleteSubject: function (id) {
        var userId = Consts.GetPageId();
        var subjectId = id;
        var classId = $("#ClassSelect").val();
        $.ajax({
            url: "/CoreApi/DeleteTeacherSubject",
            method: "POST",
            data: {
                userId: userId,
                classId: classId,
                subjectId: subjectId
            },
            beforeSend: function () {
                $("#ButtonAddNewSubject").prop("disabled", true);

            },
            success: function (response) {
                $("#ButtonAddNewSubject").prop("disabled", false);

                if (response.Success) {
                    Ajax.LoadAvailableSubjects();
                    Ajax.LoadCurrentSubjects();

                    MessageBox.Show("Assignment has been removed from teacher list");
                } else {
                    var error = "";
                    $.each(response.Errors, (index, value) => {
                        error += value += "\n";
                    });
                    if (error === '') {
                        error = "An error occured while posting data to " + url;
                    }
                    MessageBox.Show(error);
                }
            },
            error: function () {
                $("#ButtonAddNewSubject").prop("disabled", true);
                MessageBox.Show("An error occured while removing data in the server");
            }
        })
    }
}

$(document).ready(function () {
    Ajax.Init();
    Page.Init();
    Buttons.Init();
});