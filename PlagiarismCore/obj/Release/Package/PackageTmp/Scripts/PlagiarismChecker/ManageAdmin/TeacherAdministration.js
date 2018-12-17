let Ajax = {
    LoadStudents: function () {
        var url = "/CoreAPI/GetAllTeachers";
        $("#TeacherTable").DataTable({
            "ajax": url,
            "columns": [
                { "data": "FullName" },
                { "data": "Email" },
                { "data": "TeacherId" },
                { "data": "TotalClasses" },
                { "data": "TotalSubjects" },
                {
                    "data": "Id",
                    "render": function (data, type, row, meta) {
                        if (type === 'display') {
                            data = '<a class="btn btn-info btn-sm" href="/ManageAdmin/TeacherDetail/' + data + '">Edit</a>';
                        }
                        return data;
                    }
                }
            ],
            "scrollX": true
        });
    }
}
$(document).ready(function () {
    Ajax.LoadStudents();
});