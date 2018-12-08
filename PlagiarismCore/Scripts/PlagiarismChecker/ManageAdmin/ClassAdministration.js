let Ajax = {
    Init: function () {
        this.LoadClasses();
    },
    LoadClasses: function () {
        if ($.fn.DataTable.isDataTable("#ClassTable")) {
            $('#ClassTable').DataTable().clear().destroy();
        }
        var url = "/CoreAPI/GetAllClasses";
        $("#ClassTable").DataTable({
            ajax: url,
            columns: [
                { data: "ClassName" },
                { data: "TotalStudents" },
                {
                    data: "Id",
                    render: function (data, type, row) {
                        if (type === 'display') {
                            data = "<button class='btn btn-sm btn-primary' onclick='Buttons.ShowEdit(\"" + data + "\",\"" + row.ClassName + "\")'>Edit</button>" +
                                "  <button class='btn btn-sm btn-success' onclick='Buttons.ShowRelatedStudents(\"" + data + "\")'>Show Students</button>"
                                ;
                        }
                        return data;
                    }
                }
            ],
            scrollY: "400px",
            scrollCollapse: true,
            searching: false,
            search: false,
            paging: false,
            info: false,
            ordering: false
        });
    },
    LoadRelatedStudents: function (id) {
        var url = "/CoreAPI/GetAllStudentClasses/" + id;
        if ($.fn.DataTable.isDataTable("#StudentRelatedTable")) {
            $('#StudentRelatedTable').DataTable().clear().destroy();
        }
        $("#StudentRelatedTable").DataTable({
            ajax: url,
            columns: [
                { data: "FullName" },
                { data: "Email" },
                { data: "StudentID" }
            ],
            scrollY: "400px",
            scrollCollapse: true,
            scrollX: true
        })
    }
}

let Buttons = {
  
    SaveEdit: function () {
        var classNameEdit = $("#ClassNameEdit").val();
        var classId = $("#ClassIdHidden").val();

        if (classNameEdit.trim() === '') {
            alert('Class name cannot be empty');
        } else {

            $.ajax({
                url: "/CoreApi/EditClass",
                method: "POST",
                data: {
                    Id: classId,
                    ClassName: classNameEdit,
                },
                success: function (response) {
                    $("#ModalEditClass").modal("hide");
                    Ajax.LoadClasses();
                    if (response.Success) {
                        alert("Class has been updated successfully");
                    } else {
                        alert("An error occured when updating class");
                        console.log(response.Errors);
                    }

                },
                error: function () {
                    alert("An error occured while updating class in the server");
                }
            })
        }

    },
    ShowEdit: function (id, data) {
        $("#ClassIdHidden").val(id);
        $("#ClassNameEdit").val(data);
        $("#ModalEditClass").modal({
            backdrop: "static"
        });
    },
    RemoveClass: function () {
        var id = $("#ClassIdHidden").val();

        $.ajax({
            url: "/CoreApi/DeleteClass",
            method: "POST",
            data: {
                id: id,
            },
            success: function (response) {
                $("#ModalEditClass").modal("hide");
                Ajax.LoadClasses();
                if (response.Success) {
                    alert("Class has been removed successfully");
                } else {
                    alert("An error occured when removing class");
                    console.log(response.Errors);
                }
            },
            error: function () {
                alert("An error occured while removing class in the server");
            }
        })

    },
    ShowRelatedStudents: function (id) {
        $("#ModalShowStudents").modal({
            backdrop: "static"
        });
        setTimeout(() =>
            Ajax.LoadRelatedStudents(id),1000)
    }
}
let Page = {
    Init: function () {
        this.CheckMessage();
    },
    CheckMessage: function () {
        var message = $("#Message").val();
        if (message && message !== "") {
            alert(message);
        }
    }
 
}
$(document).ready(function () {
    Ajax.Init();
    Page.Init();
});