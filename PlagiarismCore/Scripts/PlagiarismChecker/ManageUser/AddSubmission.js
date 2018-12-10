let Ajax = {
    Init: function () {
        this.LoadAssignments();
    },
    LoadAssignments: function () {

        var url = "/CoreAPI/GetAssignments";
        var id = $("#StudentId").val();
        $.ajax({
            url: url,
            data:{
                id: id,
                except:false
            },
            beforeSend: function () {
                console.log(".....Requesting data from " + url);
            },
            success: function (response) {
                console.log(".....Data successfully retrieved from " + url);
                $("#AssignmentId").empty();
                if (response.length > 0) {

                    $.each(response, (index, value) => {
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