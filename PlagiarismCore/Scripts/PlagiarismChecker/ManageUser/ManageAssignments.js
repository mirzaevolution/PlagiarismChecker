let Ajax = {

    Init: function () {
        this.LoadSubmittedAssignment();
    },
    LoadSubmittedAssignment: function () {
        var id = $("#IDHidden").val();
        var url = "/CoreAPI/GetStudentSubmittedAssignments/" + id;
        if ($.fn.DataTable.isDataTable("#SubmittedAssignmentTable")) {
            $('#SubmittedAssignmentTable').DataTable().clear().destroy();
        }
        $("#SubmittedAssignmentTable").DataTable({
            ajax: url,
            columns: [
                { data: "AssignmentName" },
                { data: "Title" },
                { data: "Counter" },
                { data: "Description" },
                { data: "PercentageInteger" },
                { data: "Status" },


                { data: "Score" },
                {
                    data: "UploadedFilePath",
                    render: function (data, type, row) {
                        if (type === 'display') {
                            data = "<a href='" + data + "' target='_blank'>Download File</a>";
                        }
                        return data;
                    }
                },
                { data: "IsChecked" },
                {
                    data: "SubmissionDate",
                    render: function (data, type) {
                        if (type === 'display')
                            data = moment(data).format('LLL');
                        return data;
                    }
                }

            ],
            scrollX: true,
            info: false,
            paging: true
        });
    }
}
$(document).ready(function () {
    Ajax.LoadSubmittedAssignment();
});