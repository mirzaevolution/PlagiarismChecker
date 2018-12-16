let Ajax = {
    LoadStudents: function () {
        var url = "/CoreAPI/GetStudents";
        $("#StudentTable").DataTable({
            "ajax": url,
            "columns": [
                {"data":"FullName"},
                {"data":"Email"},
                {"data":"StudentID"},
                {"data":"AssignedClass"},
                { "data": "Assignments" },
                {
                    "data":"ID",
                    "render": function (data,type,row,meta) {
                        if (type === 'display') {
                            data= '<a class="btn btn-info btn-sm" href="/ManageAdmin/StudentDetail/' + data + '">Edit</a>';
                        }
                        return data;
                    }
                }
            ],
            "scrollX":true
        });
    }
}
$(document).ready(function () {
    Ajax.LoadStudents();
});