let Ajax = {
    Init: function () {
        this.LoadAssignments();
    },
    LoadAssignments: function () {

        var url = "/CoreAPI/GetAllAssignments";
        $.ajax({
            url: url,
            beforeSend: function () {
                console.log(".....Requesting data from " + url);
            },
            success: function (response) {
                console.log(".....Data successfully retrieved from " + url);
                $("#AssignmentId").empty();
                if (response.data.length > 0) {

                    $.each(response.data, (index, value) => {
                        $("#AssignmentId").append("<option value='" + value.Id + "'>" + value.AssignmentName+ "</option>");
                    });

                }

            },
            error: function () {
                console.log(".....Data failed to receive from " + url);
            }
        });
    },
}

$(document).ready(function () {
    Ajax.Init();
});